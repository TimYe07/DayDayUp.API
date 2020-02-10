using System;
using System.Threading;
using System.Threading.Tasks;
using DayDayUp.BlogContext.Repositories;
using DayDayUp.BlogContext.Services;
using DayDayUp.BlogContext.ValueObject;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace DayDayUp.BlogContext.Commands.Categories
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, OperationResult>
    {
        public DeleteCategoryCommandHandler
        (
            ICategoryRepository categoryRepo,
            ILogger<DeleteCategoryCommandHandler> logger)
        {
            _categoryRepo = categoryRepo;
            _logger = logger;
        }

        private readonly ICategoryRepository _categoryRepo;
        private readonly ILogger<DeleteCategoryCommandHandler> _logger;

        public async Task<OperationResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _categoryRepo.Find(c => c.Id == request.Id);
            if (category == null)
            {
                return OperationResult.Fail("该分类不存在或已删除，请重试。");
            }

            _categoryRepo.Delete(category);
            try
            {
                await _categoryRepo.UnitOfWork.SaveChangesAsync(cancellationToken);
                return OperationResult.Succeed();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return OperationResult.Fail($"删除分类出错了，请重试。");
            }
        }
    }
}