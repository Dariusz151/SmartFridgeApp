using System.Collections.Generic;

namespace SmartFridgeApp.Core.Application.Features;

public class MonthlyWasteReportDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int TotalItemsWasted { get; set; }
    public List<WasteReportItemDto> Items { get; set; } = [];
}
