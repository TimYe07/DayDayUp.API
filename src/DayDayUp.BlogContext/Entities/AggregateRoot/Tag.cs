using System.Collections.Generic;
using DayDayUp.BlogContext.SeedWork;

namespace DayDayUp.BlogContext.Entities.AggregateRoot
{
    public class Tag : BaseEntity, IAggregateRoot
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public List<PostTag> PostTags { get; set; }
    }
}