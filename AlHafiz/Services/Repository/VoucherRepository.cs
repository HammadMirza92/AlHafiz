using AlHafiz.AppDbContext;
using AlHafiz.DTOs;
using AlHafiz.Enums;
using AlHafiz.Models;
using AlHafiz.Services.IRepository;
using AlHafiz.Services.Repository.Base;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
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

            // Filter by FromDate
            if (filter.FromDate.HasValue)
                query = query.Where(v => v.CreatedAt >= filter.FromDate.Value);

            // Filter by ToDate
            if (filter.ToDate.HasValue)
            {
                var toDatePlusOne = filter.ToDate.Value.AddDays(1);
                query = query.Where(v => v.CreatedAt < toDatePlusOne);
            }

            // Filter by CustomerId
            if (filter.CustomerId.HasValue)
                query = query.Where(v => v.CustomerId == filter.CustomerId.Value);

            // Filter by ExpenseHeadId
            if (filter.ExpenseHeadId.HasValue)
                query = query.Where(v => v.ExpenseHeadId == filter.ExpenseHeadId.Value);

            // Filter by VoucherType with special case for Purchase
            if (filter.VoucherType.HasValue)
            {
                if (filter.VoucherType.Value == VoucherType.Purchase)
                {
                    // Fetch vouchers of type Purchase or type 1 (Purchase)
                    query = query.Where(v => v.VoucherType == VoucherType.Purchase || v.VoucherType == (VoucherType)1);
                }
                else
                {
                    // Filter by specific VoucherType
                    query = query.Where(v => v.VoucherType == filter.VoucherType.Value);
                }
            }

            // Return the filtered results
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

        public async Task<IEnumerable<Voucher>> FilterVouchersByPaymentTypeAndDateAsync(PaymentType paymentType, DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.Vouchers
                .Where(v => v.PaymentType == paymentType)
                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(v => v.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(v => v.CreatedAt <= toDate.Value);

            return await query.ToListAsync();
        }
        // Method to get predefined rate for customer-item combination
        public async Task<decimal?> GetCustomerItemRateAsync(int customerId, int itemId)
        {
            var rate = await _context.CustomerItemRates
                .FirstOrDefaultAsync(r => r.CustomerId == customerId && r.ItemId == itemId);
            return rate?.Rate;
        }

        // Updated method to set item rate for customer (now updates the predefined rates table)
        public async Task SetItemRateForCustomerAsync(int customerId, int itemId, decimal rate)
        {
            var existingRate = await _context.CustomerItemRates
                .FirstOrDefaultAsync(r => r.CustomerId == customerId && r.ItemId == itemId);

            if (existingRate != null)
            {
                // Update existing rate
                existingRate.Rate = rate;
                existingRate.UpdatedAt = DateTime.Now;
                _context.CustomerItemRates.Update(existingRate);
            }
            else
            {
                // Create new rate
                var newRate = new CustomerItemRate
                {
                    CustomerId = customerId,
                    ItemId = itemId,
                    Rate = rate,
                    CreatedAt = DateTime.Now
                };
                _context.CustomerItemRates.Add(newRate);
            }

            await _context.SaveChangesAsync();
        }

        // Method to auto-populate rates when creating voucher items
        public async Task<VoucherItem> CreateVoucherItemWithAutoRateAsync(VoucherItem voucherItem, int? customerId = null)
        {
            // If customer is provided and rate is not set, try to get predefined rate
            if (customerId.HasValue && voucherItem.Rate == 0)
            {
                var predefinedRate = await GetCustomerItemRateAsync(customerId.Value, voucherItem.ItemId);
                if (predefinedRate.HasValue)
                {
                    voucherItem.Rate = predefinedRate.Value;
                }
            }

            // Calculate amount if rate is available
            if (voucherItem.Rate > 0)
            {
                voucherItem.Amount = voucherItem.NetWeight * voucherItem.Rate;
            }

            voucherItem.CreatedAt = DateTime.Now;
            _context.VoucherItems.Add(voucherItem);
            await _context.SaveChangesAsync();

            return voucherItem;
        }

        // Method to create voucher with auto-populated rates
        public async Task<Voucher> CreateVoucherWithAutoRatesAsync(Voucher voucher, List<VoucherItem> voucherItems)
        {
            // Create the voucher first
            voucher.CreatedAt = DateTime.Now;
            _context.Vouchers.Add(voucher);
            await _context.SaveChangesAsync();

            // Create voucher items with auto-populated rates
            foreach (var item in voucherItems)
            {
                item.VoucherId = voucher.Id;

                // Auto-populate rate if not provided and customer exists
                if (voucher.CustomerId.HasValue && item.Rate == 0)
                {
                    var predefinedRate = await GetCustomerItemRateAsync(voucher.CustomerId.Value, item.ItemId);
                    if (predefinedRate.HasValue)
                    {
                        item.Rate = predefinedRate.Value;
                    }
                }

                // Calculate amount
                if (item.Rate > 0)
                {
                    item.Amount = item.NetWeight * item.Rate;
                }

                item.CreatedAt = DateTime.Now;
                _context.VoucherItems.Add(item);
            }

            // Update voucher total amount
            voucher.Amount = voucherItems.Sum(vi => vi.Amount);
            _context.Vouchers.Update(voucher);

            await _context.SaveChangesAsync();

            // Return voucher with items
            return await _context.Vouchers
                .Include(v => v.VoucherItems)
                .ThenInclude(vi => vi.Item)
                .Include(v => v.Customer)
                .FirstOrDefaultAsync(v => v.Id == voucher.Id);
        }

        // Method to get suggested rates for voucher creation
        public async Task<Dictionary<int, decimal?>> GetSuggestedRatesForCustomerAsync(int customerId, List<int> itemIds)
        {
            var rates = await _context.CustomerItemRates
                .Where(r => r.CustomerId == customerId && itemIds.Contains(r.ItemId))
                .ToDictionaryAsync(r => r.ItemId, r => (decimal?)r.Rate);

            // Fill in null values for items without predefined rates
            var result = new Dictionary<int, decimal?>();
            foreach (var itemId in itemIds)
            {
                result[itemId] = rates.ContainsKey(itemId) ? rates[itemId] : null;
            }

            return result;
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
