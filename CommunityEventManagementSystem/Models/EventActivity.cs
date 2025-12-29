namespace CommunityEventManagementSystem.Models;

public sealed class EventActivity
{
    public int EventId { get; set; }
    public Event? Event { get; set; }

    public int ActivityId { get; set; }
    public Activity? Activity { get; set; }
}
