using System.Collections.Generic;
using System.Threading.Tasks;
using DayDayUp.BlogContext.Entities.AggregateRoot;
using DayDayUp.BlogContext.Models;

namespace DayDayUp.BlogContext.Queries
{
    public interface ICategoryQueries
    {
        Task<PagingQuery<CategoryQueryDto>> GetAllCategoryAsync();
        Task<CategoryQueryDto> GetCategoryAsync(string slug);
    }
}