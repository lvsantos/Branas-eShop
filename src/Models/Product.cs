using System.Diagnostics.CodeAnalysis;

namespace eShop.Api.Models;

public sealed class Product
{
    public Product(
        string description,
        decimal price,
        int quantity,
        FreightField deepth,
        FreightField height,
        FreightField width,
        FreightField weight)
    {
        if (quantity < 0)
            throw new ArgumentException();

        Code = Guid.NewGuid();
        Description = description;
        Price = price;
        Quantity = quantity;
        Deepth = deepth;
        Height = height;
        Width = width;
        Weight = weight;
    }

    public Guid Code { get; init; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; private set; }
    public FreightField Deepth { get; set; }
    public FreightField Height { get; private set; }
    public FreightField Width { get; private set; }
    public FreightField Weight { get; private set; }

    public decimal CalculateVolumeInMetersCubic()
    {
        int volumeInCentimetersCubic = Deepth.Value * Height.Value * Width.Value;
        return volumeInCentimetersCubic / 1_000_000m;
    }
    public decimal CalculateDensityInKgPerMetersCubic()
    {
        decimal densityInKgPerMetersCubic = Weight.Value / CalculateVolumeInMetersCubic();
        decimal densityInKgPerMetersCubicTruncated = decimal.Truncate(densityInKgPerMetersCubic);
        return densityInKgPerMetersCubicTruncated;
    }
}

public class CompareProducts : IEqualityComparer<Product>
{
    public bool Equals(Product? x, Product? y)
    {
        if (x is null || y is null)
            return false;
        return x.Code == y.Code;
    }

    public int GetHashCode([DisallowNull] Product obj)
    {
        return obj.Code.GetHashCode();
    }
}