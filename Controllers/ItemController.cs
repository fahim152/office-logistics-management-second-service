using Microsoft.AspNetCore.Mvc;
using office_logistics_managament_cqrs_second_service.Models;
using office_logistics_managament_cqrs_second_service.Services;

namespace office_logistics_managament_cqrs_second_service.Controllers;


[Route("api/[controller]")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly ItemService _itemService;

    public ItemController(ItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Item>> GetAllItems()
    {
        var items = _itemService.GetAllItems();
        return Ok(items);
    }

    [HttpPost]
    public IActionResult CreateItem([FromBody] Item item)
    {
    
        _itemService.InsertItem(item);

        return Ok(item);
    }
}
