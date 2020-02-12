using System.Linq;
using System.Threading.Tasks;
using DayDayUp.BlogContext.Extensions;
using DayDayUp.BlogContext.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DayDayUp.BlogContext.Queries
{
    public class PostQueries : IPostQueries
    {
        public PostQueries(Repositories.BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly Repositories.BlogDbContext _dbContext;

        public async Task<PagingQuery<PostDto>> GetPagingQueryListAsync(int page, int limit, string timestamp)
        {
            var postsQuery = _dbContext.Posts
                .Include(p => p.Category)
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag);
            var total = await postsQuery.CountAsync();
            var queryResult = new PagingQuery<PostDto>
            {
                Page = page,
                Limit = limit,
                Total = total,
                Values = await postsQuery.OrderByDescending(q => q.Id)
                    .ProjectToType<PostDto>()
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToListAsync()
            };

            return queryResult;
        }

        public async Task<PagingQuery<PostDto>> GetPagingQueryListInCategoryAsync
        (
            string categorySlug,
            int page,
            int limit,
            string timestamp)
        {
            var postsQuery = _dbContext.Posts.Include(p => p.Category)
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .Where(p => p.Category.Slug == categorySlug && !p.IsDraft && !p.IsPrivate && !p.IsDeleted);

            var total = await postsQuery.CountAsync();
            var queryResult = new PagingQuery<PostDto>
            {
                Page = page,
                Limit = limit,
                Total = total,
                Values = await postsQuery.OrderByDescending(q => q.Id)
                    .ProjectToType<PostDto>()
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToListAsync()
            };

            return queryResult;
        }

        public async Task<PagingQuery<PostDto>> GetPagingQueryListInTagAsync
        (
            string tagSlug,
            int page,
            int limit,
            string timestamp)
        {
            var postsQuery = _dbContext.Posts.Include(p => p.Category)
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .Where(p => p.PostTags.Any(pt => pt.Tag.Slug == tagSlug) && !p.IsDraft && !p.IsPrivate &&
                            !p.IsDeleted);

            var total = await postsQuery.CountAsync();
            var queryResult = new PagingQuery<PostDto>
            {
                Page = page,
                Limit = limit,
                Total = total,
                Values = await postsQuery.OrderByDescending(q => q.Id)
                    .ProjectToType<PostDto>()
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToListAsync()
            };

            return queryResult;
        }

        public async Task<PostDetailDto> GetPostDetailBySlugAsync(string slug)
        {
            var queryResult = await _dbContext.Posts.Include(p => p.Category)
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .Where(p => p.Slug == slug && !p.IsDraft && !p.IsPrivate && !p.IsDeleted)
                .ProjectToType<PostDetailDto>()
                .FirstOrDefaultAsync();

            return queryResult;
        }

        public async Task<PublishedPostDto> GetPublishedPostAsync(string slug)
        {
            var queryResult = await _dbContext.Posts.Include(p => p.Category)
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .Where(p => p.Slug == slug && !p.IsDraft && !p.IsPrivate && !p.IsDeleted)
                .ProjectToType<PublishedPostDto>()
                .FirstOrDefaultAsync();

            return queryResult;
        }
    }
}