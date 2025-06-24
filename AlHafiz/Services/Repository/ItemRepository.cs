using AlHafiz.AppDbContext;
using AlHafiz.Models;
using AlHafiz.Services.IRepository;
using AlHafiz.Services.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace AlHafiz.Services.Repository
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Item>> SearchItemsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            return await _context.Items
                .Where(i => i.Name.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
