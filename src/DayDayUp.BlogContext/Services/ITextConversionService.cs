using System.Threading.Tasks;
using DayDayUp.BlogContext.ValueObject;

namespace DayDayUp.BlogContext.Services
{
    public interface ITextConversionService
    {
        Task<TextDocument> ToMarkdownAsync(string content);

        Task<string> GenerateSlugAsync(string title);
        string ExtractKeywords(string text, int worldCount);
    }
}