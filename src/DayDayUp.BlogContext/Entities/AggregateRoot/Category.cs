using System.Collections.Generic;
using DayDayUp.BlogContext.SeedWork;

namespace DayDayUp.BlogContext.Entities.AggregateRoot
{
    public class Category : BaseEntity, IAggregateRoot
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}