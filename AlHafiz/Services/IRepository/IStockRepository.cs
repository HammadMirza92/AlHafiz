using AlHafiz.Models;
using AlHafiz.Services.IRepository.Base;

namespace AlHafiz.Services.IRepository
{
    public interface IStockRepository : IGenericRepository<Stock>
    {
        Task<IEnumerable<Stock>> GetStocksWithItemDetailsAsync();
        Task<Stock> GetStockByItemIdAsync(int itemId);
        Task<Stock> UpdateStockQuantityAsync(int itemId, decimal quantityChange);
    }
}
