using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DayDayUp.BlogContext.Commands.Posts;
using DayDayUp.BlogContext.Entities.AggregateRoot;
using DayDayUp.BlogContext.Extensions;
using DayDayUp.BlogContext.Repositories;
using DayDayUp.BlogContext.Services;
using DayDayUp.BlogContext.ValueObject;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DayDayUp.BlogContext.Commands
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, OperationResult>
    {
        public UpdatePostCommandHandler
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

        public async Task<OperationResult> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {

            var post = await _postRepository.FindAsync(p => p.Id == request.Id && !p.IsDeleted);
            if (post == null)
            {
                return OperationResult.Fail("该文章不存在或已删除，请刷新后重试。");
            }
            
            var textDocumentTask = _textConversion.ToMarkdownAsync(request.Content);

            post.SetOrUpdateTitle(request.Title);
            post.SetOrUpdateContent(await textDocumentTask);

            if (request.CreateOn != null)
            {
                post.SetOrUpdateCreateOn(request.CreateOn);
            }

            var updateOn = request.UpdateOn ?? DateTime.Now;
            post.SetOrUpdateUpdateOn(updateOn);

            if (request.Category != post.Category.Name)
            {
                var category = await _postDomainService.GetOrCreateCategoryAsync(request.Category);
                post.SetOrUpdateCategory(category);
            }
            
            
            
            var tags = await _postDomainService.GetOrCreateTagAsync(request.Tags);
            post.SetOrUpdateTags(tags);

            if (!string.IsNullOrEmpty(request.Slug))
            {
                post.SetOrUpdateSlug(request.Slug);
            }

            var desc = Utils.GetPostExcerpt(post.ConvertedContent, 200);
            post.SetOrUpdateDesc(desc);
            
            var keywords = _textConversion.ExtractKeywords(Utils.RemoveTags(post.ConvertedContent), 5);
            post.SetOrUpdateKeywords(keywords);

            try
            {
                _postRepository.Update(post);
                await _postRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return OperationResult.Succeed(post.Id.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return OperationResult.Fail("更新文章出错了，请重试");
            }
        }
    }
}