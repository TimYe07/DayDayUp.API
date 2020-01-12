// using System;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using DayDayUp.BlogContext.Entities.AggregateRoot;
// using DayDayUp.BlogContext.Extensions;
// using DayDayUp.BlogContext.Infrastructure.Repositories;
// using DayDayUp.Plugins;
// using MediatR;
// using Microsoft.Extensions.Logging;
// using MongoDB.Bson;
// using Newtonsoft.Json;
//
// namespace DayDayUp.BlogContext.Commands
// {
//     public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, bool>
//     {
//         public UpdatePostCommandHandler
//         (
//             IPostRepository postRepository,
//             ITextConversionService textConversionService,
//             ILogger<UpdatePostCommandHandler> logger)
//         {
//             _postRepository = postRepository;
//             _textConversion = textConversionService;
//             _logger = logger;
//         }
//
//         private readonly IPostRepository _postRepository;
//         private readonly ITextConversionService _textConversion;
//         private readonly ILogger<UpdatePostCommandHandler> _logger;
//
//         public async Task<bool> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
//         {
//             var jsonDoc = JsonConvert.SerializeObject(request.UpdatePostDictionary);
//             var updateDocument = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(jsonDoc);
//
//             var bsDocument = new BsonDocument();
//             foreach (var item in updateDocument.Names)
//             {
//                 var isExit = ObjectExtension.TryGetOwnPropertyName(typeof(Post), item, out var propertyName);
//                 if (isExit)
//                 {
//                     bsDocument.Add(propertyName, updateDocument[item]);
//                 }
//             }
//
//             if (bsDocument.Names.Any(x => x.Equals("Content", StringComparison.CurrentCultureIgnoreCase)))
//             {
//                 var textDocument = await _textConversion.ToMarkdownAsync(bsDocument["Content"].AsString);
//                 bsDocument.Add("ConvertedContent", textDocument.Doc.Html);
//                 bsDocument.Add("Toc", new BsonArray(textDocument.Toc));
//             }
//
//             bsDocument.Add("UpdateOn", new BsonDateTime(DateTime.Now));
//
//             try
//             {
//                 await _postRepository.UpdateAsync(request.Id, bsDocument);
//                 return true;
//             }
//             catch (Exception e)
//             {
//                 _logger.LogError(e.Message);
//                 return false;
//             }
//         }
//     }
// }