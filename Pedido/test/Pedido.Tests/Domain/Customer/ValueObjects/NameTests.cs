using Pedido.Domain.Custumer.Exceptions;
using Pedido.Domain.Custumer.ValueObjects;
using FluentAssertions;

namespace Pedido.Tests.Domain.Customers.ValueObjects;

public class NameTests
{
    [Fact]
    public void Create_WithValidName_ShouldReturnNameObject()
    {
        // Arrange & Act
        var name = Name.Create("João Silva");

        // Assert
        name.Should().NotBeNull();
        name.Value.Should().Be("João Silva");
    }

    [Fact]
    public void Create_WithNameContainingAccents_ShouldCapitalizeProperly()
    {
        var name = Name.Create("José da Silva");
        name.Should().NotBeNull();
        name.Value.Should().Be("José Da Silva");
    }

    [Fact]
    public void Create_WithLowerCaseName_ShouldCapitalizeFirstLetters()
    {
        var name = Name.Create("maria santos");
        name.Should().NotBeNull();
        name.Value.Should().Be("Maria Santos");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Create_WithEmptyOrNullOrWhitespace_ShouldThrowException(string invalidName)
    {
        var act = () => Name.Create(invalidName);
        act.Should().Throw<ClienteDomainException>()
            .WithMessage("O nome não pode ser vazio");
    }

    [Fact]
    public void Create_WithNameTooShort_ShouldThrowException()
    {
        var act = () => Name.Create("Ab");
        act.Should().Throw<ClienteDomainException>()
            .WithMessage("O nome deve ter no mínimo 3 caracteres");
    }

    [Fact]
    public void Create_WithNameTooLong_ShouldThrowException()
    {
        var longName = new string('A', 101);
        var act = () => Name.Create(longName);
        act.Should().Throw<ClienteDomainException>()
            .WithMessage("O nome não pode ter mais que 100 caracteres");
    }

    [Theory]
    [InlineData("João123")]
    [InlineData("Maria@Silva")]
    [InlineData("Ana#Maria")]
    public void Create_WithInvalidCharacters_ShouldThrowException(string invalidName)
    {
        var act = () => Name.Create(invalidName);
        act.Should().Throw<ClienteDomainException>()
            .WithMessage("O nome possui caracteres inválidos");
    }

    [Fact]
    public void Create_WithNameContainingHyphen_ShouldNormalizeCorrectly()
    {
        var name = Name.Create("Maria-José");
        name.Should().NotBeNull();
        // Observação: normalização atual só aplica TitleCase por palavras separadas por espaço
        name.Value.Should().Be("Maria-josé");
    }

    [Fact]
    public void Create_WithNameContainingApostrophe_ShouldNormalizeCorrectly()
    {
        var name = Name.Create("O'Brien");
        name.Should().NotBeNull();
        name.Value.Should().Be("O'brien");
    }

    [Fact]
    public void ToString_ShouldReturnTheNameValue()
    {
        var name = Name.Create("João Silva");
        name.ToString().Should().Be("João Silva");
    }

    [Fact]
    public void ImplicitConversion_ShouldReturnStringValue()
    {
        var name = Name.Create("João Silva");
        string result = name;
        result.Should().Be("João Silva");
    }

    [Fact]
    public void Create_WithNameContainingMultipleSpaces_ShouldTrimAndCapitalize()
    {
        // Arrange & Act
        var name = Name.Create("  maria   dos   santos  ");

        // Assert
        name.Should().NotBeNull();
        name.Value.Should().Be("Maria Dos Santos");
    }

    [Fact]
    public void Create_WithNameContainingMixedCasesAndSpaces_ShouldNormalizeCorrectly()
    {
        var name = Name.Create("  jOãO   da sIlvA  ");
        name.Should().NotBeNull();
        name.Value.Should().Be("João Da Silva");
    }
}
