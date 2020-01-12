namespace DayDayUp.BlogContext.ValueObject
{
    public class TextDocument
    {
        public TocItem[] Toc { get; set; }
        public Document Doc { get; set; }
        public string[] Languages { get; set; }
    }
}