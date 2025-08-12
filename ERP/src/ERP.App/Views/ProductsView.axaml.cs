using Avalonia.Controls;
using Avalonia.Interactivity;
using ERP.App.ViewModels;
using ERP.Domain.Entities;

namespace ERP.App.Views;

public partial class ProductsView : UserControl
{
    public ProductsView()
    {
        InitializeComponent();
        this.FindControl<Button>("AddBtn").Click += OnAddClicked;
        this.FindControl<Button>("RefreshBtn").Click += OnRefreshClicked;
    }

    private async void OnRefreshClicked(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ProductsViewModel vm)
        {
            await vm.LoadAsync();
        }
    }

    private async void OnAddClicked(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ProductsViewModel vm)
        {
            var name = this.FindControl<TextBox>("NameBox").Text ?? string.Empty;
            var sku = this.FindControl<TextBox>("SkuBox").Text ?? string.Empty;
            if (!decimal.TryParse(this.FindControl<TextBox>("PriceBox").Text, out var price)) price = 0m;
            if (!int.TryParse(this.FindControl<TextBox>("StockBox").Text, out var stock)) stock = 0;
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(sku)) return;
            await vm.AddAsync(new Product { Name = name.Trim(), Sku = sku.Trim(), Price = price, StockQuantity = stock });
        }
    }
}