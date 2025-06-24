using AlHafiz.AppDbContext;
using AlHafiz.DTOs;
using AlHafiz.Enums;
using AlHafiz.Models;
using AlHafiz.Services.IRepository;
using AlHafiz.Services.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace AlHafiz.Services.Repository
{
    public class VoucherRepository : GenericRepository<Voucher>, IVoucherRepository
    {
        private readonly IStockRepository _stockRepository;

        public VoucherRepository(ApplicationDbContext context, IStockRepository stockRepository) : base(context)
        {
            _stockRepository = stockRepository;
        }

        public async Task<Voucher> GetVoucherWithDetailsAsync(int id)
        {
            return await _context.Vouchers
                .Include(v => v.Customer)
                .Include(v => v.Bank)
                .Include(v => v.ExpenseHead)
                .Include(v => v.VoucherItems)
                    .ThenInclude(vi => vi.Item)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Voucher>> GetVouchersWithDetailsAsync()
        {
            return await _context.Vouchers
                .Include(v => v.Customer)
                .Include(v => v.Bank)
                .Include(v => v.ExpenseHead)
                .Include(v => v.VoucherItems)
            .ThenInclude(vi => vi.Item)
                .ToListAsync();
        }

        public async Task<IEnumerable<Voucher>> FilterVouchersAsync(VoucherFilterDto filter)
        {
            var query = _context.Vouchers
                .Include(v => v.Customer)
                .Include(v => v.Bank)
                .Include(v => v.ExpenseHead)
                .Include(v => v.VoucherItems)
                    .ThenInclude(vi => vi.Item)
                .AsQueryable();

            if (filter.FromDate.HasValue)
                query = query.Where(v => v.CreatedAt >= filter.FromDate.Value);

            if (filter.ToDate.HasValue)
            {
                var toDatePlusOne = filter.ToDate.Value.AddDays(1);
                query = query.Where(v => v.CreatedAt < toDatePlusOne);
            }

            if (filter.CustomerId.HasValue)
                query = query.Where(v => v.CustomerId == filter.CustomerId.Value);

            if (filter.ExpenseHeadId.HasValue)
                query = query.Where(v => v.ExpenseHeadId == filter.ExpenseHeadId.Value);

            if (filter.VoucherType.HasValue)
                query = query.Where(v => v.VoucherType == filter.VoucherType.Value);

            return await query.ToListAsync();
        }

        public async Task<Voucher> CreateVoucherWithItemsAsync(Voucher voucher, IEnumerable<VoucherItem> voucherItems)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.Vouchers.AddAsync(voucher);
                    await _context.SaveChangesAsync();

                    if (voucherItems != null && voucherItems.Any())
                    {
                        foreach (var item in voucherItems)
                        {
                            item.VoucherId = voucher.Id;
                            await _context.VoucherItems.AddAsync(item);
                        }

                        await _context.SaveChangesAsync();

                        // Update stock based on voucher type
                        if (voucher.VoucherType == VoucherType.Purchase)
                        {
                            foreach (var item in voucherItems)
                            {
                                var stock = await _context.Stocks
                                    .FirstOrDefaultAsync(s => s.ItemId == item.ItemId);

                                if (stock != null)
                                {
                                    stock.Quantity += item.NetWeight;
                                    stock.UpdatedAt = DateTime.Now;
                                }
                                else
                                {
                                    await _context.Stocks.AddAsync(new Stock
                                    {
                                        ItemId = item.ItemId,
                                        Quantity = item.NetWeight,
                                        CreatedAt = DateTime.Now
                                    });
                                }
                            }
                        }
                        else if (voucher.VoucherType == VoucherType.Sale)
                        {
                            foreach (var item in voucherItems)
                            {
                                var stock = await _context.Stocks
                                    .FirstOrDefaultAsync(s => s.ItemId == item.ItemId);

                                if (stock != null)
                                {
                                    stock.Quantity -= item.NetWeight;
                                    stock.UpdatedAt = DateTime.Now;
                                }
                            }
                        }

                        await _context.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();

                    return await GetVoucherWithDetailsAsync(voucher.Id);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<Voucher> UpdateVoucherWithItemsAsync(Voucher voucher, IEnumerable<VoucherItem> voucherItems)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Get existing voucher with its items
                    var existingVoucher = await GetVoucherWithDetailsAsync(voucher.Id);
                    if (existingVoucher == null)
                        return null;

                    // Revert stock changes from existing voucher items
                    if (existingVoucher.VoucherType == VoucherType.Purchase)
                    {
                        foreach (var item in existingVoucher.VoucherItems)
                        {
                            var stock = await _context.Stocks
                                .FirstOrDefaultAsync(s => s.ItemId == item.ItemId);

                            if (stock != null)
                            {
                                stock.Quantity -= item.NetWeight;
                                stock.UpdatedAt = DateTime.Now;
                            }
                        }
                    }
                    else if (existingVoucher.VoucherType == VoucherType.Sale)
                    {
                        foreach (var item in existingVoucher.VoucherItems)
                        {
                            var stock = await _context.Stocks
                                .FirstOrDefaultAsync(s => s.ItemId == item.ItemId);

                            if (stock != null)
                            {
                                stock.Quantity += item.NetWeight;
                                stock.UpdatedAt = DateTime.Now;
                            }
                        }
                    }

                    // Remove existing voucher items
                    _context.VoucherItems.RemoveRange(existingVoucher.VoucherItems);

                    // Update voucher properties
                    existingVoucher.VoucherType = voucher.VoucherType;
                    existingVoucher.CustomerId = voucher.CustomerId;
                    existingVoucher.PaymentType = voucher.PaymentType;
                    existingVoucher.BankId = voucher.BankId;
                    existingVoucher.PaymentDetails = voucher.PaymentDetails;
                    existingVoucher.ExpenseHeadId = voucher.ExpenseHeadId;
                    existingVoucher.Amount = voucher.Amount;
                    existingVoucher.GariNo = voucher.GariNo;
                    existingVoucher.Details = voucher.Details;
                    existingVoucher.UpdatedAt = DateTime.Now;

                    _context.Vouchers.Update(existingVoucher);
                    await _context.SaveChangesAsync();

                    // Add new voucher items
                    if (voucherItems != null && voucherItems.Any())
                    {
                        foreach (var item in voucherItems)
                        {
                            item.VoucherId = voucher.Id;
                            await _context.VoucherItems.AddAsync(item);
                        }

                        await _context.SaveChangesAsync();

                        // Update stock based on voucher type
                        if (voucher.VoucherType == VoucherType.Purchase)
                        {
                            foreach (var item in voucherItems)
                            {
                                var stock = await _context.Stocks
                                    .FirstOrDefaultAsync(s => s.ItemId == item.ItemId);

                                if (stock != null)
                                {
                                    stock.Quantity += item.NetWeight;
                                    stock.UpdatedAt = DateTime.Now;
                                }
                                else
                                {
                                    await _context.Stocks.AddAsync(new Stock
                                    {
                                        ItemId = item.ItemId,
                                        Quantity = item.NetWeight,
                                        CreatedAt = DateTime.Now
                                    });
                                }
                            }
                        }
                        else if (voucher.VoucherType == VoucherType.Sale)
                        {
                            foreach (var item in voucherItems)
                            {
                                var stock = await _context.Stocks
                                    .FirstOrDefaultAsync(s => s.ItemId == item.ItemId);

                                if (stock != null)
                                {
                                    stock.Quantity -= item.NetWeight;
                                    stock.UpdatedAt = DateTime.Now;
                                }
                            }
                        }

                        await _context.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();

                    return await GetVoucherWithDetailsAsync(voucher.Id);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
