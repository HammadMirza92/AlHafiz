using AlHafiz.DTOs;
using AlHafiz.Services.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace AlHafiz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;

        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockDto>>> GetStocks()
        {
            var stocks = await _stockRepository.GetStocksWithItemDetailsAsync();
            var stocksDto = stocks.Select(s => new StockDto
            {
                Id = s.Id,
                ItemId = s.ItemId,
                ItemName = s.Item?.Name,
                Quantity = s.Quantity
            });

            return Ok(stocksDto);
        }

        [HttpGet("item/{itemId}")]
        public async Task<ActionResult<StockDto>> GetStockByItemId(int itemId)
        {
            var stock = await _stockRepository.GetStockByItemIdAsync(itemId);

            if (stock == null)
                return NotFound();

            var stockDto = new StockDto
            {
                Id = stock.Id,
                ItemId = stock.ItemId,
                ItemName = stock.Item?.Name,
                Quantity = stock.Quantity
            };

            return Ok(stockDto);
        }
    }
}
