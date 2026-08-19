using ABCPharmacy.Api.DTOs;
using ABCPharmacy.Api.Models;
using ABCPharmacy.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ABCPharmacy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicinesController : ControllerBase
{
    private readonly IMedicineService _service;
    public MedicinesController(IMedicineService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search = null)
    {
        var items = await _service.GetAllAsync(search);
        return Ok(items);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MedicineCreateDto dto)
    {
        var med = new Medicine
        {
            FullName = dto.FullName,
            Notes = dto.Notes,
            ExpiryDate = dto.ExpiryDate,
            Quantity = dto.Quantity,
            Price = Math.Round(dto.Price, 2),
            Brand = dto.Brand
        };

        var created = await _service.AddAsync(med);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }
}