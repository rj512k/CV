using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ERP.App.Validation;
using ERP.Data;
using ERP.Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ERP.App.ViewModels;

public class CustomersViewModel : BaseViewModel
{
    private readonly ErpDbContext _dbContext;
    private readonly CustomerValidator _validator = new();

    public CustomersViewModel(ErpDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ObservableCollection<Customer> Customers { get; } = new();

    public async Task LoadAsync()
    {
        Customers.Clear();
        var items = await _dbContext.Customers.AsNoTracking().OrderBy(c => c.Name).ToListAsync();
        foreach (var c in items) Customers.Add(c);
    }

    public async Task AddAsync(Customer customer)
    {
        _validator.ValidateAndThrow(customer);
        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync();
        await LoadAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        _validator.ValidateAndThrow(customer);
        _dbContext.Customers.Update(customer);
        await _dbContext.SaveChangesAsync();
        await LoadAsync();
    }

    public async Task DeleteAsync(Customer customer)
    {
        _dbContext.Customers.Remove(customer);
        await _dbContext.SaveChangesAsync();
        await LoadAsync();
    }
}