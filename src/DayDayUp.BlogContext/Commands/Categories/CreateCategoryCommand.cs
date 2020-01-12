using DayDayUp.BlogContext.ValueObject;
using MediatR;

namespace DayDayUp.BlogContext.Commands.Categories
{
    public class CreateCategoryCommand : IRequest<OperationResult>
    {
        public CreateCategoryCommand(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}