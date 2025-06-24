using AlHafiz.DTOs;
using AlHafiz.Models;
using AlHafiz.Services.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace AlHafiz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;

        public ItemController(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetItems()
        {
            var items = await _itemRepository.GetAllAsync();
            var itemsDto = items.Select(i => new ItemDto
            {
                Id = i.Id,
                Name = i.Name
            });

            return Ok(itemsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItem(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);

            if (item == null)
                return NotFound();

            var itemDto = new ItemDto
            {
                Id = item.Id,
                Name = item.Name
            };

            return Ok(itemDto);
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItem(CreateItemDto createItemDto)
        {
            var item = new Item
            {
                Name = createItemDto.Name,
                CreatedAt = DateTime.Now
            };

            var createdItem = await _itemRepository.AddAsync(item);

            var itemDto = new ItemDto
            {
                Id = createdItem.Id,
                Name = createdItem.Name
            };

            return CreatedAtAction(nameof(GetItem), new { id = itemDto.Id }, itemDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, UpdateItemDto updateItemDto)
        {
            var item = await _itemRepository.GetByIdAsync(id);

            if (item == null)
                return NotFound();

            item.Name = updateItemDto.Name;
            item.UpdatedAt = DateTime.Now;

            await _itemRepository.UpdateAsync(item);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var result = await _itemRepository.DeleteAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ItemDto>>> SearchItems([FromQuery] string searchTerm)
        {
            var items = await _itemRepository.SearchItemsAsync(searchTerm);

            var itemsDto = items.Select(i => new ItemDto
            {
                Id = i.Id,
                Name = i.Name
            });

            return Ok(itemsDto);
        }
    }
}
