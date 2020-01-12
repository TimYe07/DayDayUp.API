using System;

namespace DayDayUp.BlogContext.Models
{
    public class PostDto
    {
        public string Title { get; set; }
        public PostInCategoryDto Category { get; set; }
        public PostInTagDto[] Tags { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public int ViewCount { get; set; }
        public DateTimeOffset CreateOn { get; set; }
        public DateTimeOffset? PublishOn { get; set; }
        public DateTimeOffset? UpdateOn { get; set; }
    }
}