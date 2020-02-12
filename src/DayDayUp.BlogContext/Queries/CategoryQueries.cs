using System.Linq;
using System.Threading.Tasks;
using DayDayUp.BlogContext.Extensions;
using DayDayUp.BlogContext.Models;
using DayDayUp.BlogContext.ValueObject;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DayDayUp.BlogContext.Queries
{
    public class CategoryQueries : ICategoryQueries
    {
        public CategoryQueries(Repositories.BlogDbContext dbContext,IOptions<Secrets> options)
        {
            _dbContext = dbContext;
            _secrets = options.Value;
        }

        private readonly Repositories.BlogDbContext _dbContext;
        private readonly Secrets _secrets;

        public async Task<PagingQuery<CategoryQueryDto>> GetPagingCategoriesAsync
            (string keywords = "", int page = 1, int size = 10)
        {
            page = PagingUtil.QueryPageValidator(page);
            size = PagingUtil.QuerySizeValidator(size);
            var skip = (page - 1) * size;

            var query = _dbContext.Categories.AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(c =>
                    EF.Functions.Like(c.Name, $"%{keywords}%") || EF.Functions.Like(c.Slug, $"%{keywords}%"));
            }

            var total = await query.CountAsync();
            var categories = await query
                .Skip(skip)
                .Take(size)
                .Select(c => new CategoryQueryDto()
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Slug = c.Slug,
                    Count = _dbContext.Posts.Count(p => p.CategoryId == c.Id)
                }).OrderByDescending(c => c.Count)
                .ToListAsync();

            var queryResult = new PagingQuery<CategoryQueryDto>
            {
                Total = total,
                Page = page,
                Limit = size,
                Values = categories
            };

            return queryResult;
        }

        public async Task<CategoryQueryDto> GetCategoryAsync(string slug)
        {
            return await _dbContext.Categories
                .Where(c => c.Slug == slug)
                .Select(c => new CategoryQueryDto()
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Slug = c.Slug,
                    Count = _dbContext.Posts.Count(p => p.CategoryId == c.Id)
                }).FirstOrDefaultAsync();
        }
    }
}