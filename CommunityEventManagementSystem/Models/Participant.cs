using System.ComponentModel.DataAnnotations;

namespace CommunityEventManagementSystem.Models;

public sealed class Participant
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(30)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    public DateOnly DateOfBirth { get; set; }

    public List<EventRegistration> Registrations { get; set; } = new();
}
