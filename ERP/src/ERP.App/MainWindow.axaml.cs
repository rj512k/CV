using Avalonia.Controls;
using Avalonia.Interactivity;
using ERP.App.ViewModels;

namespace ERP.App;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.FindControl<Button>("ProductsNav").Click += (_, __) => NavigateToProducts();
        this.FindControl<Button>("CustomersNav").Click += (_, __) => NavigateToCustomers();
        this.FindControl<Button>("SalesNav").Click += (_, __) => NavigateToSales();
        this.FindControl<Button>("ReportsNav").Click += (_, __) => NavigateToReports();
        this.FindControl<Button>("LogoutBtn").Click += (_, __) => Logout();
    }

    private async void NavigateToProducts()
    {
        if (DataContext is ShellViewModel shell)
        {
            shell.NavigateToProducts();
            await shell.ProductsViewModel.LoadAsync();
        }
    }

    private async void NavigateToCustomers()
    {
        if (DataContext is ShellViewModel shell)
        {
            shell.NavigateToCustomers();
            await shell.CustomersViewModel.LoadAsync();
        }
    }

    private async void NavigateToSales()
    {
        if (DataContext is ShellViewModel shell)
        {
            shell.NavigateToSales();
            await shell.SalesViewModel.LoadAsync();
        }
    }

    private async void NavigateToReports()
    {
        if (DataContext is ShellViewModel shell)
        {
            shell.NavigateToReports();
            await shell.ReportsViewModel.LoadAsync();
        }
    }

    private void Logout()
    {
        if (DataContext is ShellViewModel shell)
        {
            shell.Logout();
        }
    }
}