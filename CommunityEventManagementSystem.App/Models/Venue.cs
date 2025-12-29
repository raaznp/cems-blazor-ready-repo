using System.ComponentModel.DataAnnotations;

namespace CommunityEventManagementSystem.Models;

public sealed class Venue
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(250)]
    public string Address { get; set; } = string.Empty;

    [Range(1, 100000)]
    public int Capacity { get; set; } = 100;

    public List<Event> Events { get; set; } = new();
}
