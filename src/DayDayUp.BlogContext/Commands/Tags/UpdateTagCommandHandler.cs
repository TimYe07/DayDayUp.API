using System;
using System.Threading;
using System.Threading.Tasks;
using DayDayUp.BlogContext.Repositories;
using DayDayUp.BlogContext.Services;
using DayDayUp.BlogContext.ValueObject;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DayDayUp.BlogContext.Commands.Tags
{
    public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand, OperationResult>
    {
        public UpdateTagCommandHandler
        (
            ITagRepository tagRepo,
            ITextConversionService textConversionService,
            ILogger<UpdateTagCommandHandler> logger)
        {
            _tagRepo = tagRepo;
            _textConversion = textConversionService;
            _logger = logger;
        }

        private readonly ITagRepository _tagRepo;
        private readonly ITextConversionService _textConversion;
        private readonly ILogger<UpdateTagCommandHandler> _logger;

        public async Task<OperationResult> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = _tagRepo.Find(x => x.Id == request.Id);
            if (tag == null)
            {
                return OperationResult.Fail("该标签不存在，请重试。");
            }

            tag.SetOrUpdateName(request.Name);
            tag.SetOrUpdateSlug(request.Slug);

            if (request.IsGenerateSlug)
            {
                await tag.GenerateSlugAsync(_textConversion);
                var slugIsExits = _tagRepo.Any(c => c.Slug == tag.Slug);
                if (slugIsExits)
                {
                    return OperationResult.Fail($"标签地址名 '{tag.Slug}' 已存在，请手动设置一个值。");
                }
            }

            _tagRepo.Update(tag);

            try
            {
                await _tagRepo.UnitOfWork.SaveChangesAsync(cancellationToken);
                return OperationResult.Succeed();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return OperationResult.Fail($"更新标签 '{request.Name}' 出错了，请重试。");
            }
        }
    }
}