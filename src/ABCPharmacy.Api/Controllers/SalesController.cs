using ABCPharmacy.Api.DTOs;
using ABCPharmacy.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ABCPharmacy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISalesService _service;
    public SalesController(ISalesService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Record([FromBody] SaleCreateDto dto)
    {
        var sale = await _service.RecordSaleAsync(dto.MedicineId, dto.Quantity);
        return CreatedAtAction(null, sale);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());
}