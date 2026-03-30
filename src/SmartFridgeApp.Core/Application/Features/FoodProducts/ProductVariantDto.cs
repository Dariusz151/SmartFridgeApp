namespace SmartFridgeApp.Core.Application.Features;

public class ProductVariantDto
{
    public int VariantId { get; set; }
    public short FoodProductId { get; set; }
    public string Name { get; set; }
    public string? Barcode { get; set; }
}
