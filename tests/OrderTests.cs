using eShop.Api.Models;
using FluentAssertions;

namespace eShop.Tests;

public class OrderTests
{
    [Theory(DisplayName = "Deve criar um pedido com 3 produtos (com descrição, preço e quantidade) e calcular o valor total")]
    [InlineData("11144477735")]
    [InlineData("058.631.528-42")]
    public void Order_Should_CalculatePrice(string cpf)
    {
        //Arrange
        var products = new List<Product>
        {
            new Product("Produto 1", 10, 1),
            new Product("Produto 2", 20, 2),
            new Product("Produto 3", 30, 3)
        };
        var order = new Order(products, cpf);

        //Act
        decimal priceAmount = order.CalculatePriceAmount();

        //Assert
        priceAmount.Should().Be(140);
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
        var products = new List<Product>
        {
            new Product("Produto 1", 10, 1),
            new Product("Produto 2", 20, 2),
            new Product("Produto 3", 30, 3)
        };
        var order = new Order(products, cpf);
        order.AssociateDiscountCoupon(15);

        //Act
        decimal priceAmount = order.CalculatePriceAmount();

        //Assert
        priceAmount.Should().Be(125);
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
        var products = new List<Product>
        {
            new Product("Produto 1", 10, 1),
            new Product("Produto 2", 20, 2),
            new Product("Produto 3", 30, 3)
        };

        //Act
        Action act = () => new Order(products, cpf!);

        //Assert
        act.Should().Throw<ArgumentException>();
    }
}
