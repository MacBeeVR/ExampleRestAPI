using NorthwindData;
using NorthwindData.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ExampleRestAPI.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class CustomerDemographicsController : ControllerBase
    {
        private readonly INorthwindDataService _dataService;
        public CustomerDemographicsController(INorthwindDataService dataService)
            => _dataService = dataService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerDemographic([MaxLength(10)] string id)
        {
            var demographic = await _dataService.GetCustomerDemographicsAsync(id);
            return demographic is null
                ? NotFound()
                : Ok(demographic);
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerDemographics() 
            => Ok(await _dataService.GetCustomerDemographicsAsync());

        [HttpPost]
        public async Task<IActionResult> AddCustomerDemographic(CustomerDemographics demographic)
        {
            var succeeded = await _dataService.AddCustomerDemographicsAsync(demographic);
            return succeeded
                ? Created($"api/CustomerDemographics/{demographic.CustomerTypeID}", demographic)
                : BadRequest(ErrorMessageStrings.RecordAddError);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomerDemographic(CustomerDemographics demographic)
        {
            var succeeded = await _dataService.UpdateCustomerDemographicsAsync(demographic);
            return succeeded
                ? Ok()
                : BadRequest(ErrorMessageStrings.RecordUpdateError);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerDemographic([MaxLength(10)] string id)
        {
            var succeeded = await _dataService.DeleteCustomerDemographicsAsync(id);
            return succeeded
                ? Ok()
                : BadRequest(ErrorMessageStrings.RecordDeleteError);
        }
    }
}
