using Bogus;
using Bogus.Extensions.Brazil;
using eShop.Api.Models;
using FluentAssertions;

namespace eShop.Tests;

public class OrderTests : IClassFixture<ProductFixture>
{
    private readonly ProductFixture _productFixture;
    private readonly string _cpf;

    public OrderTests(ProductFixture productFixture)
    {
        _productFixture = productFixture;
        _cpf = new Faker(Locale.PT_BR).Person.Cpf();
    }

    [Theory(DisplayName = "Deve criar um pedido com 3 produtos (com descrição, preço e quantidade) e calcular o valor total")]
    [InlineData("11144477735")]
    [InlineData("058.631.528-42")]
    public void Order_Should_CalculatePrice(string cpf)
    {
        //Arrange
        List<Product> products = _productFixture.CreateProducts(3);
        var order = new Order(products, cpf);

        //Act
        decimal priceAmount = order.CalculatePriceAmount();

        //Assert
        priceAmount.Should().Be(order.Products.Sum(p => p.Price * p.Quantity));
    }
    [Theory(DisplayName = "Deve criar um pedido com 3 produtos, associar um cupom de desconto e calcular o total")]
    [InlineData("22663435309")]
    [InlineData("226.634.353-09")]
    [InlineData("26346314130")]
    [InlineData("79345300805")]
    [InlineData("34188321300")]
    public void Order_Should_CalculatePrice_When_DiscountCouponIsAssociated(string cpf)
    {
        //Arrange
        List<Product> products = _productFixture.CreateProducts(3);
        var order = new Order(products, cpf);
        var couponValue = 20;
        var coupon = new DiscountCoupon
        {
            Value = couponValue,
            ExpiredDate = DateTime.UtcNow.AddDays(+1)
        };
        order.AssociateDiscountCoupon(coupon);

        //Act
        decimal priceAmount = order.CalculatePriceAmount();

        //Assert
        priceAmount.Should().Be(order.Products.Sum(p => p.Price * p.Quantity) - couponValue);
    }
    [Theory(DisplayName = "Não deve criar um pedido com cpf inválido")]
    [InlineData("123456789")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("222.222.222-22")]
    [InlineData("226.634.353-90")]
    [InlineData("226.634.353-00")]
    [InlineData("226.634.353-50")]
    [InlineData("34188321d00")]
    public void Order_Should_ThownArgumentException_When_CpfIsInvalid(string? cpf)
    {
        //Arrange
        List<Product> products = _productFixture.CreateProducts(5);

        //Act
        Action act = () => new Order(products, cpf!);

        //Assert
        act.Should().Throw<ArgumentException>();
    }
    [Fact(DisplayName = "Não deve aplicar cupom de desconto expirado")]
    public void Order_ShouldNot_ApplyDiscountCoupon_When_ItIsExpired()
    {
        //Arrange
        List<Product> products = _productFixture.CreateProducts(4);
        var order = new Order(products, _cpf);
        var coupon = new DiscountCoupon
        {
            Value = 15,
            ExpiredDate = DateTime.UtcNow.AddSeconds(-1)
        };
        
        // Act
        Action act = () => order.AssociateDiscountCoupon(coupon);

        //Assert
        act.Should().Throw<ArgumentException>();
    }
    [Fact(DisplayName = "Ao fazer um pedido, o mesmo item não pode ser informado mais de uma vez")]
    public void Order_ShouldNot_BeCreated_When_ProductIsDuplicated()
    {
        //Arrange
        List<Product> products = _productFixture.CreateProducts(5);
        products = CreateProductDuplicated(products);

        //Act
        Action act = () => new Order(products, _cpf);

        //Assert
        act.Should().Throw<ArgumentException>();

        List<Product> CreateProductDuplicated(List<Product> products)
        {
            products.Add(products.Last());
            return products;
        }
    }
    [Fact(DisplayName = "Deve calcular o valor do frete com base nas dimensões (altura, largura e profundidade em cm) e o peso dos produtos (em kg)")]
    public void Order_Should_CalculateFreight()
    {
        //Arrange
        List<Product> products = _productFixture.CreateProducts(5);
        Order order = new(products, _cpf);

        //Act
        decimal freight = order.CalculateFreightAmount();

        //Assert
        var result = order.Products.Sum(p => 1000 * p.CalculateVolumeInMetersCubic() * p.CalculateDensityInKgPerMetersCubic() / 100);
        result.Should().Be(freight);
    }

    [Fact(DisplayName = "Deve retornar o preço mínimo de frete caso ele seja superior ao valor calculado")]
    public void Order_Should_ReturnMinimumFreightValue_When_FreightIsUnderMinumumValue()
    {
        //Arrange
        Product product = _productFixture.CreateProduct(
            description: "Pilha",
            price: 10,
            quantity: 5,
            deepth: 3,
            height: 3,
            width: 10,
            weight: 1);
        Order order = new(product, _cpf);

        //Act
        decimal freight = order.CalculateFreightAmount();

        //Assert
        freight.Should().Be(Order.FREIGHT_MINUMUM_VALUE);
    }
}
