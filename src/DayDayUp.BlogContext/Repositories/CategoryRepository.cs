using DayDayUp.BlogContext.Entities.AggregateRoot;

namespace DayDayUp.BlogContext.Repositories
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(BlogDbContext dbContext) : base(dbContext)
        {
        }
    }
}