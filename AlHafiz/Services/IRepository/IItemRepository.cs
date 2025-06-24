using AlHafiz.Models;
using AlHafiz.Services.IRepository.Base;

namespace AlHafiz.Services.IRepository
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        Task<IEnumerable<Item>> SearchItemsAsync(string searchTerm);
    }
}
