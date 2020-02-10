using System;
using System.Threading;
using System.Threading.Tasks;
using DayDayUp.BlogContext.Commands.Categories;
using DayDayUp.BlogContext.Repositories;
using DayDayUp.BlogContext.Services;
using DayDayUp.BlogContext.ValueObject;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DayDayUp.BlogContext.Commands.Tags
{
    public class DeleteTagCommandHandler : IRequestHandler<DeleteCategoryCommand, OperationResult>
    {
        public DeleteTagCommandHandler
        (
            ITagRepository tagRepo,
            ILogger<DeleteTagCommandHandler> logger)
        {
            _tagRepo = tagRepo;
            _logger = logger;
        }

        private readonly ITagRepository _tagRepo;
        private readonly ILogger<DeleteTagCommandHandler> _logger;

        public async Task<OperationResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var tag = _tagRepo.Find(c => c.Id == request.Id);
            if (tag == null)
            {
                return OperationResult.Fail("该标签不存在或已删除，请重试。");
            }

            _tagRepo.Delete(tag);
            try
            {
                await _tagRepo.UnitOfWork.SaveChangesAsync(cancellationToken);
                return OperationResult.Succeed();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return OperationResult.Fail($"删除标签出错了，请重试。");
            }
        }
    }
}