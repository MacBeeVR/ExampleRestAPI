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
        public async Task<IActionResult> AddCategory(CategoryInsertDTO category)
        {
            var data        = _mapper.Map<Category>(category);
            var succeeded   = await _dataService.AddCategoryAsync(data);
            return succeeded
                ? Created($"api/Categories/{data.CategoryID}", category)
                : BadRequest(ErrorMessageStrings.RecordAddError);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            var succeeded = await _dataService.UpdateCategoryAsync(category);
            return succeeded
                ? Ok()
                : BadRequest(ErrorMessageStrings.RecordUpdateError);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var succeeded = await _dataService.DeleteCategoryAsync(id);
            return succeeded 
                ? Ok() 
                : BadRequest(ErrorMessageStrings.RecordDeleteError);
        }
    }
}
