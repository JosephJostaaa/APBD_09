using Microsoft.AspNetCore.Mvc;
using Tutorial9.Dto;
using Tutorial9.Services;

namespace Tutorial9.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController : ControllerBase
{
    private IWarehouseService _warehouseService;
    
    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }
    
    [HttpPost("fulfill")]
    public async Task<IActionResult> FulfillOrderAsync([FromBody] ProductWarehouseDto productWarehouseDto, CancellationToken ct)
    {
        var result = await _warehouseService.FulfillOrderAsync(ct, productWarehouseDto);
        return Ok(result);
    }
    
    
}