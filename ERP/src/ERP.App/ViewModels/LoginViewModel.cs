using System.Threading.Tasks;
using ERP.App.Services;
using ERP.Domain.Entities;

namespace ERP.App.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly IAuthService _authService;

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    private string _username = string.Empty;
    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            RaisePropertyChanged();
        }
    }

    private string _password = string.Empty;
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            RaisePropertyChanged();
        }
    }

    private string _errorMessage = string.Empty;
    public string ErrorMessage
    {
        get => _errorMessage;
        private set
        {
            _errorMessage = value;
            RaisePropertyChanged();
        }
    }

    public async Task<User?> SignInAsync()
    {
        ErrorMessage = string.Empty;
        var user = await _authService.SignInAsync(Username.Trim(), Password);
        if (user == null)
        {
            ErrorMessage = "Invalid credentials";
        }
        return user;
    }
}