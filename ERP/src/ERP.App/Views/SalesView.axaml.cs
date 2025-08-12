using Avalonia.Controls;
using Avalonia.Interactivity;
using ERP.App.ViewModels;

namespace ERP.App.Views;

public partial class SalesView : UserControl
{
    public SalesView()
    {
        InitializeComponent();
        this.FindControl<Button>("AddBtn").Click += OnAddClicked;
        this.FindControl<Button>("RefreshBtn").Click += OnRefreshClicked;
    }

    private async void OnRefreshClicked(object? sender, RoutedEventArgs e)
    {
        if (DataContext is SalesViewModel vm)
        {
            await vm.LoadAsync();
        }
    }

    private async void OnAddClicked(object? sender, RoutedEventArgs e)
    {
        if (DataContext is SalesViewModel vm)
        {
            if (!int.TryParse(this.FindControl<TextBox>("CustomerIdBox").Text, out var customerId)) return;
            if (!int.TryParse(this.FindControl<TextBox>("ProductIdBox").Text, out var productId)) return;
            if (!int.TryParse(this.FindControl<TextBox>("QtyBox").Text, out var qty)) qty = 1;
            await vm.CreateSaleAsync(customerId, 1, new[] { (productId, qty) });
        }
    }
}