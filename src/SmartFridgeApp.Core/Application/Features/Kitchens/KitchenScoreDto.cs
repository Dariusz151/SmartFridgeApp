using System;

namespace SmartFridgeApp.Core.Application.Features;

public class KitchenScoreDto
{
    public Guid KitchenId { get; set; }
    public int WasteScore { get; set; }
    public string Rank { get; set; }
}
