using NorthwindData;
using NorthwindData.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace ExampleRestAPI.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IMapper                _mapper;
        private readonly INorthwindDataService  _dataService;

        public CustomersController(INorthwindDataService dataService, IMapper mapper)
        {
            _mapper         = mapper;
            _dataService    = dataService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer([MaxLength(5)] string id)
        {
            var customer = await _dataService.GetCustomerAsync(id);
            return customer is null 
                ? NotFound() 
                : Ok(customer);
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
            => Ok(await _dataService.GetCustomersAsync());

        [HttpPost]
        public async Task<IActionResult> AddCustomer(Customer customer)
        {
            var succeeded = await _dataService.AddCustomerAsync(customer);
            return succeeded
                ? Created($"api/Customers/{customer.CustomerID}", customer)
                : BadRequest(ErrorMessageStrings.RecordAddError);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomer(Customer customer)
        {
            var data        = _mapper.Map<Customer>(customer);
            var succeeded   = await _dataService.UpdateCustomerAsync(data);

            return succeeded
                ? Ok()
                : BadRequest(ErrorMessageStrings.RecordUpdateError);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer([MaxLength(5)] string id)
        {
            var succeeded = await _dataService.DeleteCustomerAsync(id);
            return succeeded
                ? Ok()
                : BadRequest(ErrorMessageStrings.RecordDeleteError);
        }
    }
}
