using System.Collections.Generic;
using System.Threading.Tasks;
using DayDayUp.BlogContext.Models;

namespace DayDayUp.BlogContext.Queries
{
    public interface ITagQueries
    {
        Task<PagingQuery<TagQueryDto>> GetPagingCategoriesAsync(string keywords = "", int page = 1, int size = 10);
        Task<TagQueryDto> GetTagAsync(string slug);
    }
}