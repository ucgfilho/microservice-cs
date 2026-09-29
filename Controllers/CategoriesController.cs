using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using projetoAPI.Models;
using projetoAPI.Services.Interfaces;

namespace projetoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        return Ok(await _categoryService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
            return NotFound();

        return Ok(category);
    }

    [HttpPost]
    [Authorize(Roles = "vendedor")]
    public async Task<IActionResult> PostCategory([FromBody] Category newCategory)
    {
        var category = await _categoryService.CreateAsync(newCategory);
        return Created("", category);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "vendedor")]
    public async Task<IActionResult> PutCategory(int id, [FromBody] Category updatedCategory)
    {
        var category = await _categoryService.UpdateAsync(id, updatedCategory);
        if (category == null)
            return NotFound();

        return Ok(category);
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "vendedor")]
    public async Task<IActionResult> PatchCategory(int id, [FromBody] Category updatedCategory)
    {
        var category = await _categoryService.PatchAsync(id, updatedCategory);
        if (category == null)
            return NotFound();

        return Ok(category);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "vendedor")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var deleted = await _categoryService.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
