using System.Threading.Tasks;
using DayDayUp.BlogContext.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAllTagAsync()
        {
            var result = await _tagQueries.GetAllTagAsync();
            return Ok(result);
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetTagAsync(string slug)
        {
            var result = await _tagQueries.GetTagAsync(slug);
            return Ok(result);
        }
    }
}