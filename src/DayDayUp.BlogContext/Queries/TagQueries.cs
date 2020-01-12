using System.Linq;
using System.Threading.Tasks;
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

        public async Task<PagingQuery<TagQueryDto>> GetAllTagAsync()
        {
            var queryResult = new PagingQuery<TagQueryDto>
            {
                Page = 1,
                Limit = 10,
                Values = _dbContext.Tags.Select(t => new TagQueryDto()
                {
                    Name = t.Name,
                    Slug = t.Slug,
                    Count = _dbContext.PostTags.Count(pt => pt.TagId == t.Id)
                }).OrderByDescending(c => c.Count)
            };

            return await Task.FromResult(queryResult);
        }

        public async Task<TagQueryDto> GetTagAsync(string slug)
        {
            return await _dbContext.Tags
                .AsQueryable()
                .Where(t => t.Slug == slug)
                .ProjectToType<TagQueryDto>()
                .FirstOrDefaultAsync();
        }
    }
}