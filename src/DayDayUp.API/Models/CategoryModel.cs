namespace DayDayUp.API.Models
{
    public class CategoryModel
    {
    }

    public class CategoryCreateModel : CategoryModel
    {
        public string Name { get; set; }
    }

    public class CategoryUpdateModel : CategoryModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public bool IsGenerateSlug { get; set; }
    }
}