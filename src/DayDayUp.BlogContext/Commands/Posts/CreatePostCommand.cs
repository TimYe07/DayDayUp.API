using System;
using DayDayUp.BlogContext.ValueObject;
using MediatR;

namespace DayDayUp.BlogContext.Commands.Posts
{
    public class CreatePostCommand : IRequest<OperationResult>
    {
        public CreatePostCommand
        (
            string title,
            string slug,
            string category,
            string[] tags,
            string content,
            DateTime? createOn,
            DateTime? updateOn)
        {
            Title = title;
            Tags = tags;
            Category = category;
            Content = content;
            CreateOn = createOn;
            UpdateOn = updateOn;
        }

        public string Title { get; private set; }
        public string Slug { get; private set; }
        public string Category { get; private set; }
        public string[] Tags { get; private set; }
        public string Content { get; private set; }
        public DateTime? CreateOn { get; private set; }
        public DateTime? UpdateOn { get; private set; }
    }
}