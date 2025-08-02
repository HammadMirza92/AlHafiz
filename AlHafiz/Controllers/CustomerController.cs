using AlHafiz.DTOs;
using AlHafiz.Models;
using AlHafiz.Services.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace AlHafiz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            var customers = await _customerRepository.GetAllCustomers();
            var customersDto = customers.Select(c => new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                PhoneNumber = c.PhoneNumber

            });

            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            var customerDto = new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Description = customer.Description,
                PhoneNumber = customer.PhoneNumber
            };

            return Ok(customerDto);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto createCustomerDto)
        {
            var customer = new Customer
            {
                Name = createCustomerDto.Name,
                Description = createCustomerDto.Description,
                PhoneNumber = createCustomerDto.PhoneNumber,
                CreatedAt = DateTime.Now
            };

            var createdCustomer = await _customerRepository.AddAsync(customer);

            var customerDto = new CustomerDto
            {
                Id = createdCustomer.Id,
                Name = createdCustomer.Name,
                Description = createCustomerDto.Description,
                PhoneNumber = createCustomerDto.PhoneNumber
            };

            return CreatedAtAction(nameof(GetCustomer), new { id = customerDto.Id }, customerDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, UpdateCustomerDto updateCustomerDto)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            customer.Name = updateCustomerDto.Name;
            customer.Description = updateCustomerDto.Description;
            customer.PhoneNumber = updateCustomerDto.PhoneNumber;
            customer.UpdatedAt = DateTime.Now;

            await _customerRepository.UpdateAsync(customer);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var result = await _customerRepository.DeleteAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> SearchCustomers([FromQuery] string searchTerm)
        {
            var customers = await _customerRepository.SearchCustomersAsync(searchTerm);

            var customersDto = customers.Select(c => new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                PhoneNumber = c.PhoneNumber
            });

            return Ok(customersDto);
        }
    }
}
