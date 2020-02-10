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
            var tag = new Tag();
            var tagIsExits = _tagRepo.Any(c => c.Name == request.Name);
            if (tagIsExits)
            {
                return OperationResult.Fail($"标签 '{request.Name}' 已存在，请勿重复添加。");
            }

            tag.SetOrUpdateName(request.Name);
            await tag.GenerateSlugAsync(_textConversion);

            var slugIsExits = _tagRepo.Any(c => c.Slug == tag.Slug);
            if (slugIsExits)
            {
                return OperationResult.Fail($"标签地址名 '{tag.Slug}' 已存在，请勿重复添加。");
            }

            _tagRepo.Insert(tag);

            try
            {
                await _tagRepo.UnitOfWork.SaveChangesAsync(cancellationToken);
                return OperationResult.Succeed();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return OperationResult.Fail($"添加标签 '{request.Name}' 出错了，请重试。");
            }
        }
    }
}