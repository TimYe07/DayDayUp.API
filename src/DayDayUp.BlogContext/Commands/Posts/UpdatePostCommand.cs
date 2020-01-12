using System.Collections.Generic;
using MediatR;

namespace DayDayUp.BlogContext.Commands.Posts
{
    public class UpdatePostCommand : IRequest<bool>
    {
        public UpdatePostCommand(string id, Dictionary<string, object> updatePostDictionary)
        {
            Id = id;
            UpdatePostDictionary = updatePostDictionary;
        }

        public string Id { get; private set; }

        public Dictionary<string, object> UpdatePostDictionary { get; private set; }
    }
}