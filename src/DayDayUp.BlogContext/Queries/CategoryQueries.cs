using System.Linq;
using System.Threading.Tasks;
using DayDayUp.BlogContext.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DayDayUp.BlogContext.Queries
{
    public class CategoryQueries : ICategoryQueries
    {
        public CategoryQueries(Repositories.BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly Repositories.BlogDbContext _dbContext;

        public async Task<PagingQuery<CategoryQueryDto>> GetAllCategoryAsync()
        {
            var queryResult = new PagingQuery<CategoryQueryDto>
            {
                Page = 1,
                Limit = 10,
                Values = _dbContext.Categories.Select(c => new CategoryQueryDto()
                {
                    Name = c.Name,
                    Slug = c.Slug,
                    Count = _dbContext.Posts.Count(p => p.CategoryId == c.Id)
                }).OrderByDescending(c => c.Count)
            };

            return await Task.FromResult(queryResult);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _dbContext.Categories
                .AsNoTracking()
                .CountAsync();
        }

        public async Task<CategoryQueryDto> GetCategoryAsync(string slug)
        {
            return await _dbContext.Categories
                .Where(c => c.Slug == slug)
                .ProjectToType<CategoryQueryDto>()
                .FirstOrDefaultAsync();
        }
    }
}