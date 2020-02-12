using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DayDayUp.BlogContext.Entities.AggregateRoot;
using DayDayUp.BlogContext.Extensions;
using DayDayUp.BlogContext.Repositories;
using DayDayUp.BlogContext.Services;
using DayDayUp.BlogContext.ValueObject;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DayDayUp.BlogContext.Commands.Posts
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, OperationResult>
    {
        public CreatePostCommandHandler
        (
            IPostRepository postRepository,
            IPostDomainService postDomainService,
            ITextConversionService textConversionService,
            ILogger<CreatePostCommandHandler> logger)
        {
            _postRepository = postRepository;
            _postDomainService = postDomainService;
            _textConversion = textConversionService;
            _logger = logger;
        }

        private readonly IPostRepository _postRepository;
        private readonly ITextConversionService _textConversion;
        private readonly IPostDomainService _postDomainService;
        private readonly ILogger<CreatePostCommandHandler> _logger;

        public async Task<OperationResult> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var textDocumentTask = _textConversion.ToMarkdownAsync(request.Content);
            var summaryTask = _textConversion.GenerateSummaryAsync(request.Content);

            var post = new Post();
            if (string.IsNullOrEmpty(request.Slug))
            {
                post.SetOrUpdateSlug(post.Id.EncodeLongId(request.Title));
            }
            else
            {
                post.SetOrUpdateSlug(request.Slug);
            }

            var category = await _postDomainService.GetOrCreateCategoryAsync(request.Category ?? "其他");
            post.SetOrUpdateCategory(category);
            if (request.Tags.Any())
            {
                var tags = await _postDomainService.GetOrCreateTagAsync(request.Tags);
                post.SetOrUpdateTags(tags);
            }

            post.SetOrUpdateTitle(request.Title);
            post.SetOrUpdateDesc(await summaryTask);
            post.SetOrUpdateContent(await textDocumentTask);
            post.SetOrUpdateCreateOn(request.CreateOn);
            post.SetOrUpdateUpdateOn(request.UpdateOn);

            try
            {
                _postRepository.Insert(post);
                await _postRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return OperationResult.Succeed(post.Id.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return OperationResult.Fail("添加文章出错了，请重试");
            }
        }
    }
}