using Avalonia.Controls;
using Avalonia.Interactivity;
using ERP.App.ViewModels;

namespace ERP.App.Views;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();
        if (this.FindControl<Button>("SignInButton") is { } btn)
        {
            btn.Click += OnSignInClicked;
        }
    }

    private async void OnSignInClicked(object? sender, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel vm)
        {
            await vm.SignInAsync();
        }
    }
}