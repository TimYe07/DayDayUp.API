using System.Net;
using System.Text.RegularExpressions;

namespace DayDayUp.BlogContext.Services
{
    public static class MarkdownUtility
    {
        private static readonly Regex _regexInlineCode = new Regex(@"([^`])`.+?`([^`])");
        private static readonly Regex _regexCode = new Regex(@"```[^`]+```");
        private static readonly Regex _regexLink = new Regex(@"\[(.*?)\]\(.*?\)");
        private static readonly Regex _regexImage = new Regex(@"!\[(.*?)\]\(.*?\)");
        private static readonly Regex _regexMarkupChar = new Regex(@"[*#>-]");
        private static readonly Regex _regexMultiSpace = new Regex(@" {2,}");

        public static string RemoveMarkdownTags(string text)
        {
            text = WebUtility.HtmlDecode(text);
            text = _regexImage.Replace(text, " ");
            text = _regexCode.Replace(text, " ");
            text = _regexInlineCode.Replace(text, "$1 $2");
            text = _regexLink.Replace(text, " \"$1\" ");
            text = _regexMarkupChar.Replace(text, " ");
            return _regexMultiSpace.Replace(text, " ");
        }
    }
}