using AlHafiz.AppDbContext;
using AlHafiz.Models;
using AlHafiz.Services.IRepository;
using AlHafiz.Services.Repository.Base;

namespace AlHafiz.Services.Repository
{
    public class BankRepository : GenericRepository<Bank>, IBankRepository
    {
        public BankRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
