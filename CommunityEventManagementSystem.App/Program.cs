using CommunityEventManagementSystem.Data;
using CommunityEventManagementSystem.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace CommunityEventManagementSystem;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpContextAccessor();

        // Razor Components (Blazor Web App) - Interactive Server
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        // EF Core - SQLite (use a writable absolute path)
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            var baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appFolder = Path.Combine(baseFolder, "CommunityEventManagementSystem");
            Directory.CreateDirectory(appFolder);

            var dbPath = Path.Combine(appFolder, "communityevents.db");
            var connectionString = $"Data Source={dbPath}";

            options.UseSqlite(connectionString);

            Console.WriteLine($"SQLite database path: {dbPath}");
        });

        // Cookie Auth (hardcoded admin login)
        builder.Services.AddAuthentication("AdminCookie")
            .AddCookie("AdminCookie", options =>
            {
                options.Cookie.Name = "CommunityEventManagementSystem.Admin";
                options.LoginPath = "/admin/login";
                options.AccessDeniedPath = "/admin/denied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
            });

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("AdminOnly", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("IsAdmin", "true");
            });

        // Services
        builder.Services.AddScoped<AdminAuthService>();
        builder.Services.AddScoped<EventService>();
        builder.Services.AddScoped<VenueService>();
        builder.Services.AddScoped<ActivityService>();
        builder.Services.AddScoped<ParticipantService>();
        builder.Services.AddScoped<RegistrationService>();

        var app = builder.Build();

        // Migrate + seed
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
            // SeedData.EnsureSeeded(db);
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAntiforgery();

        // ===== Admin Login (POST endpoint sets cookie BEFORE response starts) =====
        app.MapPost("/admin/login-post", async (HttpContext http, AdminAuthService auth) =>
        {
            var form = await http.Request.ReadFormAsync();
            var username = (form["username"].ToString() ?? "").Trim();
            var password = (form["password"].ToString() ?? "").Trim();

            if (!auth.ValidateAdminCredentials(username, password))
            {
                http.Response.Redirect("/admin/login?error=1");
                return;
            }

            await auth.SignInAdminAsync(http);
            http.Response.Redirect("/admin");
        }).DisableAntiforgery();

        // ===== Admin Logout (POST endpoint clears cookie safely) =====
        app.MapPost("/admin/logout-post", async (HttpContext http, AdminAuthService auth) =>
        {
            await auth.SignOutAdminAsync(http);
            http.Response.Redirect("/");
        }).DisableAntiforgery();

        app.MapRazorComponents<Components.App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
