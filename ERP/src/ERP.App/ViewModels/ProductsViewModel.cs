using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ERP.App.Validation;
using ERP.Data;
using ERP.Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ERP.App.ViewModels;

public class ProductsViewModel : BaseViewModel
{
    private readonly ErpDbContext _dbContext;
    private readonly ProductValidator _validator = new();

    public ProductsViewModel(ErpDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ObservableCollection<Product> Products { get; } = new();

    public async Task LoadAsync()
    {
        Products.Clear();
        var items = await _dbContext.Products.AsNoTracking().OrderBy(p => p.Name).ToListAsync();
        foreach (var p in items) Products.Add(p);
    }

    public async Task AddAsync(Product product)
    {
        _validator.ValidateAndThrow(product);
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
        await LoadAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _validator.ValidateAndThrow(product);
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync();
        await LoadAsync();
    }

    public async Task DeleteAsync(Product product)
    {
        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
        await LoadAsync();
    }
}