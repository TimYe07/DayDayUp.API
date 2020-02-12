using System;
using System.Collections.Generic;
using DayDayUp.BlogContext.ValueObject;
using MediatR;

namespace DayDayUp.BlogContext.Commands.Posts
{
    public class UpdatePostCommand : IRequest<OperationResult>
    {
        public UpdatePostCommand
        (
            long id,
            string title,
            string slug,
            string category,
            string[] tags,
            string content,
            DateTime? createOn,
            DateTime? updateOn)
        {
            Id = id;
            Title = title;
            Slug = slug;
            Tags = tags;
            Category = category;
            Content = content;
            CreateOn = createOn;
            UpdateOn = updateOn;
        }

        public long Id { get; private set; }
        public string Title { get; private set; }
        public string Slug { get; private set; }
        public string Category { get; private set; }
        public string[] Tags { get; private set; }
        public string Content { get; private set; }
        public DateTime? CreateOn { get; private set; }
        public DateTime? UpdateOn { get; private set; }
    }
}