using DayDayUp.BlogContext.Entities.AggregateRoot;

namespace DayDayUp.BlogContext.Repositories
{
    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        public PostRepository(BlogContext.Repositories.BlogDbContext dbContext) : base(dbContext)
        {
        }
    }
}