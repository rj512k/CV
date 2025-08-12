using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ERP.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.App;

public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;

    public App(ServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var shellVm = _serviceProvider.GetRequiredService<ShellViewModel>();
            shellVm.ShowLogin();

            var main = new MainWindow
            {
                DataContext = shellVm
            };
            desktop.MainWindow = main;
        }

        base.OnFrameworkInitializationCompleted();
    }
}