using System.Linq;
using System.Text.Json;
using DayDayUp.BlogContext.Entities.AggregateRoot;
using DayDayUp.BlogContext.Models;
using DayDayUp.BlogContext.ValueObject;
using Mapster;

namespace DayDayUp.BlogContext.Mapings
{
    public static class MapsterConfig
    {
        public static void Init()
        {
            TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);
            TypeAdapterConfig<Post, PostDto>
                .NewConfig()
                .Map(d => d.Tags, s => s.PostTags.Select(pt => pt.Tag));

            TypeAdapterConfig<Post, PostDetailDto>
                .NewConfig()
                .Map(d => d.Content, s => s.ConvertedContent)
                .Map(d => d.Tags, s => s.PostTags.Select(pt => pt.Tag))
                .Map(d => d.Toc, s => ConvertToObject(s.Toc));

            TypeAdapterConfig<Tag, TagQueryDto>
                .NewConfig();

            TypeAdapterConfig<Tag, PostInTagDto>
                .NewConfig();

            TypeAdapterConfig<Category, CategoryQueryDto>
                .NewConfig();

            TypeAdapterConfig<Category, PostInCategoryDto>
                .NewConfig();
        }

        private static TocItem[] ConvertToObject(string json)
        {
            return JsonSerializer.Deserialize<TocItem[]>(json);
        }
    }
}