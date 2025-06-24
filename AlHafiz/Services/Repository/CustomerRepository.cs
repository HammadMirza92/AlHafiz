using AlHafiz.AppDbContext;
using AlHafiz.Models;
using AlHafiz.Services.IRepository;
using AlHafiz.Services.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace AlHafiz.Services.Repository
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            return await _context.Customers
                .Where(c => c.Name.Contains(searchTerm))
                .ToListAsync();
        }
        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            return await _context.Customers.Include(p=> p.CashTransactions).Include(c=> c.Vouchers).ToListAsync();
        }
    }
}
