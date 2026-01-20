using Pedido.Domain.Exceptions;
using Pedido.Domain.ValueObjects;
using FluentAssertions;

namespace Pedido.Tests.Domain;

public class PedidoItemTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateOrderItem()
    {
        // Arrange: preparando dados válidos para criar um item de pedido
        var productId = Guid.NewGuid();
        var productName = "Hambúrguer";
        var unitPrice = 25.50m;
        var quantity = 2;

        // Act: criando o item de pedido
        var item = PedidoItem.Create(productId, productName, unitPrice, quantity);

        // Assert: verificando se o item foi criado corretamente
        item.Should().NotBeNull();
        item.ProdutoId.Should().Be(productId);
        item.ProdutoNome.Should().Be(productName);
        item.UnitPrice.Should().Be(unitPrice);
        item.Quant.Should().Be(quantity);
        item.SubTotal.Should().Be(51.00m); // total = unitPrice * quantity
    }

    [Fact]
    public void Create_WithEmptyProductId_ShouldThrowException()
    {
        // Act & Assert: tentar criar item com Guid vazio deve lançar exceção
        var act = () => PedidoItem.Create(Guid.Empty, "Hambúrguer", 25.50m, 1);
        act.Should().Throw<PedidoDomainException>()
            .WithMessage("O ID do produto é obrigatório");
    }

    [Fact]
    public void Create_WithEmptyProductName_ShouldThrowException()
    {
        // Act & Assert: criar item com nome vazio não é permitido
        var act = () => PedidoItem.Create(Guid.NewGuid(), "", 25.50m, 1);
        act.Should().Throw<PedidoDomainException>()
            .WithMessage("O nome do produto é obrigatório");
    }

    [Fact]
    public void Create_WithZeroUnitPrice_ShouldThrowException()
    {
        // Act & Assert: preço unitário zero deve lançar exceção
        var act = () => PedidoItem.Create(Guid.NewGuid(), "Hambúrguer", 0m, 1);
        act.Should().Throw<PedidoDomainException>()
            .WithMessage("O preço unitário deve ser maior que zero");
    }

    [Fact]
    public void Create_WithZeroQuantity_ShouldThrowException()
    {
        // Act & Assert: quantidade zero não é permitida
        var act = () => PedidoItem.Create(Guid.NewGuid(), "Hambúrguer", 25.50m, 0);
        act.Should().Throw<PedidoDomainException>()
            .WithMessage("A quantidade deve ser maior que zero");
    }

    [Fact]
    public void WithQuantity_WithValidQuantity_ShouldReturnNewItem()
    {
        // Arrange: item inicial com quantidade 1
        var item = PedidoItem.Create(Guid.NewGuid(), "Hambúrguer", 25.50m, 1);

        // Act: atualizando a quantidade do item
        var newItem = item.WithQuantity(3);

        // Assert: conferindo se o novo item tem a quantidade atualizada e subtotal correto
        newItem.Quant.Should().Be(3);
        newItem.SubTotal.Should().Be(76.50m); // 25.50 * 3
        newItem.Id.Should().Be(item.Id); // mesmo Id, apenas quantidade mudou
    }

    [Fact]
    public void WithQuantity_WithZeroQuantity_ShouldThrowException()
    {
        // Arrange: item inicial válido
        var item = PedidoItem.Create(Guid.NewGuid(), "Hambúrguer", 25.50m, 1);

        // Act & Assert: tentar atualizar quantidade para zero deve lançar exceção
        var act = () => item.WithQuantity(0);
        act.Should().Throw<PedidoDomainException>()
            .WithMessage("A quantidade deve ser maior que zero");
    }
}
