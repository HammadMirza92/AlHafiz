using AlHafiz.AppDbContext;
using AlHafiz.DTOs;
using AlHafiz.Enums;
using AlHafiz.Models;
using AlHafiz.Services.IRepository;
using AlHafiz.Services.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace AlHafiz.Services.Repository
{
    public class CashTransactionRepository : GenericRepository<CashTransaction>, ICashTransactionRepository
    {
        public CashTransactionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CashTransaction>> GetCashTransactionsWithDetailsAsync()
        {
            return await _context.CashTransactions
                .Include(ct => ct.Customer)
                .Include(ct => ct.Bank)
                .ToListAsync();
        }

        public async Task<CashTransaction> GetCashTransactionWithDetailsAsync(int id)
        {
            return await _context.CashTransactions
                .Include(ct => ct.Customer)
                .Include(ct => ct.Bank)
                .FirstOrDefaultAsync(ct => ct.Id == id);
        }
        public async Task<BalanceTransaction> GetLatestBalanceAsync(int customerId, PaymentType paymentType)
        {
            return await _context.BalanceTransactions
                .Where(bt => bt.CustomerId == customerId && bt.PaymentType == paymentType)
                .OrderByDescending(bt => bt.Date)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateBalanceAsync(int customerId, PaymentType paymentType, decimal amountSpent)
        {
            var latestBalance = await GetLatestBalanceAsync(customerId, paymentType);
            decimal newBalance = latestBalance?.ClosingBalance ?? 0 - amountSpent;

            var newBalanceTransaction = new BalanceTransaction
            {
                CustomerId = customerId,
                PaymentType = paymentType,
                OpeningBalance = newBalance,
                ClosingBalance = newBalance - amountSpent,
                Date = DateTime.Now
            };

            await _context.BalanceTransactions.AddAsync(newBalanceTransaction);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<CashTransaction>> GetTransactionsByCustomerAndDateAsync(int customerId, DateTime? fromDate, DateTime? toDate, PaymentType paymentType)
        {
            var query = _context.CashTransactions
                .Where(ct => ct.CustomerId == customerId && ct.PaymentType == paymentType)
                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(ct => ct.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(ct => ct.CreatedAt <= toDate.Value);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<CashTransaction>> FilterCashTransactionsAsync(CashTransactionFilterDto filter)
        {
            var query = _context.CashTransactions
                .Include(ct => ct.Customer)
                .Include(ct => ct.Bank)
                .AsQueryable();

            if (filter.FromDate.HasValue)
                query = query.Where(ct => ct.CreatedAt >= filter.FromDate.Value);

            if (filter.ToDate.HasValue)
            {
                var toDatePlusOne = filter.ToDate.Value.AddDays(1);
                query = query.Where(ct => ct.CreatedAt < toDatePlusOne);
            }

            if (filter.CustomerId.HasValue)
                query = query.Where(ct => ct.CustomerId == filter.CustomerId.Value);

            if (filter.IsCashReceived.HasValue)
                query = query.Where(ct => ct.IsCashReceived == filter.IsCashReceived.Value);

            return await query.ToListAsync();
        }
    }
}
