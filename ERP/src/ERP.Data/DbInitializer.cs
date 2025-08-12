using BCrypt.Net;
using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(ErpDbContext dbContext)
    {
        await dbContext.Database.EnsureCreatedAsync();

        if (!await dbContext.Users.AnyAsync())
        {
            var admin = new User
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = UserRole.Admin,
                IsActive = true
            };
            await dbContext.Users.AddAsync(admin);
        }

        if (!await dbContext.Products.AnyAsync())
        {
            await dbContext.Products.AddRangeAsync(
                new Product { Name = "Sample Widget", Sku = "WID-001", Price = 19.99m, StockQuantity = 100 },
                new Product { Name = "Gadget", Sku = "GAD-001", Price = 29.99m, StockQuantity = 50 }
            );
        }

        if (!await dbContext.Customers.AnyAsync())
        {
            await dbContext.Customers.AddRangeAsync(
                new Customer { Name = "Acme Corp", Email = "info@acme.test", Phone = "+1-555-0100" },
                new Customer { Name = "John Doe", Email = "john@example.test" }
            );
        }

        await dbContext.SaveChangesAsync();
    }
}