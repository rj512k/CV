using System.Linq;
using System.Threading.Tasks;
using ERP.Data;
using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.App.Services;

public interface IAuthService
{
    Task<User?> SignInAsync(string username, string password);
    void SignOut();
    User? CurrentUser { get; }
    bool HasRole(params UserRole[] roles);
}

public class AuthService : IAuthService
{
    private readonly ErpDbContext _dbContext;

    public AuthService(ErpDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public User? CurrentUser { get; private set; }

    public async Task<User?> SignInAsync(string username, string password)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username && u.IsActive);
        if (user == null)
        {
            return null;
        }

        var ok = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if (!ok)
        {
            return null;
        }

        CurrentUser = user;
        return user;
    }

    public void SignOut()
    {
        CurrentUser = null;
    }

    public bool HasRole(params UserRole[] roles)
    {
        if (CurrentUser == null)
        {
            return false;
        }
        return roles.Contains(CurrentUser.Role);
    }
}