using System.Collections.Generic;
using System.Threading.Tasks;
using DayDayUp.BlogContext.Entities.AggregateRoot;
using DayDayUp.BlogContext.Models;

namespace DayDayUp.BlogContext.Queries
{
    public interface ICategoryQueries
    {
        Task<PagingQuery<CategoryQueryDto>> GetPagingCategoriesAsync(string keywords = "", int page = 1, int size = 10);
        Task<CategoryQueryDto> GetCategoryAsync(string slug);
    }
}