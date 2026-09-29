using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using projetoAPI.Models;
using projetoAPI.DTOs;
using projetoAPI.Services.Interfaces;

namespace projetoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        return Ok(await _productService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    [Authorize(Roles = "vendedor")]
    public async Task<IActionResult> PostProduct([FromBody] ProductCreateDTO newProductDto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var product = new Product
        {
            Name = newProductDto.Name,
            Description = newProductDto.Description,
            CategoryId = newProductDto.CategoryId,
            Price = newProductDto.Price,
            Stock = newProductDto.Stock,
            IsActive = newProductDto.IsActive,
            UserId = userId
        };

        var createdProduct = await _productService.CreateAsync(product);
        return Created("", createdProduct);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "vendedor")]
    public async Task<IActionResult> PutProduct(int id, [FromBody] ProductCreateDTO updatedProductDto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var existingProduct = await _productService.GetByIdAsync(id);
        if (existingProduct == null)
            return NotFound();

        if (existingProduct.UserId != userId)
            return Forbid();

        var product = new Product
        {
            Name = updatedProductDto.Name,
            Description = updatedProductDto.Description,
            CategoryId = updatedProductDto.CategoryId,
            Price = updatedProductDto.Price,
            Stock = updatedProductDto.Stock,
            IsActive = updatedProductDto.IsActive,
            UserId = userId
        };

        var updated = await _productService.UpdateAsync(id, product);
        return Ok(updated);
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "vendedor")]
    public async Task<IActionResult> PatchProduct(int id, [FromBody] ProductCreateDTO updatedProductDto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var existingProduct = await _productService.GetByIdAsync(id);
        if (existingProduct == null)
            return NotFound();

        if (existingProduct.UserId != userId)
            return Forbid();

        var product = new Product
        {
            Name = updatedProductDto.Name,
            Description = updatedProductDto.Description,
            CategoryId = updatedProductDto.CategoryId,
            Price = updatedProductDto.Price,
            Stock = updatedProductDto.Stock,
            IsActive = updatedProductDto.IsActive,
            UserId = userId
        };

        var updated = await _productService.PatchAsync(id, product);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "vendedor")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        if (product.UserId != userId)
            return Forbid();

        var deleted = await _productService.DeleteAsync(id);
        return NoContent();
    }
}
