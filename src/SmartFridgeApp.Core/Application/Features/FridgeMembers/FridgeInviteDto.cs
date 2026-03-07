using System;

namespace SmartFridgeApp.Core.Application.Features;

public class FridgeInviteDto
{
    public int Id { get; set; }
    public Guid FridgeId { get; set; }
    public string FridgeName { get; set; }
    public string InviterEmail { get; set; }
    public string InviterName { get; set; }
    public DateTime InvitedAt { get; set; }
}
