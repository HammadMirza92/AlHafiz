using AlHafiz.DTOs;
using AlHafiz.Enums;
using AlHafiz.Models;
using AlHafiz.Services.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace AlHafiz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherRepository _voucherRepository;

        public VoucherController(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VoucherDto>>> GetVouchers()
        {
            var vouchers = await _voucherRepository.GetVouchersWithDetailsAsync();
            var vouchersDto = vouchers.Select(MapVoucherToDto);

            return Ok(vouchersDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VoucherDto>> GetVoucher(int id)
        {
            var voucher = await _voucherRepository.GetVoucherWithDetailsAsync(id);

            if (voucher == null)
                return NotFound();

            var voucherDto = MapVoucherToDto(voucher);

            return Ok(voucherDto);
        }

        [HttpPost]
        public async Task<ActionResult<VoucherDto>> CreateVoucher(CreateVoucherDto createVoucherDto)
        {
            var voucher = new Voucher
            {
                VoucherType = createVoucherDto.VoucherType,
                CustomerId = createVoucherDto.CustomerId,
                PaymentType = createVoucherDto.PaymentType,
                BankId = createVoucherDto.BankId,
                PaymentDetails = createVoucherDto.PaymentDetails,
                ExpenseHeadId = createVoucherDto.ExpenseHeadId,
                Amount = createVoucherDto.Amount,
                GariNo = createVoucherDto.GariNo,
                Details = createVoucherDto.Details,
                CreatedAt = DateTime.Now
            };

            var voucherItems = new List<VoucherItem>();

            if (createVoucherDto.VoucherItems != null && createVoucherDto.VoucherItems.Any())
            {
                foreach (var item in createVoucherDto.VoucherItems)
                {
                    voucherItems.Add(new VoucherItem
                    {
                        ItemId = item.ItemId,
                        Weight = item.Weight,
                        Kat = item.Kat,
                        NetWeight = item.NetWeight,
                        DesiMan = item.DesiMan,
                        Rate = item.Rate,
                        Amount = item.Amount,
                        isTrackStock = item.isTrackStock,
                        CreatedAt = DateTime.Now
                    });
                }
            }

            var createdVoucher = await _voucherRepository.CreateVoucherWithItemsAsync(voucher, voucherItems);

            var voucherDto = MapVoucherToDto(createdVoucher);

            return CreatedAtAction(nameof(GetVoucher), new { id = voucherDto.Id }, voucherDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VoucherDto>> UpdateVoucher(int id, UpdateVoucherDto updateVoucherDto)
        {
            var existingVoucher = await _voucherRepository.GetVoucherWithDetailsAsync(id);

            if (existingVoucher == null)
                return NotFound();

            var voucher = new Voucher
            {
                Id = id,
                VoucherType = updateVoucherDto.VoucherType,
                CustomerId = updateVoucherDto.CustomerId,
                PaymentType = updateVoucherDto.PaymentType,
                BankId = updateVoucherDto.BankId,
                PaymentDetails = updateVoucherDto.PaymentDetails,
                ExpenseHeadId = updateVoucherDto.ExpenseHeadId,
                Amount = updateVoucherDto.Amount,
                GariNo = updateVoucherDto.GariNo,
                Details = updateVoucherDto.Details
            };

            var voucherItems = new List<VoucherItem>();

            if (updateVoucherDto.VoucherItems != null && updateVoucherDto.VoucherItems.Any())
            {
                foreach (var item in updateVoucherDto.VoucherItems)
                {
                    voucherItems.Add(new VoucherItem
                    {
                        Id = item.Id,
                        VoucherId = id,
                        ItemId = item.ItemId,
                        Weight = item.Weight,
                        Kat = item.Kat,
                        NetWeight = item.NetWeight,
                        DesiMan = item.DesiMan,
                        Rate = item.Rate,
                        Amount = item.Amount,
                        CreatedAt = DateTime.Now
                    });
                }
            }

            var updatedVoucher = await _voucherRepository.UpdateVoucherWithItemsAsync(voucher, voucherItems);

            if (updatedVoucher == null)
                return NotFound();

            var voucherDto = MapVoucherToDto(updatedVoucher);

            return Ok(voucherDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoucher(int id)
        {
            var result = await _voucherRepository.DeleteAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }
        [HttpGet("filter-vouchers")]
        public async Task<ActionResult<IEnumerable<VoucherDto>>> FilterVouchers([FromQuery] PaymentType paymentType, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var vouchers = await _voucherRepository.FilterVouchersByPaymentTypeAndDateAsync(paymentType, fromDate, toDate);
            var vouchersDto = vouchers.Select(MapVoucherToDto);

            return Ok(vouchersDto);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<VoucherDto>>> FilterVouchers([FromQuery] VoucherFilterDto filter)
        {
            var vouchers = await _voucherRepository.FilterVouchersAsync(filter);
            var vouchersDto = vouchers.Select(MapVoucherToDto);

            return Ok(vouchersDto);
        }
        [HttpPost("set-rate")]
        public async Task<IActionResult> SetRate([FromBody] SetRateDto setRateDto)
        {
            await _voucherRepository.SetItemRateForCustomerAsync(setRateDto.CustomerId, setRateDto.ItemId, setRateDto.Rate);
            return NoContent();
        }

        private VoucherDto MapVoucherToDto(Voucher voucher)
        {
            return new VoucherDto
            {
                Id = voucher.Id,
                VoucherType = voucher.VoucherType,
                CustomerId = voucher.CustomerId,
                CustomerName = voucher.Customer?.Name,
                PaymentType = voucher.PaymentType,
                BankId = voucher.BankId,
                BankName = voucher.Bank?.Name,
                PaymentDetails = voucher.PaymentDetails,
                ExpenseHeadId = voucher.ExpenseHeadId,
                ExpenseHeadName = voucher.ExpenseHead?.Name,
                Amount = voucher.Amount,
                GariNo = voucher.GariNo,
                Details = voucher.Details,
                CreatedAt = voucher.CreatedAt,
                UpdatedAt = voucher.UpdatedAt,
                VoucherItems = voucher.VoucherItems?.Select(vi => new VoucherItemDto
                {
                    Id = vi.Id,
                    VoucherId = vi.VoucherId,
                    ItemId = vi.ItemId,
                    ItemName = vi.Item?.Name,
                    Weight = vi.Weight,
                    Kat = vi.Kat,
                    NetWeight = vi.NetWeight,
                    DesiMan = vi.DesiMan,
                    Rate = vi.Rate,
                    Amount = vi.Amount
                }).ToList()
            };
        }
    }
}
