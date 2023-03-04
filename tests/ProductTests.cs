using FluentAssertions;

namespace eShop.Tests;

public class ProductTests : IClassFixture<ProductFixture>
{
    private readonly ProductFixture _productFixture;

    public ProductTests(ProductFixture productFixture)
    {
        _productFixture = productFixture;
    }

    [Fact(DisplayName = "Ao fazer um pedido, a quantidade de um item não pode ser negativa")]
    public void Product_ShouldNot_BeCreated_When_QuantityIsNegative()
    {
        //Arrange & Act
        Action action = () => _productFixture.CreateProduct("Descrição 01", 50, -1, 0, 0, 0, 0);

        //Assert
        action.Should().Throw<ArgumentException>();
    }
    [Theory(DisplayName = "Nenhuma dimensão do item pode ser negativa")]
    [MemberData(nameof(GetInvalidDimensions))]
    
    public void Product_ShouldNot_BeCreated_When_AnyDimensionIsNegative(int length, int height, int width)
    {
        //Arrange & Act
        Action action = () => _productFixture.CreateProduct("Descrição 01", 50, 1, length, height, width, 0);

        //Assert
        action.Should().Throw<ArgumentException>();
    }
    [Fact(DisplayName = "Criar um pedido com as dimensões não negativas")]
    public void Product_Should_BeCreated_When_AllDimensionsIsNotNegative()
    {
        //Arrange & Act
        Action action = () => _productFixture.CreateProduct("Descrição 01", 50, 1, 0, 0, 0, 0);

        //Assert
        action.Should().NotThrow<Exception>();
    }

    [Fact(DisplayName = "O peso do item não pode ser negativo")]
    public void Product_ShouldNot_BeCreated_When_WeightIsNegative()
    {
        //Arrange & Act
        Action action = () => _productFixture.CreateProduct("Descrição 01", 50, 1, 0, 0, 0, -1);

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    public static IEnumerable<object[]> GetInvalidDimensions()
    {
        yield return new object[] { 1, 1, -1 };
        yield return new object[] { 1, -1, 1 };
        yield return new object[] { 1, -1, -1 };
        yield return new object[] { -1, 1, 1 };
        yield return new object[] { -1, 1, -1 };
        yield return new object[] { -1, -1, 1 };
        yield return new object[] { -1, -1, -1 };
    }
}
