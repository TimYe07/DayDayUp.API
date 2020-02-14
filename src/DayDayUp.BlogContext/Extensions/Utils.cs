using System.Collections.Generic;
using System.Linq;

namespace DayDayUp.BlogContext.Extensions
{
    public static class Utils
    {
        public static string GetPostExcerpt(string rawContent, int wordCount)
        {
            var plainText = RemoveTags(rawContent);

            var result = plainText.Ellipsize(wordCount);
            return result;
        }

        public static string RemoveTags(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return string.Empty;
            }

            var result = new char[html.Length];

            var cursor = 0;
            var inside = false;
            foreach (var current in html)
            {
                switch (current)
                {
                    case '<':
                        inside = true;
                        continue;
                    case '>':
                        inside = false;
                        continue;
                }

                if (!inside)
                {
                    result[cursor++] = current;
                }
            }

            var stringResult = new string(result, 0, cursor);

            return stringResult.Replace("&nbsp;", " ");
        }

        public static string Ellipsize(this string text, int characterCount)
        {
            return text.Ellipsize(characterCount, "\u00A0\u2026");
        }

        public static string Ellipsize
        (
            this string text,
            int characterCount,
            string ellipsis,
            bool wordBoundary = false)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            if (characterCount < 0 || text.Length <= characterCount)
                return text;

            // search beginning of word
            var backup = characterCount;
            while (characterCount > 0 && text[characterCount - 1].IsLetter())
            {
                characterCount--;
            }

            // search previous word
            while (characterCount > 0 && text[characterCount - 1].IsSpace())
            {
                characterCount--;
            }

            // if it was the last word, recover it, unless boundary is requested
            if (characterCount == 0 && !wordBoundary)
            {
                characterCount = backup;
            }

            var trimmed = text.Substring(0, characterCount);
            return trimmed + ellipsis;
        }

        public static bool IsLetter(this char c)
        {
            return 'A' <= c && c <= 'Z' || 'a' <= c && c <= 'z';
        }

        public static bool IsSpace(this char c)
        {
            return c == '\r' || c == '\n' || c == '\t' || c == '\f' || c == ' ';
        }

        public static string Left(string sSource, int iLength)
        {
            return sSource.Substring(0, iLength > sSource.Length ? sSource.Length : iLength);
        }

        public static string Right(string sSource, int iLength)
        {
            return sSource.Substring(iLength > sSource.Length ? 0 : sSource.Length - iLength);
        }

        public static (List<string> addTags, List<string> removeTags) TagChanges
            (IEnumerable<string> tags, IEnumerable<string> sourceTags)
        {
            var addTags = new List<string>();
            var removeTags = new List<string>();

            if (!tags.Any() && !sourceTags.Any())
            {
                return (addTags, removeTags);
            }

            if (tags.Any() && !sourceTags.Any())
            {
                return (tags.ToList(), removeTags);
            }

            if (!tags.Any() && sourceTags.Any())
            {
                return (addTags, sourceTags.ToList());
            }

            foreach (var sourceTag in sourceTags)
            {
                if (tags.Any(t => t != sourceTag))
                {
                    removeTags.Add(sourceTag);
                }
            }

            foreach (var tag in tags)
            {
                if (sourceTags.Any(t => t != tag))
                {
                    addTags.Add(tag);
                }
            }

            return (addTags, removeTags);
        }
    }
}