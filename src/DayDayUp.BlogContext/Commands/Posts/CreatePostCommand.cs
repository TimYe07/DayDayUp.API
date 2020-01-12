using DayDayUp.BlogContext.ValueObject;
using MediatR;

namespace DayDayUp.BlogContext.Commands.Posts
{
    public class CreatePostCommand : IRequest<OperationResult>
    {
        public CreatePostCommand
        (
            string title,
            string category,
            string[] tags,
            string content,
            bool isDraft,
            bool isPrivate)
        {
            Title = title;
            Tags = tags;
            Category = category;
            Content = content;
            IsDraft = isDraft;
            IsPrivate = isPrivate;
        }

        public string Title { get; private set; }
        public string Category { get; private set; }
        public string[] Tags { get; private set; }
        public string Content { get; private set; }
        public bool IsDraft { get; private set; }

        public bool IsPrivate { get;  private set; }
    }
}