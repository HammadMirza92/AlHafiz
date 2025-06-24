using AlHafiz.DTOs;
using AlHafiz.Models;
using AlHafiz.Services.IRepository.Base;

namespace AlHafiz.Services.IRepository
{
    public interface ICashTransactionRepository : IGenericRepository<CashTransaction>
    {
        Task<IEnumerable<CashTransaction>> GetCashTransactionsWithDetailsAsync();
        Task<CashTransaction> GetCashTransactionWithDetailsAsync(int id);
        Task<IEnumerable<CashTransaction>> FilterCashTransactionsAsync(CashTransactionFilterDto filter);
    }
}
