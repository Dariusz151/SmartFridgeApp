using System;

namespace SmartFridgeApp.Core.Application.Features;

public class KitchenInviteDto
{
    public int Id { get; set; }
    public Guid kitchenId { get; set; }
    public string kitchenName { get; set; }
    public string InviterEmail { get; set; }
    public string InviterName { get; set; }
    public DateTime InvitedAt { get; set; }
}
