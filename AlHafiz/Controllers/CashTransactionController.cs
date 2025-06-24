using AlHafiz.DTOs;
using AlHafiz.Models;
using AlHafiz.Services.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace AlHafiz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashTransactionController : ControllerBase
    {
        private readonly ICashTransactionRepository _cashTransactionRepository;

        public CashTransactionController(ICashTransactionRepository cashTransactionRepository)
        {
            _cashTransactionRepository = cashTransactionRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CashTransactionDto>>> GetCashTransactions()
        {
            var cashTransactions = await _cashTransactionRepository.GetCashTransactionsWithDetailsAsync();
            var cashTransactionsDto = cashTransactions.Select(MapCashTransactionToDto);

            return Ok(cashTransactionsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CashTransactionDto>> GetCashTransaction(int id)
        {
            var cashTransaction = await _cashTransactionRepository.GetCashTransactionWithDetailsAsync(id);

            if (cashTransaction == null)
                return NotFound();

            var cashTransactionDto = MapCashTransactionToDto(cashTransaction);

            return Ok(cashTransactionDto);
        }

        [HttpPost]
        public async Task<ActionResult<CashTransactionDto>> CreateCashTransaction(CreateCashTransactionDto createCashTransactionDto)
        {
            var cashTransaction = new CashTransaction
            {
                CustomerId = createCashTransactionDto.CustomerId,
                PaymentType = createCashTransactionDto.PaymentType,
                BankId = createCashTransactionDto.BankId,
                PaymentDetails = createCashTransactionDto.PaymentDetails,
                Amount = createCashTransactionDto.Amount,
                IsCashReceived = createCashTransactionDto.IsCashReceived,
                Details = createCashTransactionDto.Details,
                CreatedAt = DateTime.Now
            };

            var createdCashTransaction = await _cashTransactionRepository.AddAsync(cashTransaction);

            var cashTransactionDto = MapCashTransactionToDto(createdCashTransaction);

            return CreatedAtAction(nameof(GetCashTransaction), new { id = cashTransactionDto.Id }, cashTransactionDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCashTransaction(int id, CreateCashTransactionDto updateCashTransactionDto)
        {
            var cashTransaction = await _cashTransactionRepository.GetByIdAsync(id);

            if (cashTransaction == null)
                return NotFound();

            cashTransaction.CustomerId = updateCashTransactionDto.CustomerId;
            cashTransaction.PaymentType = updateCashTransactionDto.PaymentType;
            cashTransaction.BankId = updateCashTransactionDto.BankId;
            cashTransaction.PaymentDetails = updateCashTransactionDto.PaymentDetails;
            cashTransaction.Amount = updateCashTransactionDto.Amount;
            cashTransaction.IsCashReceived = updateCashTransactionDto.IsCashReceived;
            cashTransaction.Details = updateCashTransactionDto.Details;
            cashTransaction.UpdatedAt = DateTime.Now;

            await _cashTransactionRepository.UpdateAsync(cashTransaction);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCashTransaction(int id)
        {
            var result = await _cashTransactionRepository.DeleteAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<CashTransactionDto>>> FilterCashTransactions([FromQuery] CashTransactionFilterDto filter)
        {
            var cashTransactions = await _cashTransactionRepository.FilterCashTransactionsAsync(filter);
            var cashTransactionsDto = cashTransactions.Select(MapCashTransactionToDto);

            return Ok(cashTransactionsDto);
        }

        private CashTransactionDto MapCashTransactionToDto(CashTransaction cashTransaction)
        {
            return new CashTransactionDto
            {
                Id = cashTransaction.Id,
                CustomerId = cashTransaction.CustomerId,
                CustomerName = cashTransaction.Customer?.Name,
                PaymentType = cashTransaction.PaymentType,
                BankId = cashTransaction.BankId,
                BankName = cashTransaction.Bank?.Name,
                PaymentDetails = cashTransaction.PaymentDetails,
                Amount = cashTransaction.Amount,
                IsCashReceived = cashTransaction.IsCashReceived,
                Details = cashTransaction.Details,
                CreatedAt = cashTransaction.CreatedAt
            };
        }
    }
}
