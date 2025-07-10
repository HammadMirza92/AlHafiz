using AlHafiz.DTOs;
using AlHafiz.Models;
using AlHafiz.Services.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace AlHafiz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateManagementController : ControllerBase
    {
        private readonly ICustomerItemRateRepository _rateRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IItemRepository _itemRepository;

        public RateManagementController(
            ICustomerItemRateRepository rateRepository,
            ICustomerRepository customerRepository,
            IItemRepository itemRepository)
        {
            _rateRepository = rateRepository;
            _customerRepository = customerRepository;
            _itemRepository = itemRepository;
        }

        // GET: api/RateManagement
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerItemRateDto>>> GetAllRates()
        {
            var rates = await _rateRepository.GetAllRatesAsync();
            var rateDtos = rates.Select(r => new CustomerItemRateDto
            {
                Id = r.Id,
                CustomerId = r.CustomerId,
                CustomerName = r.Customer.Name,
                ItemId = r.ItemId,
                ItemName = r.Item.Name,
                Rate = r.Rate,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            });
            return Ok(rateDtos);
        }

        // GET: api/RateManagement/matrix
        [HttpGet("matrix")]
        public async Task<ActionResult<RateMatrixDto>> GetRateMatrix()
        {
            var customers = await _customerRepository.GetAllCustomers();
            var items = await _itemRepository.GetAllAsync();
            var rates = await _rateRepository.GetAllRatesAsync();

            var rateMatrix = new RateMatrixDto
            {
                Customers = customers.Select(c => new CustomerDto { Id = c.Id, Name = c.Name }).ToList(),
                Items = items.Select(i => new ItemDto { Id = i.Id, Name = i.Name }).ToList(),
                CustomerRates = customers.Select(customer => new CustomerRateDto
                {
                    CustomerId = customer.Id,
                    CustomerName = customer.Name,
                    ItemRates = items.Select(item =>
                    {
                        var rate = rates.FirstOrDefault(r => r.CustomerId == customer.Id && r.ItemId == item.Id);
                        return new ItemRateDto
                        {
                            ItemId = item.Id,
                            ItemName = item.Name,
                            Rate = rate?.Rate,
                            HasRate = rate != null
                        };
                    }).ToList()
                }).ToList()
            };

            return Ok(rateMatrix);
        }

        // GET: api/RateManagement/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerItemRateDto>> GetRate(int id)
        {
            var rate = await _rateRepository.GetRateByIdAsync(id);
            if (rate == null)
                return NotFound();

            var rateDto = new CustomerItemRateDto
            {
                Id = rate.Id,
                CustomerId = rate.CustomerId,
                CustomerName = rate.Customer.Name,
                ItemId = rate.ItemId,
                ItemName = rate.Item.Name,
                Rate = rate.Rate,
                CreatedAt = rate.CreatedAt,
                UpdatedAt = rate.UpdatedAt
            };

            return Ok(rateDto);
        }

        // GET: api/RateManagement/customer/{customerId}/item/{itemId}
        [HttpGet("customer/{customerId}/item/{itemId}")]
        public async Task<ActionResult<CustomerItemRateDto>> GetRateByCustomerAndItem(int customerId, int itemId)
        {
            var rate = await _rateRepository.GetRateByCustomerAndItemAsync(customerId, itemId);
            if (rate == null)
                return NotFound();

            var rateDto = new CustomerItemRateDto
            {
                Id = rate.Id,
                CustomerId = rate.CustomerId,
                CustomerName = rate.Customer.Name,
                ItemId = rate.ItemId,
                ItemName = rate.Item.Name,
                Rate = rate.Rate,
                CreatedAt = rate.CreatedAt,
                UpdatedAt = rate.UpdatedAt
            };

            return Ok(rateDto);
        }

        // GET: api/RateManagement/customer/{customerId}
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<CustomerItemRateDto>>> GetRatesByCustomer(int customerId)
        {
            var rates = await _rateRepository.GetRatesByCustomerAsync(customerId);
            var rateDtos = rates.Select(r => new CustomerItemRateDto
            {
                Id = r.Id,
                CustomerId = r.CustomerId,
                CustomerName = r.Customer.Name,
                ItemId = r.ItemId,
                ItemName = r.Item.Name,
                Rate = r.Rate,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            });
            return Ok(rateDtos);
        }

        // GET: api/RateManagement/item/{itemId}
        [HttpGet("item/{itemId}")]
        public async Task<ActionResult<IEnumerable<CustomerItemRateDto>>> GetRatesByItem(int itemId)
        {
            var rates = await _rateRepository.GetRatesByItemAsync(itemId);
            var rateDtos = rates.Select(r => new CustomerItemRateDto
            {
                Id = r.Id,
                CustomerId = r.CustomerId,
                CustomerName = r.Customer.Name,
                ItemId = r.ItemId,
                ItemName = r.Item.Name,
                Rate = r.Rate,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            });
            return Ok(rateDtos);
        }

        // POST: api/RateManagement
        [HttpPost]
        public async Task<ActionResult<CustomerItemRateDto>> CreateRate(CreateCustomerItemRateDto createRateDto)
        {
            // Check if rate already exists
            var existingRate = await _rateRepository.GetRateByCustomerAndItemAsync(
                createRateDto.CustomerId, createRateDto.ItemId);

            if (existingRate != null)
                return Conflict("Rate already exists for this customer-item combination");

            // Verify customer and item exist
            var customer = await _customerRepository.GetByIdAsync(createRateDto.CustomerId);
            var item = await _itemRepository.GetByIdAsync(createRateDto.ItemId);

            if (customer == null)
                return BadRequest("Customer not found");
            if (item == null)
                return BadRequest("Item not found");

            var rate = new CustomerItemRate
            {
                CustomerId = createRateDto.CustomerId,
                ItemId = createRateDto.ItemId,
                Rate = createRateDto.Rate
            };

            var createdRate = await _rateRepository.AddRateAsync(rate);
            var rateDto = new CustomerItemRateDto
            {
                Id = createdRate.Id,
                CustomerId = createdRate.CustomerId,
                CustomerName = createdRate.Customer.Name,
                ItemId = createdRate.ItemId,
                ItemName = createdRate.Item.Name,
                Rate = createdRate.Rate,
                CreatedAt = createdRate.CreatedAt,
                UpdatedAt = createdRate.UpdatedAt
            };

            return CreatedAtAction(nameof(GetRate), new { id = rateDto.Id }, rateDto);
        }

        // PUT: api/RateManagement/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRate(int id, UpdateCustomerItemRateDto updateRateDto)
        {
            var rate = await _rateRepository.GetRateByIdAsync(id);
            if (rate == null)
                return NotFound();

            rate.Rate = updateRateDto.Rate;
            await _rateRepository.UpdateRateAsync(rate);

            return NoContent();
        }

        // POST: api/RateManagement/set-rate
        [HttpPost("set-rate")]
        public async Task<IActionResult> SetRate(SetRateDto setRateDto)
        {
            // Verify customer and item exist
            var customer = await _customerRepository.GetByIdAsync(setRateDto.CustomerId);
            var item = await _itemRepository.GetByIdAsync(setRateDto.ItemId);

            if (customer == null)
                return BadRequest("Customer not found");
            if (item == null)
                return BadRequest("Item not found");

            var success = await _rateRepository.SetCustomerItemRateAsync(
                setRateDto.CustomerId, setRateDto.ItemId, setRateDto.Rate);

            if (success)
                return NoContent();
            else
                return StatusCode(500, "Failed to set rate");
        }

        // POST: api/RateManagement/bulk-update
        [HttpPost("bulk-update")]
        public async Task<IActionResult> BulkUpdateRates(BulkRateUpdateDto bulkUpdateDto)
        {
            foreach (var rateDto in bulkUpdateDto.Rates)
            {
                await _rateRepository.SetCustomerItemRateAsync(
                    rateDto.CustomerId, rateDto.ItemId, rateDto.Rate);
            }

            return NoContent();
        }

        // DELETE: api/RateManagement/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRate(int id)
        {
            var result = await _rateRepository.DeleteRateAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/RateManagement/customer/{customerId}/item/{itemId}
        [HttpDelete("customer/{customerId}/item/{itemId}")]
        public async Task<IActionResult> DeleteRateByCustomerAndItem(int customerId, int itemId)
        {
            var result = await _rateRepository.DeleteRateByCustomerAndItemAsync(customerId, itemId);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // GET: api/RateManagement/get-rate/{customerId}/{itemId}
        [HttpGet("get-rate/{customerId}/{itemId}")]
        public async Task<ActionResult<decimal?>> GetCustomerItemRate(int customerId, int itemId)
        {
            var rate = await _rateRepository.GetCustomerItemRateAsync(customerId, itemId);
            return Ok(new { rate = rate });
        }

        // GET: api/RateManagement/rate-exists/{customerId}/{itemId}
        [HttpGet("rate-exists/{customerId}/{itemId}")]
        public async Task<ActionResult<bool>> RateExists(int customerId, int itemId)
        {
            var exists = await _rateRepository.RateExistsAsync(customerId, itemId);
            return Ok(new { exists = exists });
        }
    }
}