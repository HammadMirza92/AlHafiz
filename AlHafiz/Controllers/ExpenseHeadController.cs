using AlHafiz.DTOs;
using AlHafiz.Models;
using AlHafiz.Services.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace AlHafiz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseHeadController : ControllerBase
    {
        private readonly IExpenseHeadRepository _expenseHeadRepository;

        public ExpenseHeadController(IExpenseHeadRepository expenseHeadRepository)
        {
            _expenseHeadRepository = expenseHeadRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseHeadDto>>> GetExpenseHeads()
        {
            var expenseHeads = await _expenseHeadRepository.GetAllAsync();
            var expenseHeadsDto = expenseHeads.Select(e => new ExpenseHeadDto
            {
                Id = e.Id,
                Name = e.Name
            });

            return Ok(expenseHeadsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseHeadDto>> GetExpenseHead(int id)
        {
            var expenseHead = await _expenseHeadRepository.GetByIdAsync(id);

            if (expenseHead == null)
                return NotFound();

            var expenseHeadDto = new ExpenseHeadDto
            {
                Id = expenseHead.Id,
                Name = expenseHead.Name
            };

            return Ok(expenseHeadDto);
        }

        [HttpPost]
        public async Task<ActionResult<ExpenseHeadDto>> CreateExpenseHead(CreateExpenseHeadDto createExpenseHeadDto)
        {
            var expenseHead = new ExpenseHead
            {
                Name = createExpenseHeadDto.Name,
                CreatedAt = DateTime.Now
            };

            var createdExpenseHead = await _expenseHeadRepository.AddAsync(expenseHead);

            var expenseHeadDto = new ExpenseHeadDto
            {
                Id = createdExpenseHead.Id,
                Name = createdExpenseHead.Name
            };

            return CreatedAtAction(nameof(GetExpenseHead), new { id = expenseHeadDto.Id }, expenseHeadDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpenseHead(int id, UpdateExpenseHeadDto updateExpenseHeadDto)
        {
            var expenseHead = await _expenseHeadRepository.GetByIdAsync(id);

            if (expenseHead == null)
                return NotFound();

            expenseHead.Name = updateExpenseHeadDto.Name;
            expenseHead.UpdatedAt = DateTime.Now;

            await _expenseHeadRepository.UpdateAsync(expenseHead);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpenseHead(int id)
        {
            var result = await _expenseHeadRepository.DeleteAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
