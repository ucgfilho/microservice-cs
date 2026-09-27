using Microsoft.EntityFrameworkCore;
using projetoAPI.Data;
using projetoAPI.Models;
using projetoAPI.Services.Interfaces;

namespace projetoAPI.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task<Category> CreateAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category?> UpdateAsync(int id, Category updatedCategory)
    {
        var existing = await _context.Categories.FindAsync(id);
        if (existing == null)
            return null;

        existing.Name = updatedCategory.Name;
        existing.Description = updatedCategory.Description;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<Category?> PatchAsync(int id, Category updatedCategory)
    {
        var existing = await _context.Categories.FindAsync(id);
        if (existing == null)
            return null;

        if (!string.IsNullOrEmpty(updatedCategory.Name))
            existing.Name = updatedCategory.Name;

        if (updatedCategory.Description != null)
            existing.Description = updatedCategory.Description;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Categories.FindAsync(id);
        if (existing == null)
            return false;

        _context.Categories.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
