using DayDayUp.BlogContext.ValueObject;
using MediatR;

namespace DayDayUp.BlogContext.Commands.Posts
{
    public class DeletePostCommand : IRequest<OperationResult>
    {
        public DeletePostCommand(long id)
        {
            Id = id;
        }

        public long Id { get; private set; }
    }
}