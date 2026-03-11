using System;

namespace SmartFridgeApp.Core.Application.Features;

public class FridgeScoreDto
{
    public Guid FridgeId { get; set; }
    public int WasteScore { get; set; }
    public string Rank { get; set; }
}
