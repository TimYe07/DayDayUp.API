using DayDayUp.BlogContext.ValueObject;
using MediatR;

namespace DayDayUp.BlogContext.Commands.Tags
{
    public class CreateTagCommand : IRequest<OperationResult>
    {
        public CreateTagCommand(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}