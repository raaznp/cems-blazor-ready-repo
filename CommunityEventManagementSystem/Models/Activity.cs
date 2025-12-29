using System.ComponentModel.DataAnnotations;

namespace CommunityEventManagementSystem.Models;

public sealed class Activity
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    public List<EventActivity> EventActivities { get; set; } = new();
}
