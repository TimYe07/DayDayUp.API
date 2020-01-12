using System.Collections.Generic;
using System.Threading.Tasks;
using DayDayUp.BlogContext.Entities.AggregateRoot;

namespace DayDayUp.BlogContext.Services
{
    public interface IPostDomainService
    {
        Task<Category> GetOrCreateCategoryAsync(string name);

        Task<IEnumerable<Tag>> GetOrCreateTagAsync(string[] tags);
    }
}