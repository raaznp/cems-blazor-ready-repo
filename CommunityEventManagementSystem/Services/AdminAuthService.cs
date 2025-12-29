using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CommunityEventManagementSystem.Services;

public sealed class AdminAuthService
{
    private readonly IConfiguration _config;

    public AdminAuthService(IConfiguration config)
    {
        _config = config;
    }

    public bool ValidateAdminCredentials(string username, string password)
    {
        var expectedUsername = _config["AdminCredentials:Username"] ?? "admin";
        var expectedPassword = _config["AdminCredentials:Password"] ?? "admin";

        return string.Equals(username?.Trim(), expectedUsername, StringComparison.OrdinalIgnoreCase)
            && string.Equals(password?.Trim(), expectedPassword, StringComparison.Ordinal);
    }

    public async Task SignInAdminAsync(HttpContext httpContext)
    {
        var claims = new List<Claim>
        {
            new Claim("IsAdmin", "true"),
            new Claim(ClaimTypes.Name, "Admin")
        };

        var identity = new ClaimsIdentity(claims, "AdminCookie");
        var principal = new ClaimsPrincipal(identity);

        await httpContext.SignInAsync("AdminCookie", principal);
    }

    public async Task SignOutAdminAsync(HttpContext httpContext)
    {
        await httpContext.SignOutAsync("AdminCookie");
    }
}
