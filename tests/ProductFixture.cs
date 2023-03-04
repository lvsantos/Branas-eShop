using Bogus;
using eShop.Api.Models;

namespace eShop.Tests;

public class ProductFixture
{
    public ProductFixture()
    {
    }

    public List<Product> CreateProducts(int amount = 1)
    {
        return new Faker<Product>(Locale.PT_BR)
            .CustomInstantiator(f => new Product(
                description: f.Commerce.ProductName(),
                price: f.Random.Decimal(1, 1000),
                quantity: f.Random.Int(1, 10),
                deepth: new FreightField(f.Random.Int(10, 500), FreightFieldUnitMeasure.Centimeters),
                height: new FreightField(f.Random.Int(10, 400), FreightFieldUnitMeasure.Centimeters),
                width: new FreightField(f.Random.Int(10, 150), FreightFieldUnitMeasure.Centimeters),
                weight: new FreightField(f.Random.Int(1, 100), FreightFieldUnitMeasure.Kilograms)))
            .Generate(amount);
    }
    public Product CreateProduct(
        string description,
        decimal price,
        int quantity,
        int deepth,
        int height,
        int width,
        int weight)
    {
        Product product = new(
            description,
            price,
            quantity,
            new FreightField(deepth, FreightFieldUnitMeasure.Centimeters),
            new FreightField(height, FreightFieldUnitMeasure.Centimeters),
            new FreightField(width, FreightFieldUnitMeasure.Centimeters),
            new FreightField(weight, FreightFieldUnitMeasure.Kilograms));
        return product;
    }
}
