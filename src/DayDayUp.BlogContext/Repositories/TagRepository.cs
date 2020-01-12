using DayDayUp.BlogContext.Entities.AggregateRoot;

namespace DayDayUp.BlogContext.Repositories
{
    public class TagRepository : RepositoryBase<Tag>, ITagRepository
    {
        public TagRepository(BlogDbContext dbContext) : base(dbContext)
        {
        }
    }
}