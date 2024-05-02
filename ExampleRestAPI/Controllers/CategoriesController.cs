using NorthwindData;
using NorthwindData.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExampleRestAPI.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly INorthwindDataService _dataService;

        public CategoriesController(INorthwindDataService dataService)
            => _dataService = dataService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _dataService.GetCategoryAsync(id);
            if (category is null)
                return NotFound();

            return Ok(category);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
            => Ok(await _dataService.GetCategoriesAsync());

        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            await _dataService.AddCategoryAsync(category);
            return Created($"api/Categories/{category.CategoryID}", category);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            var result = await _dataService.UpdateCategoryAsync(category);
            return result ? Ok() : BadRequest("An Issue Occurred Updating the Record");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _dataService.DeleteCategoryAsync(id);
            return result ? Ok() : BadRequest("An Issue Occurred Deleting the Record");
        }
    }
}
