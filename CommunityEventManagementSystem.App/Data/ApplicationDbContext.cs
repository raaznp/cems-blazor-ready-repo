using CommunityEventManagementSystem.App.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunityEventManagementSystem.App.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Event> Events => Set<Event>();
}
