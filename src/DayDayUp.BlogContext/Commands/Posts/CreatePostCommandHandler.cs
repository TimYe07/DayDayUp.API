using System;
using System.Threading;
using System.Threading.Tasks;
using DayDayUp.BlogContext.Entities.AggregateRoot;
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
            var slugTask = _textConversion.GenerateSlugAsync(request.Title);
            var summaryTask = _textConversion.GenerateSummaryAsync(request.Content);
            var categoryTask = _postDomainService.GetOrCreateCategoryAsync(request.Category);
            var tagTask = _postDomainService.GetOrCreateTagAsync(request.Tags);

            var post = new Post();
            post.SetOrUpdateTitle(request.Title);
            post.SetOrUpdatePrivate(request.IsPrivate);
            post.SetOrUpdateCategory(await categoryTask);
            post.SetOrUpdateTags(await tagTask);
            post.SetOrUpdateSlug(await slugTask);
            post.SetOrUpdateDesc(await summaryTask);
            post.SetOrUpdateContent(await textDocumentTask);

            if (request.IsDraft)
                post.SaveAsDraft();
            post.Publish();

            try
            {
                _postRepository.Insert(post);
                await _postRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return OperationResult.Succeed(post.Slug);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return OperationResult.Fail("添加文章出错了，请重试。");
            }
        }
    }
}