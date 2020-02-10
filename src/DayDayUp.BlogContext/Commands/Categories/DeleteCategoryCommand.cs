using DayDayUp.BlogContext.ValueObject;
using MediatR;

namespace DayDayUp.BlogContext.Commands.Categories
{
    public class DeleteCategoryCommand : IRequest<OperationResult>
    {
        public DeleteCategoryCommand(long id)
        {
            Id = id;
        }

        public long Id { get; private set; }
    }
}