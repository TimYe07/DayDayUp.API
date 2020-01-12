using DayDayUp.BlogContext.Extensions;

namespace DayDayUp.BlogContext.SeedWork
{
    public class BaseEntity<T>
    {
        public T Id { get; set; }
    }

    public class BaseEntity : BaseEntity<long>
    {
        public BaseEntity()
        {
            Id = LongId.GenerateNewId();
        }
    }
}