using System.Linq;
using System.Threading.Tasks;
using DayDayUp.BlogContext.Extensions;
using DayDayUp.BlogContext.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DayDayUp.BlogContext.Queries
{
    public class TagQueries : ITagQueries
    {
        public TagQueries(Repositories.BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly Repositories.BlogDbContext _dbContext;

        public async Task<PagingQuery<TagQueryDto>> GetPagingCategoriesAsync
            (string keywords = "", int page = 1, int size = 10)
        {
            page = PagingUtil.QueryPageValidator(page);
            size = PagingUtil.QuerySizeValidator(size);
            var skip = (page - 1) * size;

            var query = _dbContext.Tags.AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(c =>
                    EF.Functions.Like(c.Name, $"%{keywords}%") || EF.Functions.Like(c.Slug, $"%{keywords}%"));
            }

            var total = await query.CountAsync();
            var tags = await query
                .Skip(skip)
                .Take(size)
                .Select(t => new TagQueryDto()
                {
                    Name = t.Name,
                    Slug = t.Slug,
                    Count = _dbContext.PostTags.Count(pt => pt.TagId == t.Id)
                })
                .OrderByDescending(c => c.Count)
                .ToListAsync();

            var queryResult = new PagingQuery<TagQueryDto>
            {
                Total = total,
                Page = page,
                Limit = size,
                Values = tags
            };

            return queryResult;
        }

        public async Task<TagQueryDto> GetTagAsync(string slug)
        {
            return await _dbContext.Tags
                .AsQueryable()
                .Where(t => t.Slug == slug)
                .Select(t => new TagQueryDto()
                {
                    Name = t.Name,
                    Slug = t.Slug,
                    Count = _dbContext.PostTags.Count(pt => pt.TagId == t.Id)
                })
                .FirstOrDefaultAsync();
        }
    }
}