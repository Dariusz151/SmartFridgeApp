using System;

namespace SmartFridgeApp.Core.Application.Features.ShoppingList;

public class ShoppingListItemDto
{
    public Guid Id { get; set; }
    public Guid KitchenId { get; set; }
    public string Name { get; set; }
    public string AddedByEmail { get; set; }
    public DateTimeOffset AddedAt { get; set; }
}

public record AddShoppingListItemRequest(string Name);
