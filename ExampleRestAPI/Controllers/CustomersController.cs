using NorthwindData;
using NorthwindData.Models;
using Microsoft.AspNetCore.Mvc;
using ExampleRestAPI.DTOs;
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
        public async Task<IActionResult> AddCustomer(CustomerDTO customer)
        {
            if (ModelState.IsValid)
            {
                var data = _mapper.Map<Customer>(customer);
                await _dataService.AddCustomerAsync(data);
                return Created($"api/Customers/{data.CustomerID}", customer);
            }

            return BadRequest(ModelState);
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(string id, CustomerDTO customer)
        {
            if (id != customer.CustomerId)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var data    = _mapper.Map<Customer>(customer);
                var result  = await _dataService.UpdateCustomerAsync(data);
                return result
                    ? Ok()
                    : BadRequest("An Issue Occurred Updating the Record");
            }

            return BadRequest(ModelState);
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
