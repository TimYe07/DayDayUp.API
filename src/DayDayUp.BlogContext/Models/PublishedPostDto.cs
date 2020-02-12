using System;

namespace DayDayUp.BlogContext.Models
{
    public class PublishedPostDto
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Category { get; set; }
        public string[] Tags { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTimeOffset CreateOn { get; set; }
        public DateTimeOffset? PublishOn { get; set; }
        public DateTimeOffset? UpdateOn { get; set; }
    }
}