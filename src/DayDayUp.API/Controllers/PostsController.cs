using System.Collections.Generic;
using System.Threading.Tasks;
using DayDayUp.API.Models;
using DayDayUp.BlogContext.Commands;
using DayDayUp.BlogContext.Commands.Posts;
using DayDayUp.BlogContext.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DayDayUp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        public PostsController(IMediator mediator, IPostQueries postQueries)
        {
            _mediator = mediator;
            _postQueries = postQueries;
        }

        private readonly IMediator _mediator;
        private readonly IPostQueries _postQueries;

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreatePostModel model)
        {
            var command = new CreatePostCommand(model.Title, model.Category, model.Tags, model.Content,
                model.IsDraft, model.IsPrivate);
            var result = await _mediator.Send(command);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, Dictionary<string, object> updatePostDictionary)
        {
            var command = new UpdatePostCommand(id, updatePostDictionary);
            var result = await _mediator.Send(command);
            if (result)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetDetailAsync(string slug)
        {
            var result = await _postQueries.GetPostDetailBySlugAsync(slug);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync
        (
            string categorySlug,
            string tagSlug,
            int page = 1,
            int limit = 10,
            string timestamp = "0")
        {
            if (!string.IsNullOrEmpty(categorySlug))
            {
                var categoryQueryResult =
                    await _postQueries.GetPagingQueryListInCategoryAsync(categorySlug, page, limit, timestamp);
                return Ok(categoryQueryResult);
            }

            if (!string.IsNullOrEmpty(tagSlug))
            {
                var tagQueryResult = await _postQueries.GetPagingQueryListInTagAsync(tagSlug, page, limit, timestamp);
                return Ok(tagQueryResult);
            }

            var result = await _postQueries.GetPagingQueryListAsync(page, limit, timestamp);
            return Ok(result);
        }
    }
}