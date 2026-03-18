using System;
using SmartFridgeApp.Core.Exceptions;

namespace SmartFridgeApp.Core.Domain.Entities;

public class FridgeMember
{
    public int Id { get; private set; }
    public Guid FridgeId { get; private set; }
    public string Email { get; private set; }
    public string MemberRole { get; private set; }
    public string Status { get; private set; }
    public string Color { get; private set; }
    public DateTime InvitedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private FridgeMember() { }

    public static FridgeMember CreateCreator(Guid fridgeId, string email, string color) =>
        new()
        {
            FridgeId = fridgeId,
            Email = email,
            MemberRole = "Creator",
            Status = "Accepted",
            Color = color,
            InvitedAt = DateTime.UtcNow
        };

    public static FridgeMember CreateInvited(Guid fridgeId, string email, string color) =>
        new()
        {
            FridgeId = fridgeId,
            Email = email,
            MemberRole = "Member",
            Status = "Pending",
            Color = color,
            InvitedAt = DateTime.UtcNow
        };

    public void Accept()
    {
        if (Status != "Pending")
            throw new DomainException("Invite is not in pending state.", "InviteNotPending");
        Status = "Accepted";
        UpdatedAt = DateTime.UtcNow;
    }

    public void Decline()
    {
        if (Status != "Pending")
            throw new DomainException("Invite is not in pending state.", "InviteNotPending");
        Status = "Declined";
        UpdatedAt = DateTime.UtcNow;
    }
}
