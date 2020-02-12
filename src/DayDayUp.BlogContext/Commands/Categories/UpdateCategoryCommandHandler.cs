using System;
using System.Threading;
using System.Threading.Tasks;
using DayDayUp.BlogContext.Repositories;
using DayDayUp.BlogContext.Services;
using DayDayUp.BlogContext.ValueObject;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DayDayUp.BlogContext.Commands.Categories
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, OperationResult>
    {
        public UpdateCategoryCommandHandler
        (
            ICategoryRepository categoryRepo,
            ITextConversionService textConversionService,
            ILogger<UpdateCategoryCommandHandler> logger)
        {
            _categoryRepo = categoryRepo;
            _textConversion = textConversionService;
            _logger = logger;
        }

        private readonly ICategoryRepository _categoryRepo;
        private readonly ITextConversionService _textConversion;
        private readonly ILogger<UpdateCategoryCommandHandler> _logger;

        public async Task<OperationResult> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _categoryRepo.Find(x => x.Id == request.Id);
            if (category == null)
            {
                return OperationResult.Fail("该分类不存在，请重试。");
            }

            category.SetOrUpdateName(request.Name);

            if (category.Slug != request.Slug && !_categoryRepo.Any(c => c.Slug == request.Slug))
            {
                category.SetOrUpdateSlug(request.Slug);
            }

            if (request.IsGenerateSlug)
            {
                await category.GenerateSlugAsync(_textConversion);
                var slugIsExits = _categoryRepo.Any(c => c.Slug == category.Slug);
                if (slugIsExits)
                {
                    return OperationResult.Fail($"分类地址名 '{category.Slug}' 已存在，请手动设置一个值。");
                }
            }

            _categoryRepo.Update(category);

            try
            {
                await _categoryRepo.UnitOfWork.SaveChangesAsync(cancellationToken);
                return OperationResult.Succeed();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return OperationResult.Fail($"更新分类 '{request.Name}' 出错了，请重试。");
            }
        }
    }
}