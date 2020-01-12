using System;
using System.Threading;
using System.Threading.Tasks;
using DayDayUp.BlogContext.Entities.AggregateRoot;
using DayDayUp.BlogContext.Repositories;
using DayDayUp.BlogContext.Services;
using DayDayUp.BlogContext.ValueObject;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DayDayUp.BlogContext.Commands.Tags
{
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, OperationResult>
    {
        public CreateTagCommandHandler
        (
            ITagRepository tagRepo,
            ITextConversionService textConversionService,
            ILogger<CreateTagCommandHandler> logger)
        {
            _tagRepo = tagRepo;
            _textConversion = textConversionService;
            _logger = logger;
        }

        private readonly ITagRepository _tagRepo;
        private readonly ITextConversionService _textConversion;
        private readonly ILogger<CreateTagCommandHandler> _logger;

        public async Task<OperationResult> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var isExits = _tagRepo.Any(c => c.Name == request.Name);
            if (isExits)
            {
                return OperationResult.Fail("该标签已存在，请勿重复添加。");
            }

            var tagSlug = await _textConversion.GenerateSlugAsync(request.Name);
            if (string.IsNullOrEmpty(tagSlug))
            {
                tagSlug = request.Name;
            }

            var category = new Tag
            {
                Name = request.Name,
                Slug = tagSlug
            };

            _tagRepo.Insert(category);

            try
            {
                await _tagRepo.UnitOfWork.SaveChangesAsync(cancellationToken);
                return OperationResult.Succeed();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return OperationResult.Fail($"添加标签'{request.Name}'出错了，请重试。");
            }
        }
    }
}