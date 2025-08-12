using Avalonia.Controls;
using Avalonia.Interactivity;
using ERP.App.ViewModels;
using ERP.Domain.Entities;

namespace ERP.App.Views;

public partial class CustomersView : UserControl
{
    public CustomersView()
    {
        InitializeComponent();
        this.FindControl<Button>("AddBtn").Click += OnAddClicked;
        this.FindControl<Button>("RefreshBtn").Click += OnRefreshClicked;
    }

    private async void OnRefreshClicked(object? sender, RoutedEventArgs e)
    {
        if (DataContext is CustomersViewModel vm)
        {
            await vm.LoadAsync();
        }
    }

    private async void OnAddClicked(object? sender, RoutedEventArgs e)
    {
        if (DataContext is CustomersViewModel vm)
        {
            var name = this.FindControl<TextBox>("NameBox").Text ?? string.Empty;
            var email = this.FindControl<TextBox>("EmailBox").Text ?? string.Empty;
            var phone = this.FindControl<TextBox>("PhoneBox").Text ?? string.Empty;
            if (string.IsNullOrWhiteSpace(name)) return;
            await vm.AddAsync(new Customer { Name = name.Trim(), Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim(), Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim() });
        }
    }
}