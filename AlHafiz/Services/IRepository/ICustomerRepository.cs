using AlHafiz.Models;
using AlHafiz.Services.IRepository.Base;

namespace AlHafiz.Services.IRepository
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm);
        Task<IEnumerable<Customer>> GetAllCustomers();
    }
}
