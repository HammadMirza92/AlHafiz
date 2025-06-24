using AlHafiz.DTOs;
using AlHafiz.Models;
using AlHafiz.Services.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace AlHafiz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IBankRepository _bankRepository;

        public BankController(IBankRepository bankRepository)
        {
            _bankRepository = bankRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BankDto>>> GetBanks()
        {
            var banks = await _bankRepository.GetAllAsync();
            var banksDto = banks.Select(b => new BankDto
            {
                Id = b.Id,
                Name = b.Name
            });

            return Ok(banksDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BankDto>> GetBank(int id)
        {
            var bank = await _bankRepository.GetByIdAsync(id);

            if (bank == null)
                return NotFound();

            var bankDto = new BankDto
            {
                Id = bank.Id,
                Name = bank.Name
            };

            return Ok(bankDto);
        }

        [HttpPost]
        public async Task<ActionResult<BankDto>> CreateBank(CreateBankDto createBankDto)
        {
            var bank = new Bank
            {
                Name = createBankDto.Name,
                CreatedAt = DateTime.Now
            };

            var createdBank = await _bankRepository.AddAsync(bank);

            var bankDto = new BankDto
            {
                Id = createdBank.Id,
                Name = createdBank.Name
            };

            return CreatedAtAction(nameof(GetBank), new { id = bankDto.Id }, bankDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBank(int id, UpdateBankDto updateBankDto)
        {
            var bank = await _bankRepository.GetByIdAsync(id);

            if (bank == null)
                return NotFound();

            bank.Name = updateBankDto.Name;
            bank.UpdatedAt = DateTime.Now;

            await _bankRepository.UpdateAsync(bank);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBank(int id)
        {
            var result = await _bankRepository.DeleteAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
