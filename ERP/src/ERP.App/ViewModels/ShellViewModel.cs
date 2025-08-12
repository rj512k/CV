using ERP.App.Services;

namespace ERP.App.ViewModels;

public class ShellViewModel : BaseViewModel
{
    private readonly IAuthService _authService;

    public ShellViewModel(IAuthService authService,
        LoginViewModel loginViewModel,
        ProductsViewModel productsViewModel,
        CustomersViewModel customersViewModel,
        SalesViewModel salesViewModel,
        ReportsViewModel reportsViewModel)
    {
        _authService = authService;
        LoginViewModel = loginViewModel;
        ProductsViewModel = productsViewModel;
        CustomersViewModel = customersViewModel;
        SalesViewModel = salesViewModel;
        ReportsViewModel = reportsViewModel;

        CurrentViewModel = LoginViewModel;
    }

    public LoginViewModel LoginViewModel { get; }
    public ProductsViewModel ProductsViewModel { get; }
    public CustomersViewModel CustomersViewModel { get; }
    public SalesViewModel SalesViewModel { get; }
    public ReportsViewModel ReportsViewModel { get; }

    private BaseViewModel _currentViewModel;
    public BaseViewModel CurrentViewModel
    {
        get => _currentViewModel;
        set { _currentViewModel = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(IsLoggedIn)); }
    }

    public bool IsLoggedIn => _authService.CurrentUser != null;

    public void ShowLogin()
    {
        CurrentViewModel = LoginViewModel;
    }

    public void ShowMain()
    {
        CurrentViewModel = ProductsViewModel;
    }

    public void NavigateToProducts() => CurrentViewModel = ProductsViewModel;
    public void NavigateToCustomers() => CurrentViewModel = CustomersViewModel;
    public void NavigateToSales() => CurrentViewModel = SalesViewModel;
    public void NavigateToReports() => CurrentViewModel = ReportsViewModel;

    public void Logout()
    {
        _authService.SignOut();
        ShowLogin();
    }
}