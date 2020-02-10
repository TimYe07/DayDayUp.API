using System;
using System.Threading;
using System.Threading.Tasks;
using DayDayUp.BlogContext.Entities.AggregateRoot;
using DayDayUp.BlogContext.Repositories;
using DayDayUp.BlogContext.Services;
using DayDayUp.BlogContext.ValueObject;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DayDayUp.BlogContext.Commands.Categories
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, OperationResult>
    {
        public CreateCategoryCommandHandler
        (
            ICategoryRepository categoryRepo,
            ITextConversionService textConversionService,
            ILogger<CreateCategoryCommandHandler> logger)
        {
            _categoryRepo = categoryRepo;
            _textConversion = textConversionService;
            _logger = logger;
        }

        private readonly ICategoryRepository _categoryRepo;
        private readonly ITextConversionService _textConversion;
        private readonly ILogger<CreateCategoryCommandHandler> _logger;

        public async Task<OperationResult> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category();
            var categoryIsExits = _categoryRepo.Any(c => c.Name == request.Name);
            if (categoryIsExits)
            {
                return OperationResult.Fail($"分类 '{request.Name}' 已存在，请勿重复添加。");
            }
            
            category.SetOrUpdateName(request.Name);
            category.GenerateSlugAsync(_textConversion);

            var slugIsExits = _categoryRepo.Any(c => c.Slug == category.Slug);
            if (slugIsExits)
            {
                return OperationResult.Fail($"分类地址名 '{category.Slug}' 已存在，请勿重复添加。");
            }

            _categoryRepo.Insert(category);

            try
            {
                await _categoryRepo.UnitOfWork.SaveChangesAsync(cancellationToken);
                return OperationResult.Succeed();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return OperationResult.Fail($"添加分类 '{request.Name}' 出错了，请重试。");
            }
        }
    }
}