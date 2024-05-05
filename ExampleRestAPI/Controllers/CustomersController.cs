using NorthwindData;
using NorthwindData.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

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
        public async Task<IActionResult> GetCustomer(string id)
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
            var data = _mapper.Map<Customer>(customer);
            await _dataService.AddCustomerAsync(data);
            return Created($"api/Customers/{data.CustomerID}", customer);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomer(Customer customer)
        {
            var data    = _mapper.Map<Customer>(customer);
            var result  = await _dataService.UpdateCustomerAsync(data);
            return result
                ? Ok()
                : BadRequest("An Issue Occurred Updating the Record");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            var result = await _dataService.DeleteCustomerAsync(id);
            return result
                ? Ok()
                : BadRequest("An Issue Occurred Deleting the Record");
        }
    }
}
