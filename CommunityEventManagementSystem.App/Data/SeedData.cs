using CommunityEventManagementSystem.Models;

namespace CommunityEventManagementSystem.Data;

public static class SeedData
{
    public static void EnsureSeeded(AppDbContext db)
    {
        // ✅ Option B: Seed ONLY ONCE (never re-seed even if tables become empty later)
        if (db.Venues.Any() || db.Activities.Any() || db.Events.Any() || db.Participants.Any() || db.EventRegistrations.Any())
            return;

        // Seed Venues
        db.Venues.AddRange(
            new Venue { Name = "City Hall", Address = "1 Main Street", Capacity = 200 },
            new Venue { Name = "Community Centre", Address = "25 Riverside Road", Capacity = 120 }
        );
        db.SaveChanges();

        // Seed Activities
        db.Activities.AddRange(
            new Activity { Name = "Workshop", Description = "Skills workshop session." },
            new Activity { Name = "Networking", Description = "Meet & connect with others." },
            new Activity { Name = "Talk", Description = "Guest speaker and Q&A." }
        );
        db.SaveChanges();

        var venues = db.Venues.ToList();
        var activities = db.Activities.ToList();

        // Seed Events
        var e1 = new Event
        {
            Title = "Local Tech Meetup",
            Description = "Community technology meetup with talks and networking.",
            StartDateTime = DateTime.UtcNow.AddDays(7).AddHours(18),
            EndDateTime = DateTime.UtcNow.AddDays(7).AddHours(20),
            Capacity = 30,
            VenueId = venues[0].Id
        };

        var e2 = new Event
        {
            Title = "Wellbeing Workshop",
            Description = "Workshop focused on wellbeing and community support.",
            StartDateTime = DateTime.UtcNow.AddDays(14).AddHours(10),
            EndDateTime = DateTime.UtcNow.AddDays(14).AddHours(12),
            Capacity = 25,
            VenueId = venues[1].Id
        };

        db.Events.AddRange(e1, e2);
        db.SaveChanges();

        // Attach activities to events
        db.EventActivities.AddRange(
            new EventActivity { EventId = e1.Id, ActivityId = activities.First(a => a.Name == "Talk").Id },
            new EventActivity { EventId = e1.Id, ActivityId = activities.First(a => a.Name == "Networking").Id },
            new EventActivity { EventId = e2.Id, ActivityId = activities.First(a => a.Name == "Workshop").Id }
        );
        db.SaveChanges();

        // Seed Participants
        db.Participants.AddRange(
            new Participant
            {
                FullName = "Amy Alpha",
                Email = "amy.alpha@example.com",
                PhoneNumber = "07111111111",
                DateOfBirth = new DateOnly(2000, 1, 1)
            },
            new Participant
            {
                FullName = "Billy Bravo",
                Email = "billy.bravo@example.com",
                PhoneNumber = "07222222222",
                DateOfBirth = new DateOnly(1999, 5, 12)
            }
        );
        db.SaveChanges();
    }
}
