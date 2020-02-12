using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DayDayUp.BlogContext.SeedWork;
using DayDayUp.BlogContext.ValueObject;

namespace DayDayUp.BlogContext.Entities.AggregateRoot
{
    public class Post : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// 分类
        /// </summary>
        public long CategoryId { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 原文内容
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string ConvertedContent { get; private set; }

        /// <summary>
        /// 文档类型
        /// </summary>
        public ContentType ContentType { get; private set; } = ContentType.Markdown;

        /// <summary>
        /// 地址名
        /// </summary>
        public string Slug { get; private set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        public int ViewCount { get; private set; } = 0;

        /// <summary>
        /// 是否草稿
        /// </summary>
        public bool IsDraft { get; private set; } = false;

        /// <summary>
        /// 是否设置为私有
        /// </summary>
        public bool IsPrivate { get; private set; } = false;

        /// <summary>
        /// 创建于
        /// </summary>
        public DateTime CreateOn { get; private set; } = DateTime.Now;

        /// <summary>
        /// 更新于
        /// </summary>
        public DateTime? UpdateOn { get; private set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; private set; } = false;

        public Category Category { get; set; }
        public List<PostTag> PostTags { get; set; } = new List<PostTag>();

        public void SetOrUpdateSlug(string slug)
        {
            Slug = !string.IsNullOrEmpty(slug) ? slug : Id.ToString();
        }

        public void SetOrUpdateDesc(string desc)
        {
            Description = desc;
        }

        public void SetOrUpdateTitle(string title)
        {
            if (!string.IsNullOrEmpty(title))
                Title = title;
        }

        public void SetOrUpdateCategory(Category category)
        {
            if (Category != category)
            {
                Category = category;
            }
        }

        public void SetOrUpdateTags(IEnumerable<Tag> tags)
        {
            if (!tags.Any())
            {
                PostTags = null;
                return;
            }

            foreach (var tag in tags)
            {
                if (PostTags.All(t => t.TagId != tag.Id))
                {
                    PostTags.Add(new PostTag
                    {
                        PostId = Id,
                        TagId = tag.Id
                    });
                }
            }


            var origalTagIds = PostTags.Select(pt => pt.TagId);
            if (origalTagIds.Any())
            {
                foreach (var origalTagId in origalTagIds)
                {
                    if (!tags.All(t => t.Id != origalTagId)) continue;
                    var pt = PostTags.Find(x => x.TagId == origalTagId);
                    PostTags.Remove(pt);
                }
            }
        }

        public void SetOrUpdateContent(TextDocument textDocument)
        {
            ContentType = ContentType.Markdown;
            Content = textDocument.Doc.Source;
            ConvertedContent = textDocument.Doc.Html;
        }

        public void SetOrUpdateCreateOn(DateTime? createOn)
        {
            CreateOn = createOn ?? DateTime.Now;
        }

        public void SetOrUpdateUpdateOn(DateTime? updateOn)
        {
            UpdateOn = updateOn;
        }

        public void Deleted()
        {
            IsDeleted = true;
        }
    }
}