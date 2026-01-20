using Pedido.Domain.Entities;
using Pedido.Domain.Exceptions;
using Pedido.Domain.ValueObjects;
using FluentAssertions;
using PedidoEntity = Pedido.Domain.Entities.Pedido;

namespace Pedido.Tests.Domain;

public class PedidoTests
{
    [Fact]
    public void PrivateConstructor_ShouldInitializeDefaults()
    {
        // chama o construtor privado via reflection
        var ctor = typeof(PedidoEntity)
            .GetConstructor(System.Reflection.BindingFlags.Instance |
                            System.Reflection.BindingFlags.NonPublic,
                            null, Type.EmptyTypes, null);

        var pedido = (PedidoEntity)ctor!.Invoke(null);

        pedido.Status.Should().Be(PedidoStatus.Pending);
        pedido.TotalPrice.Should().Be(0);
        pedido.Items.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithValidData_ShouldInstantiateOrder()
    {
        var clienteId = Guid.NewGuid();
        var items = new List<PedidoItem>
        {
            PedidoItem.Create(Guid.NewGuid(), "Batata", 25.50m, 2)
        };

        var order = PedidoEntity.Create(clienteId, items);

        order.Should().NotBeNull();
        order.ClienteId.Should().Be(clienteId);
        order.Items.Should().HaveCount(1);
        order.Status.Should().Be(PedidoStatus.Pending);
        order.TotalPrice.Should().Be(51.00m);
    }

    [Fact]
    public void Create_WithoutClient_ShouldAllowOrderWithNullClienteId()
    {
        var items = new List<PedidoItem>
        {
            PedidoItem.Create(Guid.NewGuid(), "Batata", 25.50m, 1)
        };

        var order = PedidoEntity.Create(null, items);

        order.ClienteId.Should().BeNull();
        order.Items.Should().HaveCount(1);
    }

    [Fact]
    public void Create_WithNoItems_ShouldThrowException()
    {
        var act = () => PedidoEntity.Create(Guid.NewGuid(), new List<PedidoItem>());

        act.Should().Throw<PedidoDomainException>()
            .WithMessage("O pedido deve ter pelo menos um item");
    }

    [Fact]
    public void Create_WithInvalidClienteId_ShouldThrowException()
    {
        var items = new List<PedidoItem>
        {
            PedidoItem.Create(Guid.NewGuid(), "Batata", 25.50m, 1)
        };

        var act = () => PedidoEntity.Create(Guid.Empty, items);

        act.Should().Throw<PedidoDomainException>()
            .WithMessage("Quando informado, o ID do cliente deve ser um GUID válido");
    }

    [Fact]
    public void AddItem_WhenPending_ShouldAddItemAndUpdateTotal()
    {
        var order = PedidoEntity.Create(null, new List<PedidoItem>
        {
            PedidoItem.Create(Guid.NewGuid(), "Batata", 25.50m, 1)
        });

        var newItem = PedidoItem.Create(Guid.NewGuid(), "Batata Frita", 15.00m, 1);

        order.AddItem(newItem);

        order.Items.Should().HaveCount(2);
        order.TotalPrice.Should().Be(40.50m);
    }

    [Fact]
    public void AddItem_WhenNotPending_ShouldThrowException()
    {
        var order = PedidoEntity.Create(null, new List<PedidoItem>
        {
            PedidoItem.Create(Guid.NewGuid(), "Batata", 25.50m, 1)
        });

        order.UpdateStatus(PedidoStatus.Processing);

        var act = () => order.AddItem(
            PedidoItem.Create(Guid.NewGuid(), "Nuggets", 15.00m, 1)
        );

        act.Should().Throw<PedidoDomainException>()
            .WithMessage("*não está pendente*");
    }

    [Fact]
    public void AddItem_WithNullItem_ShouldThrowException()
    {
        var order = PedidoEntity.Create(null, new List<PedidoItem>
        {
            PedidoItem.Create(Guid.NewGuid(), "Hambúrguer", 25.50m, 1)
        });

        var act = () => order.AddItem(null!);

        act.Should().Throw<PedidoDomainException>()
            .WithMessage("*não pode ser nulo*");
    }

    [Fact]
    public void RemoveItem_WhenExistsAndPending_ShouldRemoveAndUpdateTotal()
    {
        var productId = Guid.NewGuid();
        var order = PedidoEntity.Create(null, new List<PedidoItem>
        {
            PedidoItem.Create(productId, "Hambúrguer", 25.50m, 1),
            PedidoItem.Create(Guid.NewGuid(), "Batata Frita", 15.00m, 1)
        });

        var removed = order.RemoveItem(productId);

        removed.Should().BeTrue();
        order.Items.Should().HaveCount(1);
        order.TotalPrice.Should().Be(15.00m);
    }

    [Fact]
    public void RemoveItem_WhenDoesNotExist_ShouldReturnFalse()
    {
        var order = PedidoEntity.Create(null, new List<PedidoItem>
        {
            PedidoItem.Create(Guid.NewGuid(), "Hambúrguer", 25.50m, 1)
        });

        var removed = order.RemoveItem(Guid.NewGuid());

        removed.Should().BeFalse();
        order.Items.Should().HaveCount(1);
    }

    [Fact]
    public void RemoveItem_WhenNotPending_ShouldThrowException()
    {
        var order = PedidoEntity.Create(null, new List<PedidoItem>
        {
            PedidoItem.Create(Guid.NewGuid(), "Hambúrguer", 25.50m, 1)
        });

        order.UpdateStatus(PedidoStatus.Processing);

        var act = () => order.RemoveItem(Guid.NewGuid());

        act.Should().Throw<PedidoDomainException>()
            .WithMessage("*não está pendente*");
    }

    [Fact]
    public void SetQrCode_And_SetPreferenceId_ShouldAssignValues()
    {
        var order = PedidoEntity.Create(null, new List<PedidoItem>
        {
            PedidoItem.Create(Guid.NewGuid(), "Batata", 25.50m, 1)
        });

        order.SetQrCode("qr-code");
        order.SetPreferenceId("pref-id");

        order.QrCode.Should().Be("qr-code");
        order.PreferenceId.Should().Be("pref-id");
    }

    [Fact]
    public void SetQrCodeOrPreferenceId_WhenEmpty_ShouldThrowException()
    {
        var order = PedidoEntity.Create(null, new List<PedidoItem>
        {
            PedidoItem.Create(Guid.NewGuid(), "Batata", 25.50m, 1)
        });

        order.Invoking(o => o.SetQrCode(""))
            .Should().Throw<PedidoDomainException>();

        order.Invoking(o => o.SetPreferenceId(""))
            .Should().Throw<PedidoDomainException>();
    }

    [Fact]
    public void UpdateStatus_ShouldAllowValidTransitions_AndThrowOnInvalid()
    {
        var order = PedidoEntity.Create(null, new List<PedidoItem>
        {
            PedidoItem.Create(Guid.NewGuid(), "Hambúrguer", 25.50m, 1)
        });

        order.UpdateStatus(PedidoStatus.Processing);
        order.UpdateStatus(PedidoStatus.Ready);
        order.UpdateStatus(PedidoStatus.Completed);

        order.Status.Should().Be(PedidoStatus.Completed);

        var actInvalid = () => order.UpdateStatus(PedidoStatus.Pending);

        actInvalid.Should().Throw<PedidoDomainException>()
            .WithMessage("*transição*");
    }

    [Fact]
    public void SetStatusDireto_ShouldUpdateStatusWithoutRules()
    {
        var order = PedidoEntity.Create(null, new List<PedidoItem>
        {
            PedidoItem.Create(Guid.NewGuid(), "Batata", 25.50m, 1)
        });

        order.SetStatusDireto(PedidoStatus.Ready);

        order.Status.Should().Be(PedidoStatus.Ready);
    }
}
