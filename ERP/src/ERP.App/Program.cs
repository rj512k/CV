using Avalonia;
using ERP.App.Services;
using ERP.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ERP.App;

class Program
{
    private static ServiceProvider? _serviceProvider;

    [STAThread]
    public static void Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddDbContext<ErpDbContext>(o => o.UseSqlite("Data Source=erp.db"));
        services.AddScoped<IAuthService, AuthService>();

        // ViewModels
        services.AddScoped<ViewModels.LoginViewModel>();
        services.AddScoped<ViewModels.ProductsViewModel>();
        services.AddScoped<ViewModels.CustomersViewModel>();
        services.AddScoped<ViewModels.SalesViewModel>();
        services.AddScoped<ViewModels.ReportsViewModel>();
        services.AddScoped<ViewModels.ShellViewModel>();

        _serviceProvider = services.BuildServiceProvider();

        using (var scope = _serviceProvider.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ErpDbContext>();
            DbInitializer.InitializeAsync(db).GetAwaiter().GetResult();
        }

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure(() => new App(_serviceProvider!))
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
