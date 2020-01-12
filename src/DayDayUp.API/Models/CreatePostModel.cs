
namespace DayDayUp.API.Models
{
    public class CreatePostModel
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public string[] Tags { get; set; }
        public string Content { get; set; }
        public bool IsDraft { get; set; }
        public bool IsPrivate { get; set; }
    }
}