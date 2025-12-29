using CommunityEventManagementSystem.Data;
using CommunityEventManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunityEventManagementSystem.Services;

public sealed class VenueService
{
    private readonly AppDbContext _db;

    public VenueService(AppDbContext db) => _db = db;

    public Task<List<Venue>> GetAllAsync() =>
        _db.Venues.OrderBy(v => v.Name).ToListAsync();

    public Task<Venue?> GetByIdAsync(int id) =>
        _db.Venues.FirstOrDefaultAsync(v => v.Id == id);

    public async Task<Venue> AddAsync(Venue venue)
    {
        _db.Venues.Add(venue);
        await _db.SaveChangesAsync();
        return venue;
    }

    public async Task UpdateAsync(Venue venue)
    {
        _db.Venues.Update(venue);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var venue = await _db.Venues.FirstOrDefaultAsync(v => v.Id == id);
        if (venue is null) return false;

        var hasEvents = await _db.Events.AnyAsync(e => e.VenueId == id);
        if (hasEvents) return false;

        _db.Venues.Remove(venue);
        await _db.SaveChangesAsync();
        return true;
    }
}
