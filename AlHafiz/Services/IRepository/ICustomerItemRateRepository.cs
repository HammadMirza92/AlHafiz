using AlHafiz.Models;

namespace AlHafiz.Services.IRepository
{
    public interface ICustomerItemRateRepository
    {
        Task<IEnumerable<CustomerItemRate>> GetAllRatesAsync();
        Task<CustomerItemRate> GetRateByIdAsync(int id);
        Task<CustomerItemRate> GetRateByCustomerAndItemAsync(int customerId, int itemId);
        Task<IEnumerable<CustomerItemRate>> GetRatesByCustomerAsync(int customerId);
        Task<IEnumerable<CustomerItemRate>> GetRatesByItemAsync(int itemId);
        Task<CustomerItemRate> AddRateAsync(CustomerItemRate rate);
        Task<CustomerItemRate> UpdateRateAsync(CustomerItemRate rate);
        Task<bool> DeleteRateAsync(int id);
        Task<bool> DeleteRateByCustomerAndItemAsync(int customerId, int itemId);
        Task<decimal?> GetCustomerItemRateAsync(int customerId, int itemId);
        Task<bool> SetCustomerItemRateAsync(int customerId, int itemId, decimal rate);
        Task<bool> RateExistsAsync(int customerId, int itemId);
        Task<IEnumerable<CustomerItemRate>> GetRatesMatrixAsync();
    }
}