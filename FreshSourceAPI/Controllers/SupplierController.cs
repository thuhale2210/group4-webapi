using AutoMapper;
using FreshSourceAPI.DTOs;
using FreshSourceAPI.Entities;
using FreshSourceAPI.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FreshSourceAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly IGenericRepository<Supplier> _supplierRepo;
    private readonly IMapper _mapper;

    public SuppliersController(
        IGenericRepository<Supplier> supplierRepo,
        IMapper mapper)
    {
        _supplierRepo = supplierRepo;
        _mapper = mapper;
    }

    // GET: api/suppliers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SupplierReadDto>>> GetSuppliers()
    {
        var suppliers = await _supplierRepo.GetAllAsync();
        var result = _mapper.Map<IEnumerable<SupplierReadDto>>(suppliers);
        return Ok(result);
    }

    // GET: api/suppliers/5
    [HttpGet("{id:int}", Name = "GetSupplierById")]
    public async Task<ActionResult<SupplierReadDto>> GetSupplierById(int id)
    {
        var supplier = await _supplierRepo.GetByIdAsync(id);
        if (supplier == null)
        {
            return NotFound();
        }

        var result = _mapper.Map<SupplierReadDto>(supplier);
        return Ok(result);
    }

    // POST: api/suppliers
    [HttpPost]
    public async Task<ActionResult<SupplierReadDto>> CreateSupplier(
        SupplierCreateDto createDto)
    {
        var supplier = _mapper.Map<Supplier>(createDto);
        await _supplierRepo.AddAsync(supplier);

        var readDto = _mapper.Map<SupplierReadDto>(supplier);

        return CreatedAtAction(
            nameof(GetSupplierById),
            new { id = supplier.Id },
            readDto);
    }

    // PUT: api/suppliers/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateSupplier(int id, SupplierUpdateDto updateDto)
    {
        var supplier = await _supplierRepo.GetByIdAsync(id);
        if (supplier == null)
        {
            return NotFound();
        }

        _mapper.Map(updateDto, supplier);
        await _supplierRepo.UpdateAsync(supplier);

        return NoContent();
    }

    // PATCH: api/suppliers/5
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> PartiallyUpdateSupplier(
        int id,
        JsonPatchDocument<SupplierUpdateDto> patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest();
        }

        var supplier = await _supplierRepo.GetByIdAsync(id);
        if (supplier == null)
        {
            return NotFound();
        }

        var supplierToPatch = _mapper.Map<SupplierUpdateDto>(supplier);

        patchDoc.ApplyTo(supplierToPatch, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _mapper.Map(supplierToPatch, supplier);
        await _supplierRepo.UpdateAsync(supplier);

        return NoContent();
    }

    // DELETE: api/suppliers/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSupplier(int id)
    {
        var supplier = await _supplierRepo.GetByIdAsync(id);
        if (supplier == null)
        {
            return NotFound();
        }

        await _supplierRepo.DeleteAsync(supplier);

        return NoContent();
    }
}