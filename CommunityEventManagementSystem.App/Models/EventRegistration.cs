using System.ComponentModel.DataAnnotations;

namespace CommunityEventManagementSystem.Models;

public sealed class EventRegistration
{
    public int Id { get; set; }

    public int EventId { get; set; }
    public Event? Event { get; set; }

    public int ParticipantId { get; set; }
    public Participant? Participant { get; set; }

    public RequestType RequestType { get; set; } = RequestType.Registration;
    public RequestStatus Status { get; set; } = RequestStatus.Pending;

    [Required]
    public DateTime RequestedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? ProcessedAtUtc { get; set; }

    [StringLength(500)]
    public string AdminNotes { get; set; } = string.Empty;
}
