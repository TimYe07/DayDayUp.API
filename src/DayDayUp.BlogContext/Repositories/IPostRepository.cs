using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DayDayUp.BlogContext.Entities.AggregateRoot;
using DayDayUp.BlogContext.SeedWork;

namespace DayDayUp.BlogContext.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<Post> FindAsync(Expression<Func<Post, bool>> predicate);
    }
}