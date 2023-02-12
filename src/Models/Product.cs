namespace eShop.Api.Models;

public sealed class Product
{
	public Product(string description, decimal price, int quantity)
	{
        Code = Guid.NewGuid();
        Description = description;
        Price = price;
        Quantity = quantity;
    }

    public Guid Code { get; init; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }
}