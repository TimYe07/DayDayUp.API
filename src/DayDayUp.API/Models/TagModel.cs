namespace DayDayUp.API.Models
{
    public class TagModel
    {
    }

    public class TagCreateModel : TagModel
    {
        public string Name { get; set; }
    }

    public class TagUpdateModel : TagModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public bool IsGenerateSlug { get; set; }
    }
}