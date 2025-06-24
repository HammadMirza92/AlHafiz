using AlHafiz.AppDbContext;
using AlHafiz.Models;
using AlHafiz.Services.IRepository;
using AlHafiz.Services.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace AlHafiz.Services.Repository
{
    public class StockRepository : GenericRepository<Stock>, IStockRepository
    {
        public StockRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Stock>> GetStocksWithItemDetailsAsync()
        {
            return await _context.Stocks
                .Include(s => s.Item)
                .ToListAsync();
        }

        public async Task<Stock> GetStockByItemIdAsync(int itemId)
        {
            return await _context.Stocks
                .Include(s => s.Item)
                .FirstOrDefaultAsync(s => s.ItemId == itemId);
        }

        public async Task<Stock> UpdateStockQuantityAsync(int itemId, decimal quantityChange)
        {
            var stock = await _context.Stocks
                .FirstOrDefaultAsync(s => s.ItemId == itemId);

            if (stock != null)
            {
                stock.Quantity += quantityChange;
                stock.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
            }
            else if (quantityChange > 0)
            {
                stock = new Stock
                {
                    ItemId = itemId,
                    Quantity = quantityChange,
                    CreatedAt = DateTime.Now
                };

                await _context.Stocks.AddAsync(stock);
                await _context.SaveChangesAsync();
            }

            return stock;
        }
    }
}
