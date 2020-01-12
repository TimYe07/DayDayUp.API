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
            var isExits = _categoryRepo.Any(c => c.Name == request.Name);
            if (isExits)
            {
                return OperationResult.Fail("该分类已存在，请勿重复添加。");
            }

            var categorySlug = await _textConversion.GenerateSlugAsync(request.Name);
            if (string.IsNullOrEmpty(categorySlug))
            {
                categorySlug = request.Name;
            }

            var category = new Category
            {
                Name = request.Name,
                Slug = categorySlug
            };

            _categoryRepo.Insert(category);

            try
            {
                await _categoryRepo.UnitOfWork.SaveChangesAsync(cancellationToken);
                return OperationResult.Succeed();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return OperationResult.Fail($"添加分类'{request.Name}'出错了，请重试。");
            }
        }
    }
}