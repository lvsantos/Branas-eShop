using eShop.Api.Validators;

namespace eShop.Api.Models;

public sealed class Order
{
    public const int FREIGHT_MINUMUM_VALUE = 10;

    private List<Product> _products = new();

    public Order(IEnumerable<Product> products, string cpf)
    {
        bool isProductDuplicated = products.ToHashSet(new CompareProducts()).Count != products.Count();
        if (isProductDuplicated)
            throw new ArgumentException();
        if (!CpfValidator.Validate(cpf))
            throw new ArgumentException();

        Code = Guid.NewGuid();
        _products = products.ToList();
        Cpf = cpf;
    }
    public Order(Product product, string cpf)
        : this(new List<Product> { product }, cpf)
    {
    }

    public Guid Code { get; init; }
    public IReadOnlyCollection<Product> Products { get => _products; }
    public string Cpf { get; private set; }
    public DiscountCoupon? DiscountCoupon { get; private set; }

    public void AssociateDiscountCoupon(DiscountCoupon discountCoupon)
    {
        if (discountCoupon.ExpiredDate < DateTime.UtcNow)
            throw new ArgumentException();
        
        DiscountCoupon = discountCoupon;
    }
    public decimal CalculateFreightAmount()
    {
        decimal freight = _products.Sum(p => 1000 * p.CalculateVolumeInMetersCubic() * p.CalculateDensityInKgPerMetersCubic() / 100);
        return freight < FREIGHT_MINUMUM_VALUE ? FREIGHT_MINUMUM_VALUE : freight;
    }
    public decimal CalculatePriceAmount()
    {
        decimal productsPriceAmount = _products.Sum(p => p.Price * p.Quantity);
        decimal discountCouponValue = (DiscountCoupon is null ? 0 : DiscountCoupon.Value);
        return productsPriceAmount - discountCouponValue;
    }
}
