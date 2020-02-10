using DayDayUp.BlogContext.ValueObject;
using MediatR;

namespace DayDayUp.BlogContext.Commands.Tags
{
    public class DeleteTagCommand : IRequest<OperationResult>
    {
        public DeleteTagCommand(long id)
        {
            Id = id;
        }

        public long Id { get; private set; }
    }
}