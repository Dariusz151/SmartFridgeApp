using SmartFridgeApp.Core.Exceptions;

namespace SmartFridgeApp.Core.Domain.Entities;

public class ProductVariant
{
    public int VariantId { get; set; }
    public short FoodProductId { get; set; }
    public FoodProduct FoodProduct { get; set; }
    public string Name { get; set; }
    public string? Barcode { get; set; }

    private ProductVariant() { }

    public ProductVariant(FoodProduct foodProduct, string name, string? barcode = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidInputException("Variant name can't be empty.", "InvalidVariantName");

        FoodProduct = foodProduct;
        FoodProductId = foodProduct.FoodProductId;
        Name = name.Trim();
        Barcode = barcode;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidInputException("Variant name can't be empty.", "InvalidVariantName");
        Name = name.Trim();
    }

    public void UpdateBarcode(string? barcode) => Barcode = barcode;
}
