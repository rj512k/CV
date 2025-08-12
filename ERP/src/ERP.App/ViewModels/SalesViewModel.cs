using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ERP.Data;
using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.App.ViewModels;

public class SalesViewModel : BaseViewModel
{
    private readonly ErpDbContext _dbContext;

    public SalesViewModel(ErpDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ObservableCollection<Sale> Sales { get; } = new();

    public async Task LoadAsync()
    {
        Sales.Clear();
        var items = await _dbContext.Sales
            .Include(s => s.Customer)
            .Include(s => s.Items)
            .ThenInclude(i => i.Product)
            .AsNoTracking()
            .OrderByDescending(s => s.DateUtc)
            .Take(100)
            .ToListAsync();
        foreach (var s in items) Sales.Add(s);
    }

    public async Task<int> CreateSaleAsync(int customerId, int userId, IEnumerable<(int productId, int quantity)> lines)
    {
        var products = await _dbContext.Products.Where(p => lines.Select(l => l.productId).Contains(p.Id)).ToListAsync();
        var sale = new Sale
        {
            CustomerId = customerId,
            UserId = userId,
            DateUtc = DateTime.UtcNow,
        };
        foreach (var (productId, quantity) in lines)
        {
            var product = products.First(p => p.Id == productId);
            sale.Items.Add(new SaleItem
            {
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = product.Price
            });
            product.StockQuantity -= quantity;
        }
        sale.TotalAmount = sale.Items.Sum(i => i.UnitPrice * i.Quantity);
        _dbContext.Sales.Add(sale);
        await _dbContext.SaveChangesAsync();
        await LoadAsync();
        return sale.Id;
    }
}