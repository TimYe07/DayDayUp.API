using System.Threading.Tasks;
using DayDayUp.BlogContext.ValueObject;

namespace DayDayUp.BlogContext.Services
{
    public interface ITextConversionService
    {
        Task<TextDocument> ToMarkdownAsync(string content);

        Task<string> GenerateSlugAsync(string title);

        Task<string> GenerateSummaryAsync(string content);
    }
}