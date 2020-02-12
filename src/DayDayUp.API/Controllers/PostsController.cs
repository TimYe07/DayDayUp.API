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
        public async Task<IActionResult> CreateAsync(PostModel model)
        {
            var command = new CreatePostCommand(
                model.Title, 
                model.Slug, 
                model.Category, 
                model.Tags, 
                model.Content,
                model.CreateOn, 
                model.UpdateOn);
            var result = await _mediator.Send(command);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(long id, PostModel model)
        {
            var command = new UpdatePostCommand(id, model.Title, model.Slug, model.Category, model.Tags, model.Content,
                model.CreateOn, model.UpdateOn);
            var result = await _mediator.Send(command);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result.Message);
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