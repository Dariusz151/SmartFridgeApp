using System;

namespace SmartFridgeApp.Core.Application.Features;

public class ShoppingStatusDto
{
    public Guid FridgeId { get; set; }
    public int ActiveItemCount { get; set; }
    public double AverageItemCount { get; set; }
    public bool IsShoppingNeeded { get; set; }
}
