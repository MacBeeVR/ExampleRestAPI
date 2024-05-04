using NorthwindData;
using NorthwindData.Models;
using Microsoft.AspNetCore.Mvc;
using ExampleRestAPI.DTOs;
using AutoMapper;

namespace ExampleRestAPI.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMapper                _mapper;
        private readonly INorthwindDataService  _dataService;

        public CategoriesController(INorthwindDataService dataService, IMapper mapper)
        {
            _mapper         = mapper;
            _dataService    = dataService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _dataService.GetCategoryAsync(id);
            return category is null 
                ? NotFound() 
                : Ok(category);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
            => Ok(await _dataService.GetCategoriesAsync());

        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryDTO category)
        {
            if (ModelState.IsValid)
            {
                var data = _mapper.Map<Category>(category);
                await _dataService.AddCategoryAsync(data);
                return Created($"api/Categories/{data.CategoryID}", category);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryDTO category)
        {
            if (ModelState.IsValid)
            {
                var data = _mapper.Map<Category>(category);
                data.CategoryID = id;
                var result = await _dataService.UpdateCategoryAsync(data);
                return result
                    ? Ok()
                    : BadRequest("An Issue Occurred Updating the Record");
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _dataService.DeleteCategoryAsync(id);
            return result 
                ? Ok() 
                : BadRequest("An Issue Occurred Deleting the Record");
        }
    }
}
