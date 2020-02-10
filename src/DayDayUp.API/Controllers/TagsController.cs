using System.Threading.Tasks;
using DayDayUp.API.Models;
using DayDayUp.BlogContext.Commands.Tags;
using DayDayUp.BlogContext.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DayDayUp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        public TagsController(ITagQueries tagQueries, IMediator mediator)
        {
            _tagQueries = tagQueries;
            _mediator = mediator;
        }

        private readonly ITagQueries _tagQueries;
        private readonly IMediator _mediator;

        [HttpGet]
        public async Task<IActionResult> GetTagAsync(string keywords = "", int page = 1, int size = 10)
        {
            var result = await _tagQueries.GetPagingCategoriesAsync(keywords, page, size);
            return Ok(result);
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetTagAsync(string slug)
        {
            var result = await _tagQueries.GetTagAsync(slug);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTagAsync(TagCreateModel model)
        {
            var command = new CreateTagCommand(model.Name);
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTagAsync(TagUpdateModel model)
        {
            var command = new UpdateTagCommand(model.Id, model.Name, model.Slug, model.IsGenerateSlug);
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTagAsync(long id)
        {
            var command = new DeleteTagCommand(id);
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}