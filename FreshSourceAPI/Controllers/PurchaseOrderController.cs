using AutoMapper;
using FreshSourceAPI.DTOs;
using FreshSourceAPI.Entities;
using FreshSourceAPI.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FreshSourceAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseOrdersController : ControllerBase
{
    private readonly IGenericRepository<PurchaseOrder> _orderRepo;
    private readonly IGenericRepository<Product> _productRepo;
    private readonly IGenericRepository<Supplier> _supplierRepo;
    private readonly IMapper _mapper;

    public PurchaseOrdersController(
        IGenericRepository<PurchaseOrder> orderRepo,
        IGenericRepository<Product> productRepo,
        IGenericRepository<Supplier> supplierRepo,
        IMapper mapper)
    {
        _orderRepo = orderRepo;
        _productRepo = productRepo;
        _supplierRepo = supplierRepo;
        _mapper = mapper;
    }

    // GET: api/purchaseorders
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PurchaseOrderReadDto>>> GetOrders()
    {
        var orders = await _orderRepo.GetAllAsync();
        var result = _mapper.Map<IEnumerable<PurchaseOrderReadDto>>(orders);
        return Ok(result);
    }

    // GET: api/purchaseorders/5
    [HttpGet("{id:int}", Name = "GetOrderById")]
    public async Task<ActionResult<PurchaseOrderReadDto>> GetOrderById(int id)
    {
        var order = await _orderRepo.GetByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        var result = _mapper.Map<PurchaseOrderReadDto>(order);
        return Ok(result);
    }

    // POST: api/purchaseorders
    [HttpPost]
    public async Task<ActionResult<PurchaseOrderReadDto>> CreateOrder(
        PurchaseOrderCreateDto createDto)
    {
        // Validate Product
        var product = await _productRepo.GetByIdAsync(createDto.ProductId);
        if (product == null)
        {
            return BadRequest($"Product with id {createDto.ProductId} does not exist.");
        }

        // Validate Supplier
        var supplier = await _supplierRepo.GetByIdAsync(createDto.SupplierId);
        if (supplier == null)
        {
            return BadRequest($"Supplier with id {createDto.SupplierId} does not exist.");
        }

        // Map DTO -> entity
        var order = _mapper.Map<PurchaseOrder>(createDto);
        order.CreatedAt = DateTime.UtcNow;
        order.Status = "Pending";

        await _orderRepo.AddAsync(order);

        var readDto = _mapper.Map<PurchaseOrderReadDto>(order);

        return CreatedAtAction(
            nameof(GetOrderById),
            new { id = order.Id },
            readDto);
    }

    // PUT: api/purchaseorders/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateOrder(
        int id,
        PurchaseOrderUpdateDto updateDto)
    {
        var order = await _orderRepo.GetByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        // Track old status to detect transition to "Received"
        var wasReceivedBefore = string.Equals(order.Status, "Received",
            StringComparison.OrdinalIgnoreCase);

        // Map incoming values onto existing entity
        _mapper.Map(updateDto, order);

        // Business rule: if status changed to "Received", adjust product stock
        var isReceivedNow = string.Equals(order.Status, "Received",
            StringComparison.OrdinalIgnoreCase);

        if (!wasReceivedBefore && isReceivedNow)
        {
            var product = await _productRepo.GetByIdAsync(order.ProductId);
            if (product != null)
            {
                product.QuantityOnHand += order.Quantity;
                await _productRepo.UpdateAsync(product);
            }
        }

        await _orderRepo.UpdateAsync(order);

        return NoContent();
    }

    // PATCH: api/purchaseorders/5
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> PartiallyUpdateOrder(
        int id,
        JsonPatchDocument<PurchaseOrderUpdateDto> patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest();
        }

        var order = await _orderRepo.GetByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        var wasReceivedBefore = string.Equals(order.Status, "Received",
            StringComparison.OrdinalIgnoreCase);

        // Map entity -> DTO for patching
        var orderToPatch = _mapper.Map<PurchaseOrderUpdateDto>(order);

        patchDoc.ApplyTo(orderToPatch, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Map patched DTO back onto entity
        _mapper.Map(orderToPatch, order);

        var isReceivedNow = string.Equals(order.Status, "Received",
            StringComparison.OrdinalIgnoreCase);

        if (!wasReceivedBefore && isReceivedNow)
        {
            var product = await _productRepo.GetByIdAsync(order.ProductId);
            if (product != null)
            {
                product.QuantityOnHand += order.Quantity;
                await _productRepo.UpdateAsync(product);
            }
        }

        await _orderRepo.UpdateAsync(order);

        return NoContent();
    }

    // DELETE: api/purchaseorders/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _orderRepo.GetByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        // Note: we are not rolling back inventory here if already Received.

        await _orderRepo.DeleteAsync(order);

        return NoContent();
    }
}