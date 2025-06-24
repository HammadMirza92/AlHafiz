using AlHafiz.AppDbContext;
using AlHafiz.Models;
using AlHafiz.Services.IRepository;
using AlHafiz.Services.Repository.Base;

namespace AlHafiz.Services.Repository
{
    public class ExpenseHeadRepository : GenericRepository<ExpenseHead>, IExpenseHeadRepository
    {
        public ExpenseHeadRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
