using System;

namespace SmartFridgeApp.Infrastructure.ShoppingList;

public class ShoppingListItemDocument
{
    public Guid Id { get; set; }
    public Guid KitchenId { get; set; }
    public string Name { get; set; }
    public string AddedByEmail { get; set; }
    public DateTimeOffset AddedAt { get; set; }
}
