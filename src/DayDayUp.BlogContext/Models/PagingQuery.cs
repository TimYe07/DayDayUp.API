using System.Collections.Generic;

namespace DayDayUp.BlogContext.Models
{
    public class PagingQuery<T>
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public IEnumerable<T> Values { get; set; }
    }
}