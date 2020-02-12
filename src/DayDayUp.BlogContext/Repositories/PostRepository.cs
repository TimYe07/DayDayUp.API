using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DayDayUp.BlogContext.Entities.AggregateRoot;
using Microsoft.EntityFrameworkCore;

namespace DayDayUp.BlogContext.Repositories
{
    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        public PostRepository(BlogContext.Repositories.BlogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Post> FindAsync(Expression<Func<Post, bool>> predicate)
        {
            return await DbContext.Set<Post>()
                .Include(p => p.Category)
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(predicate);
        }
    }
}