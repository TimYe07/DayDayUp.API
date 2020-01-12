using System.Threading.Tasks;
using DayDayUp.BlogContext.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DayDayUp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        public CategoriesController(ICategoryQueries categoryQueries, IMediator mediator)
        {
            _categoryQueries = categoryQueries;
            _mediator = mediator;
        }
        
        private readonly ICategoryQueries _categoryQueries;
        private readonly IMediator _mediator;

        [HttpGet]
        public async Task<IActionResult> GetAllCategoryAsync()
        {
            var result = await _categoryQueries.GetAllCategoryAsync();
            return Ok(result);
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetCategoryAsync(string slug)
        {
            var result = await _categoryQueries.GetCategoryAsync(slug);
            return Ok(result);
        }
    }
}

