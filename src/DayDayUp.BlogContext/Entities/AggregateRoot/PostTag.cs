namespace DayDayUp.BlogContext.Entities.AggregateRoot
{
    public class PostTag
    {
        public long TagId { get; set; }
        public Tag Tag { get; set; }
        public long PostId { get; set; }
        public Post Post { get; set; }
    }
}