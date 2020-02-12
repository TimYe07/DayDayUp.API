using System;
using System.Threading;
using System.Threading.Tasks;
using DayDayUp.BlogContext.Repositories;
using DayDayUp.BlogContext.Services;
using DayDayUp.BlogContext.ValueObject;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DayDayUp.BlogContext.Commands.Posts
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, OperationResult>
    {
        public DeletePostCommandHandler
        (
            IPostRepository postRepository,
            ILogger<DeletePostCommandHandler> logger)
        {
            _postRepository = postRepository;
            _logger = logger;
        }

        private readonly IPostRepository _postRepository;
        private readonly ILogger<DeletePostCommandHandler> _logger;

        public async Task<OperationResult> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = _postRepository.Find(p => p.Id == request.Id);
            if (post == null)
            {
                return OperationResult.Fail("该文章不存在或被删除，请刷新后重试。");
            }

            post.Deleted();

            try
            {
                _postRepository.Delete(post);
                await _postRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return OperationResult.Succeed();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return OperationResult.Fail("删除文章出错了，请重试。");
            }
        }
    }
}