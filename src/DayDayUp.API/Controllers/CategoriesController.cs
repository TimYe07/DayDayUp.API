using System.Threading.Tasks;
using DayDayUp.API.Models;
using DayDayUp.BlogContext.Commands.Categories;
using DayDayUp.BlogContext.Queries;
using DayDayUp.BlogContext.ValueObject;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DayDayUp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        public CategoriesController(ICategoryQueries categoryQueries, IMediator mediator, IOptions<Secrets> options)
        {
            _categoryQueries = categoryQueries;
            _mediator = mediator;
            _secrets = options.Value;
        }

        private readonly ICategoryQueries _categoryQueries;
        private readonly IMediator _mediator;
        private readonly Secrets _secrets;

        [HttpGet]
        public async Task<IActionResult> GetCategoryAsync(string keywords = "", int page = 1, int size = 15)
        {
            var result = await _categoryQueries.GetPagingCategoriesAsync(keywords, page, size);
            return Ok(result);
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetCategoryAsync(string slug)
        {
            var result = await _categoryQueries.GetCategoryAsync(slug);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategoryAsync(CategoryCreateModel model)
        {
            var command = new CreateCategoryCommand(model.Name);
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategoryAsync(CategoryUpdateModel model)
        {
            var command = new UpdateCategoryCommand(model.Id, model.Name, model.Slug, model.IsGenerateSlug);
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryAsync(long id)
        {
            var command = new DeleteCategoryCommand(id);
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}