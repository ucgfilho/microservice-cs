using Microsoft.EntityFrameworkCore;
using projetoAPI.Data;
using projetoAPI.Models;
using projetoAPI.Services.Interfaces;

namespace projetoAPI.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> UpdateAsync(int id, Product updatedProduct)
    {
        var existing = await _context.Products.FindAsync(id);
        if (existing == null)
            return null;

        existing.Name = updatedProduct.Name;
        existing.Description = updatedProduct.Description;
        existing.CategoryId = updatedProduct.CategoryId;
        existing.Price = updatedProduct.Price;
        existing.Stock = updatedProduct.Stock;
        existing.IsActive = updatedProduct.IsActive;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<Product?> PatchAsync(int id, Product updatedProduct)
    {
        var existing = await _context.Products.FindAsync(id);
        if (existing == null)
            return null;

        if (updatedProduct.Price > 0)
            existing.Price = updatedProduct.Price;

        if (!string.IsNullOrEmpty(updatedProduct.Name))
            existing.Name = updatedProduct.Name;

        if (updatedProduct.Description != null)
            existing.Description = updatedProduct.Description;

        if (updatedProduct.CategoryId != null)
            existing.CategoryId = updatedProduct.CategoryId;

        existing.Stock = updatedProduct.Stock;
        existing.IsActive = updatedProduct.IsActive;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Products.FindAsync(id);
        if (existing == null)
            return false;

        _context.Products.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
