using CommunityEventManagementSystem.Data;
using CommunityEventManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunityEventManagementSystem.Services;

public sealed class RegistrationService
{
    private readonly AppDbContext _db;

    public RegistrationService(AppDbContext db) => _db = db;

    public async Task<bool> IsParticipantApprovedForEventAsync(int eventId, int participantId)
    {
        return await _db.EventRegistrations.AnyAsync(r =>
            r.EventId == eventId &&
            r.ParticipantId == participantId &&
            r.RequestType == RequestType.Registration &&
            r.Status == RequestStatus.Approved);
    }

    public async Task<bool> HasPendingRequestAsync(int eventId, int participantId)
    {
        return await _db.EventRegistrations.AnyAsync(r =>
            r.EventId == eventId &&
            r.ParticipantId == participantId &&
            r.Status == RequestStatus.Pending);
    }

    public async Task<int> GetApprovedCountAsync(int eventId)
    {
        return await _db.EventRegistrations.CountAsync(r =>
            r.EventId == eventId &&
            r.RequestType == RequestType.Registration &&
            r.Status == RequestStatus.Approved);
    }

    public async Task<EventRegistration> CreateRegistrationRequestAsync(int eventId, Participant participant)
    {
        // Prevent duplicates / spam
        if (await HasPendingRequestAsync(eventId, participant.Id))
            throw new InvalidOperationException("A request is already pending for this participant and event.");

        if (await IsParticipantApprovedForEventAsync(eventId, participant.Id))
            throw new InvalidOperationException("Participant is already approved for this event.");

        var req = new EventRegistration
        {
            EventId = eventId,
            ParticipantId = participant.Id,
            RequestType = RequestType.Registration,
            Status = RequestStatus.Pending,
            RequestedAtUtc = DateTime.UtcNow
        };

        _db.EventRegistrations.Add(req);
        await _db.SaveChangesAsync();
        return req;
    }

    public async Task<EventRegistration> CreateWithdrawalRequestAsync(int eventId, int participantId)
    {
        if (await HasPendingRequestAsync(eventId, participantId))
            throw new InvalidOperationException("A request is already pending for this participant and event.");

        var approved = await _db.EventRegistrations.FirstOrDefaultAsync(r =>
            r.EventId == eventId &&
            r.ParticipantId == participantId &&
            r.RequestType == RequestType.Registration &&
            r.Status == RequestStatus.Approved);

        if (approved is null)
            throw new InvalidOperationException("No approved registration exists to withdraw from.");

        var req = new EventRegistration
        {
            EventId = eventId,
            ParticipantId = participantId,
            RequestType = RequestType.Withdrawal,
            Status = RequestStatus.Pending,
            RequestedAtUtc = DateTime.UtcNow
        };

        _db.EventRegistrations.Add(req);
        await _db.SaveChangesAsync();
        return req;
    }

    public Task<List<EventRegistration>> GetPendingRequestsAsync() =>
        _db.EventRegistrations
            .Include(r => r.Event)
            .Include(r => r.Participant)
            .Where(r => r.Status == RequestStatus.Pending)
            .OrderBy(r => r.RequestedAtUtc)
            .ToListAsync();

    public async Task ApproveAsync(int requestId, string adminNotes = "")
    {
        var req = await _db.EventRegistrations
            .Include(r => r.Event)
            .FirstOrDefaultAsync(r => r.Id == requestId);

        if (req is null) throw new InvalidOperationException("Request not found.");
        if (req.Status != RequestStatus.Pending) throw new InvalidOperationException("Request already processed.");

        if (req.RequestType == RequestType.Registration)
        {
            var approvedCount = await GetApprovedCountAsync(req.EventId);

            var capacity = req.Event?.Capacity ?? 0;
            if (capacity <= 0) throw new InvalidOperationException("Event capacity is not configured.");
            if (approvedCount >= capacity) throw new InvalidOperationException("Event is full. Cannot approve registration.");
        }

        if (req.RequestType == RequestType.Withdrawal)
        {
            // On approval of withdrawal: delete the approved registration record
            var approvedReg = await _db.EventRegistrations.FirstOrDefaultAsync(r =>
                r.EventId == req.EventId &&
                r.ParticipantId == req.ParticipantId &&
                r.RequestType == RequestType.Registration &&
                r.Status == RequestStatus.Approved);

            if (approvedReg is null)
                throw new InvalidOperationException("Approved registration not found (cannot withdraw).");

            _db.EventRegistrations.Remove(approvedReg);
        }

        req.Status = RequestStatus.Approved;
        req.ProcessedAtUtc = DateTime.UtcNow;
        req.AdminNotes = adminNotes ?? "";

        await _db.SaveChangesAsync();
    }

    public async Task DeclineAsync(int requestId, string adminNotes = "")
    {
        var req = await _db.EventRegistrations.FirstOrDefaultAsync(r => r.Id == requestId);
        if (req is null) throw new InvalidOperationException("Request not found.");
        if (req.Status != RequestStatus.Pending) throw new InvalidOperationException("Request already processed.");

        req.Status = RequestStatus.Declined;
        req.ProcessedAtUtc = DateTime.UtcNow;
        req.AdminNotes = adminNotes ?? "";
        await _db.SaveChangesAsync();
    }
}
