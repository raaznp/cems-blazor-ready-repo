using CommunityEventManagementSystem.Data;
using CommunityEventManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunityEventManagementSystem.Services;

public sealed class ParticipantService
{
    private readonly AppDbContext _db;
    public ParticipantService(AppDbContext db) => _db = db;

    public Task<List<Participant>> GetAllAsync() =>
        _db.Participants.OrderBy(p => p.FullName).ToListAsync();

    public Task<Participant?> GetByIdAsync(int id) =>
        _db.Participants.FirstOrDefaultAsync(p => p.Id == id);

    public Task<Participant?> FindByEmailAsync(string email) =>
        _db.Participants.FirstOrDefaultAsync(p => p.Email == email);

    public Task<List<Participant>> SearchAsync(string search)
    {
        search = (search ?? "").Trim().ToLowerInvariant();
        return _db.Participants
            .Where(p => p.FullName.ToLower().Contains(search) || p.Email.ToLower().Contains(search))
            .OrderBy(p => p.FullName)
            .ToListAsync();
    }

    public async Task<Participant> AddAsync(Participant participant)
    {
        // Unique Email enforced by DB index
        _db.Participants.Add(participant);
        await _db.SaveChangesAsync();
        return participant;
    }

    public async Task UpdateAsync(Participant participant)
    {
        _db.Participants.Update(participant);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var p = await _db.Participants.FirstOrDefaultAsync(x => x.Id == id);
        if (p is null) return false;

        var hasRegs = await _db.EventRegistrations.AnyAsync(r => r.ParticipantId == id);
        if (hasRegs) return false;

        _db.Participants.Remove(p);
        await _db.SaveChangesAsync();
        return true;
    }
}
