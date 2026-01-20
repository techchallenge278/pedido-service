using Pedido.Application.DTOs;
using FluentAssertions;
using Xunit;

namespace Pedido.Tests.Application.DTOs;

public class PedidoStatusDtoTests
{
    [Fact]
    public void PedidoStatusDto_ShouldInitializeWithDefaultValues()
    {
        // Act
        var dto = new PedidoStatusDto();

        // Assert
        dto.Should().NotBeNull();
        dto.PedidoId.Should().Be(Guid.Empty);
        dto.Status.Should().BeEmpty();
        dto.StatusDescription.Should().BeEmpty();
        dto.TotalPrice.Should().Be(0m);
        dto.CreatedAt.Should().Be(default(DateTime));
        dto.IsAnonymous.Should().BeFalse();
    }

    [Fact]
    public void PedidoStatusDto_ShouldSetAllProperties()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;

        // Act
        var dto = new PedidoStatusDto
        {
            PedidoId = pedidoId,
            Status = "Pending",
            StatusDescription = "Pedido criado, aguardando pagamento",
            TotalPrice = 50.0m,
            CreatedAt = createdAt,
            IsAnonymous = true
        };

        // Assert
        dto.PedidoId.Should().Be(pedidoId);
        dto.Status.Should().Be("Pending");
        dto.StatusDescription.Should().Be("Pedido criado, aguardando pagamento");
        dto.TotalPrice.Should().Be(50.0m);
        dto.CreatedAt.Should().Be(createdAt);
        dto.IsAnonymous.Should().BeTrue();
    }
}
