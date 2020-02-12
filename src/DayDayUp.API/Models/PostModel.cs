using System;

namespace DayDayUp.API.Models
{
    public class PostModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// URL地址名
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// 分类名
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string[] Tags { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 创建于
        /// </summary>
        public DateTime? CreateOn { get; set; }

        /// <summary>
        /// 更新于
        /// </summary>
        public DateTime? UpdateOn { get; set; }
    }
}