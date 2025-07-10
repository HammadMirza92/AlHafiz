using AlHafiz.DTOs;
using AlHafiz.Enums;
using AlHafiz.Models;
using AlHafiz.Services.IRepository.Base;

namespace AlHafiz.Services.IRepository
{
    public interface ICashTransactionRepository : IGenericRepository<CashTransaction>
    {
        Task<IEnumerable<CashTransaction>> GetCashTransactionsWithDetailsAsync();
        Task<CashTransaction> GetCashTransactionWithDetailsAsync(int id);
        Task<IEnumerable<CashTransaction>> FilterCashTransactionsAsync(CashTransactionFilterDto filter);
        Task<IEnumerable<CashTransaction>> GetTransactionsByCustomerAndDateAsync(int customerId, DateTime? fromDate, DateTime? toDate, PaymentType paymentType);
        Task<BalanceTransaction> GetLatestBalanceAsync(int customerId, PaymentType paymentType);
        Task UpdateBalanceAsync(int customerId, PaymentType paymentType, decimal amountSpent);


    }
}
