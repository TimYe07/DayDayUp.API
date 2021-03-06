using DayDayUp.BlogContext.ValueObject;
using MediatR;

namespace DayDayUp.BlogContext.Commands.Tags
{
    public class UpdateTagCommand : IRequest<OperationResult>
    {
        public UpdateTagCommand
        (
            long id,
            string name,
            string slug,
            bool isGenerateSlug)
        {
            Id = id;
            Name = name;
            Slug = slug;
            IsGenerateSlug = isGenerateSlug;
        }

        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Slug { get; private set; }

        /// <summary>
        /// 是否自动生成 slug
        /// </summary>
        public bool IsGenerateSlug { get; private set; }
    }
}