using System.Threading.Tasks;
using DayDayUp.BlogContext.Models;

namespace DayDayUp.BlogContext.Queries
{
    public interface IPostQueries
    {
        Task<PagingQuery<PostDto>> GetPagingQueryListAsync
        (
            int page,
            int limit,
            string timestamp);

        Task<PagingQuery<PostDto>> GetPagingQueryListInCategoryAsync
        (
            string categorySlug,
            int page,
            int limit,
            string timestamp);

        Task<PagingQuery<PostDto>> GetPagingQueryListInTagAsync
        (
            string tagSlug,
            int page,
            int limit,
            string timestamp);

        Task<PostDetailDto> GetPostDetailBySlugAsync(string slug);
        Task<PublishedPostDto> GetPublishedPostAsync(string slug);
    }
}