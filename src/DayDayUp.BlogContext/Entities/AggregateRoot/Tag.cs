using System.Collections.Generic;
using System.Threading.Tasks;
using DayDayUp.BlogContext.SeedWork;
using DayDayUp.BlogContext.Services;

namespace DayDayUp.BlogContext.Entities.AggregateRoot
{
    public class Tag : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 地址名
        /// </summary>
        public string Slug { get; set; }

        public List<PostTag> PostTags { get; set; }

        public void SetOrUpdateName(string name)
        {
            if (Name != name)
            {
                Name = name;
            }
        }

        public void SetOrUpdateSlug(string slug)
        {
            if (Slug != slug)
            {
                Slug = slug;
            }
        }

        public async Task GenerateSlugAsync(ITextConversionService textConversionService)
        {
            Slug = await textConversionService.GenerateSlugAsync(Name);
        }
    }
}