using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ERP.Data;
using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.App.ViewModels;

public class ReportsViewModel : BaseViewModel
{
    private readonly ErpDbContext _dbContext;

    public ReportsViewModel(ErpDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ObservableCollection<Sale> Sales { get; } = new();
    public ObservableCollection<Product> Inventory { get; } = new();

    public async Task LoadAsync(DateTime? fromUtc = null, DateTime? toUtc = null)
    {
        Sales.Clear();
        var q = _dbContext.Sales
            .Include(s => s.Customer)
            .Include(s => s.Items)
            .ThenInclude(i => i.Product)
            .AsNoTracking()
            .OrderByDescending(s => s.DateUtc)
            .AsQueryable();
        if (fromUtc.HasValue)
        {
            q = q.Where(s => s.DateUtc >= fromUtc.Value);
        }
        if (toUtc.HasValue)
        {
            q = q.Where(s => s.DateUtc <= toUtc.Value);
        }
        foreach (var s in await q.Take(500).ToListAsync()) Sales.Add(s);

        Inventory.Clear();
        foreach (var p in await _dbContext.Products.AsNoTracking().OrderBy(p => p.Name).ToListAsync()) Inventory.Add(p);
    }
}