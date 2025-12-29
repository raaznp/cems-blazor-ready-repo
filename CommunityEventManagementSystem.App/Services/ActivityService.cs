using CommunityEventManagementSystem.Data;
using CommunityEventManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunityEventManagementSystem.Services;

public sealed class ActivityService
{
    private readonly AppDbContext _db;
    public ActivityService(AppDbContext db) => _db = db;

    public Task<List<Activity>> GetAllAsync() =>
        _db.Activities.OrderBy(a => a.Name).ToListAsync();

    public Task<Activity?> GetByIdAsync(int id) =>
        _db.Activities.FirstOrDefaultAsync(a => a.Id == id);

    public async Task<Activity> AddAsync(Activity activity)
    {
        _db.Activities.Add(activity);
        await _db.SaveChangesAsync();
        return activity;
    }

    public async Task UpdateAsync(Activity activity)
    {
        _db.Activities.Update(activity);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var activity = await _db.Activities.FirstOrDefaultAsync(a => a.Id == id);
        if (activity is null) return false;

        var used = await _db.EventActivities.AnyAsync(x => x.ActivityId == id);
        if (used) return false;

        _db.Activities.Remove(activity);
        await _db.SaveChangesAsync();
        return true;
    }
}
