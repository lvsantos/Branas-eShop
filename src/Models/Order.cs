using eShop.Api.Validators;

namespace eShop.Api.Models;

public sealed class Order
{
    private List<Product> _products = new();
    
    public Order(IEnumerable<Product> products, string cpf)
    {
        if (!CpfValidator.Validate(cpf))
            throw new ArgumentException();

        Code = Guid.NewGuid();
        _products = products.ToList();
        Cpf = cpf;
    }

    public Guid Code { get; init; }
    public IReadOnlyCollection<Product> Products { get => _products; }
    public string Cpf { get; private set; }
    public decimal DiscountCoupon { get; private set; }

    public void AssociateDiscountCoupon(decimal discountCoupon)
    {
        DiscountCoupon = discountCoupon;
    }
    public decimal CalculatePriceAmount()
    {
        decimal priceAmount = Products.Sum(p => p.Price * p.Quantity);
        return priceAmount - DiscountCoupon;
    }
}
