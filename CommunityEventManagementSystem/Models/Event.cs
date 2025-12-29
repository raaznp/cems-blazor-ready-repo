using System.ComponentModel.DataAnnotations;

namespace CommunityEventManagementSystem.Models;

public sealed class Event
{
    public int Id { get; set; }

    [Required, StringLength(140)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(1200)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime StartDateTime { get; set; }

    [Required]
    public DateTime EndDateTime { get; set; }

    [Range(1, 100000)]
    public int Capacity { get; set; } = 20;

    public int VenueId { get; set; }
    public Venue? Venue { get; set; }

    public List<EventActivity> EventActivities { get; set; } = new();
    public List<EventRegistration> Registrations { get; set; } = new();
}
