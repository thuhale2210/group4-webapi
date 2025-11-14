using AutoMapper;
using FreshSourceAPI.DTOs;
using FreshSourceAPI.Entities;
using FreshSourceAPI.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FreshSourceAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IGenericRepository<Product> _productRepo;
    private readonly IMapper _mapper;

    public ProductsController(IGenericRepository<Product> productRepo, IMapper mapper)
    {
        _productRepo = productRepo;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetProducts()
    {
        var products = await _productRepo.GetAllAsync();
        var result = _mapper.Map<IEnumerable<ProductReadDto>>(products);
        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetProductById")]
    public async Task<ActionResult<ProductReadDto>> GetProductById(int id)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        var result = _mapper.Map<ProductReadDto>(product);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProductReadDto>> CreateProduct(ProductCreateDto createDto)
    {
        var product = _mapper.Map<Product>(createDto);

        await _productRepo.AddAsync(product);

        var readDto = _mapper.Map<ProductReadDto>(product);

        return CreatedAtAction(
            nameof(GetProductById),
            new { id = product.Id },
            readDto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, ProductUpdateDto updateDto)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        // Map new values onto existing entity
        _mapper.Map(updateDto, product);

        await _productRepo.UpdateAsync(product);

        return NoContent();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> PartiallyUpdateProduct(
    int id,
    JsonPatchDocument<ProductUpdateDto> patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest();
        }

        var product = await _productRepo.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        // Map entity to an updatable DTO
        var productToPatch = _mapper.Map<ProductUpdateDto>(product);

        // Apply patch operations to DTO
        patchDoc.ApplyTo(productToPatch, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Map patched DTO back to entity
        _mapper.Map(productToPatch, product);

        await _productRepo.UpdateAsync(product);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        await _productRepo.DeleteAsync(product);

        return NoContent();
    }
}
