using System;

namespace SmartFridgeApp.Core.Domain.ShoppingList;

public class ShoppingItem(Guid id, string name, string addedByEmail, DateTimeOffset addedAt)
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
    public string AddedByEmail { get; } = addedByEmail;
    public DateTimeOffset AddedAt { get; } = addedAt;
}
