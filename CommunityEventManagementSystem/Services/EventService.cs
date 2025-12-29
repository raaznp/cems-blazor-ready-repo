using CommunityEventManagementSystem.Data;
using CommunityEventManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunityEventManagementSystem.Services;

public sealed class EventService
{
    private readonly AppDbContext _db;
    public EventService(AppDbContext db) => _db = db;

    public Task<List<Event>> GetAllPublicAsync() =>
        _db.Events
            .Include(e => e.Venue)
            .Include(e => e.EventActivities).ThenInclude(x => x.Activity)
            .OrderBy(e => e.StartDateTime)
            .ToListAsync();

    public Task<Event?> GetByIdAsync(int id) =>
        _db.Events
            .Include(e => e.Venue)
            .Include(e => e.EventActivities).ThenInclude(x => x.Activity)
            .Include(e => e.Registrations).ThenInclude(r => r.Participant)
            .FirstOrDefaultAsync(e => e.Id == id);

    public async Task<ServiceResult> AddAsync(Event ev, List<int> activityIds)
    {
        var venue = await _db.Venues
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == ev.VenueId);

        if (venue is null)
            return ServiceResult.Fail("Selected venue does not exist.");

        if (ev.Capacity > venue.Capacity)
            return ServiceResult.Fail(
                $"Venue capacity is only {venue.Capacity}. Please set event capacity to {venue.Capacity} or less."
            );

        var newEvent = new Event
        {
            Title = ev.Title,
            Description = ev.Description,
            StartDateTime = ev.StartDateTime,
            EndDateTime = ev.EndDateTime,
            Capacity = ev.Capacity,
            VenueId = ev.VenueId
        };

        _db.Events.Add(newEvent);
        await _db.SaveChangesAsync();

        foreach (var aid in activityIds.Distinct())
            _db.EventActivities.Add(new EventActivity
            {
                EventId = newEvent.Id,
                ActivityId = aid
            });

        await _db.SaveChangesAsync();

        return ServiceResult.Success("Event created successfully.");
    }

    public async Task<ServiceResult> UpdateAsync(Event ev, List<int> activityIds)
    {
        var venue = await _db.Venues
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == ev.VenueId);

        if (venue is null)
            return ServiceResult.Fail("Selected venue does not exist.");

        if (ev.Capacity > venue.Capacity)
            return ServiceResult.Fail(
                $"Venue capacity is only {venue.Capacity}. Please set event capacity to {venue.Capacity} or less."
            );

        var existingEvent = await _db.Events.FirstOrDefaultAsync(x => x.Id == ev.Id);
        if (existingEvent is null)
            return ServiceResult.Fail("Event not found.");

        existingEvent.Title = ev.Title;
        existingEvent.Description = ev.Description;
        existingEvent.StartDateTime = ev.StartDateTime;
        existingEvent.EndDateTime = ev.EndDateTime;
        existingEvent.Capacity = ev.Capacity;
        existingEvent.VenueId = ev.VenueId;

        await _db.SaveChangesAsync();

        var existingLinks = await _db.EventActivities
            .Where(x => x.EventId == ev.Id)
            .ToListAsync();

        _db.EventActivities.RemoveRange(existingLinks);

        foreach (var aid in activityIds.Distinct())
            _db.EventActivities.Add(new EventActivity
            {
                EventId = ev.Id,
                ActivityId = aid
            });

        await _db.SaveChangesAsync();

        return ServiceResult.Success("Event updated successfully.");
    }



    /// <summary>
    /// Safe delete: only deletes the event if there are NO registrations/requests.
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        var ev = await _db.Events.FirstOrDefaultAsync(e => e.Id == id);
        if (ev is null) return false;

        var hasRegs = await _db.EventRegistrations.AnyAsync(r => r.EventId == id);
        if (hasRegs) return false;

        var links = await _db.EventActivities.Where(x => x.EventId == id).ToListAsync();
        _db.EventActivities.RemoveRange(links);

        _db.Events.Remove(ev);
        await _db.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Force delete: Admin-only behavior. Deletes registrations/requests AND activity links, then deletes the event.
    /// This allows deletion even if the event has registrations/requests.
    /// </summary>
    public async Task ForceDeleteAsync(int id)
    {
        var ev = await _db.Events
            .Include(e => e.EventActivities)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (ev is null) return;

        // Delete all registrations/requests for this event first (avoids FK issues)
        var regs = await _db.EventRegistrations.Where(r => r.EventId == id).ToListAsync();
        if (regs.Count > 0)
            _db.EventRegistrations.RemoveRange(regs);

        // Delete join rows (EventActivities)
        if (ev.EventActivities.Count > 0)
            _db.EventActivities.RemoveRange(ev.EventActivities);

        // Delete the event
        _db.Events.Remove(ev);

        await _db.SaveChangesAsync();
    }

    public async Task<int> GetApprovedAttendeeCountAsync(int eventId)
    {
        // Attendee count = Approved Registration requests
        // (Withdrawals remove the approved registration when approved)
        return await _db.EventRegistrations.CountAsync(r =>
            r.EventId == eventId &&
            r.RequestType == RequestType.Registration &&
            r.Status == RequestStatus.Approved);
    }
}
