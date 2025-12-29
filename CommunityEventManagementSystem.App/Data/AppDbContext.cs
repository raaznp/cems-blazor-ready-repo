using CommunityEventManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunityEventManagementSystem.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Participant> Participants => Set<Participant>();
    public DbSet<Venue> Venues => Set<Venue>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<EventActivity> EventActivities => Set<EventActivity>();
    public DbSet<EventRegistration> EventRegistrations => Set<EventRegistration>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Venue 1-M Event
        modelBuilder.Entity<Event>()
            .HasOne(e => e.Venue)
            .WithMany(v => v.Events)
            .HasForeignKey(e => e.VenueId)
            .OnDelete(DeleteBehavior.Restrict);

        // Event M-M Activity via EventActivity
        modelBuilder.Entity<EventActivity>()
            .HasKey(x => new { x.EventId, x.ActivityId });

        modelBuilder.Entity<EventActivity>()
            .HasOne(x => x.Event)
            .WithMany(e => e.EventActivities)
            .HasForeignKey(x => x.EventId);

        modelBuilder.Entity<EventActivity>()
            .HasOne(x => x.Activity)
            .WithMany(a => a.EventActivities)
            .HasForeignKey(x => x.ActivityId);

        // Participant M-M Event via EventRegistration (extra fields)
        modelBuilder.Entity<EventRegistration>()
            .HasIndex(r => new { r.EventId, r.ParticipantId, r.RequestType, r.Status })
            .HasDatabaseName("IX_Reg_EventParticipant_Type_Status");

        modelBuilder.Entity<EventRegistration>()
            .HasOne(r => r.Event)
            .WithMany(e => e.Registrations)
            .HasForeignKey(r => r.EventId);

        modelBuilder.Entity<EventRegistration>()
            .HasOne(r => r.Participant)
            .WithMany(p => p.Registrations)
            .HasForeignKey(r => r.ParticipantId);

        // Simple constraints
        modelBuilder.Entity<Participant>()
            .HasIndex(p => p.Email)
            .IsUnique();

        modelBuilder.Entity<Event>()
            .Property(e => e.Capacity)
            .HasDefaultValue(20);

        // Seed data is handled by SeedData.EnsureSeeded(db) at runtime,
        // but we still keep model config here.
    }
}
