using System;

namespace SmartFridgeApp.Core.Application.Features;

public class FridgeMemberDto
{
    public int Id { get; set; }
    public Guid FridgeId { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string MemberRole { get; set; }
    public string Status { get; set; }
    public string Color { get; set; }
}
