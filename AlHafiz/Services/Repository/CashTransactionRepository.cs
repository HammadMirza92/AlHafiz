using AlHafiz.AppDbContext;
using AlHafiz.DTOs;
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
