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

            var post = new Post();
            
            post.SetOrUpdateTitle(request.Title);
            post.SetOrUpdateCreateOn(request.CreateOn);
            post.SetOrUpdateUpdateOn(request.UpdateOn);

            var slug = string.IsNullOrEmpty(request.Slug) ? post.Id.EncodeLongId(request.Title) : request.Slug;
            post.SetOrUpdateSlug(slug);

            var categoryName = request.Category ?? "其他";
            var category = await _postDomainService.GetOrCreateCategoryAsync(categoryName);
            post.SetOrUpdateCategory(category);
            
            if (request.Tags.Any())
            {
                var tags = await _postDomainService.GetOrCreateTagAsync(request.Tags);
                post.SetOrUpdateTags(tags);
            }

            post.SetOrUpdateContent(await textDocumentTask);
            
            var desc = Utils.GetPostExcerpt(post.ConvertedContent, 200);
            post.SetOrUpdateDesc(desc);
            
            var keywords = _textConversion.ExtractKeywords(Utils.RemoveTags(post.ConvertedContent), 5);
            post.SetOrUpdateKeywords(keywords);
            
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