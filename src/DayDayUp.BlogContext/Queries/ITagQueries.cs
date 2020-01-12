using System.Collections.Generic;
using System.Threading.Tasks;
using DayDayUp.BlogContext.Models;

namespace DayDayUp.BlogContext.Queries
{
    public interface ITagQueries
    {
        Task<PagingQuery<TagQueryDto>> GetAllTagAsync();
        Task<TagQueryDto> GetTagAsync(string slug);
    }
}