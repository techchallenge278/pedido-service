using Pedido.Application.Queries;
using Pedido.Domain.Custumer.Entities;
using Pedido.Domain.Custumer.ValueObjects;
using Pedido.Domain.Entities;
using Pedido.Domain.Repositories;
using Pedido.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using PedidoEntity = Pedido.Domain.Entities.Pedido;
using PedidoItemValueObject = Pedido.Domain.ValueObjects.PedidoItem;

namespace Pedido.Tests.Application.Queries;

public class GetPedidosQueryHandlerTests
{
    private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
    private readonly GetPedidosQueryHandler _handler;

    public GetPedidosQueryHandlerTests()
    {
        _pedidoRepositoryMock = new Mock<IPedidoRepository>();
        _handler = new GetPedidosQueryHandler(
            _pedidoRepositoryMock.Object,
            Microsoft.Extensions.Logging.Abstractions.NullLogger<GetPedidosQueryHandler>.Instance);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldReturnPedidos()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var cliente = Cliente.Create(
            Name.Create("John Doe"));

        var order1 = PedidoEntity.Create(clienteId, new List<PedidoItemValueObject>
        {
            PedidoItemValueObject.Create(Guid.NewGuid(), "Produto 1", 10m, 1)
        });
        var order2 = PedidoEntity.Create(clienteId, new List<PedidoItemValueObject>
        {
            PedidoItemValueObject.Create(Guid.NewGuid(), "Produto 2", 15m, 1)
        });

        var orders = new List<PedidoEntity> { order1, order2 };

        _pedidoRepositoryMock.Setup(x => x.GetPedidosAsync(1, 10, null, null))
            .ReturnsAsync((orders, 2));

        var query = new GetPedidosQuery { PageNumber = 1, PageSize = 10 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Pedidos.Should().HaveCount(2);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(2);
        _pedidoRepositoryMock.Verify(x => x.GetPedidosAsync(1, 10, null, null), Times.Once);
    }

    [Fact]
    public async Task Handle_WithClienteIdFilter_ShouldReturnFilteredPedidos()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var cliente = Cliente.Create(
            Name.Create("John Doe"));

        var order = PedidoEntity.Create(clienteId, new List<PedidoItemValueObject>
        {
            PedidoItemValueObject.Create(Guid.NewGuid(), "Produto 1", 10m, 1)
        });
        var orders = new List<PedidoEntity> { order };

        _pedidoRepositoryMock.Setup(x => x.GetPedidosAsync(1, 10, clienteId, null))
            .ReturnsAsync((orders, 1));

        var query = new GetPedidosQuery 
        { 
            PageNumber = 1, 
            PageSize = 10,
            ClienteId = clienteId
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Pedidos.Should().HaveCount(1);
        _pedidoRepositoryMock.Verify(x => x.GetPedidosAsync(1, 10, clienteId, null), Times.Once);
    }

    [Fact]
    public async Task Handle_WithStatusFilter_ShouldReturnFilteredPedidos()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var cliente = Cliente.Create(
            Name.Create("John Doe"));

        var order = PedidoEntity.Create(clienteId, new List<PedidoItemValueObject>
        {
            PedidoItemValueObject.Create(Guid.NewGuid(), "Produto 1", 10m, 1)
        });
        var orders = new List<PedidoEntity> { order };

        _pedidoRepositoryMock.Setup(x => x.GetPedidosAsync(1, 10, null, PedidoStatus.Pending))
            .ReturnsAsync((orders, 1));

        var query = new GetPedidosQuery 
        { 
            PageNumber = 1, 
            PageSize = 10,
            Status = "Pending"
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Pedidos.Should().HaveCount(1);
        _pedidoRepositoryMock.Verify(x => x.GetPedidosAsync(1, 10, null, PedidoStatus.Pending), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidPageNumber_ShouldUseDefaultPageNumber()
    {
        // Arrange
        var orders = new List<PedidoEntity>();
        _pedidoRepositoryMock.Setup(x => x.GetPedidosAsync(1, 10, null, null))
            .ReturnsAsync((orders, 0));

        var query = new GetPedidosQuery { PageNumber = 0, PageSize = 10 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.PageNumber.Should().Be(1);
        _pedidoRepositoryMock.Verify(x => x.GetPedidosAsync(1, 10, null, null), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidPageSize_ShouldUseDefaultPageSize()
    {
        // Arrange
        var orders = new List<PedidoEntity>();
        _pedidoRepositoryMock.Setup(x => x.GetPedidosAsync(1, 10, null, null))
            .ReturnsAsync((orders, 0));

        var query = new GetPedidosQuery { PageNumber = 1, PageSize = 0 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.PageSize.Should().Be(10);
        _pedidoRepositoryMock.Verify(x => x.GetPedidosAsync(1, 10, null, null), Times.Once);
    }

    [Fact]
    public async Task Handle_WithPageSizeGreaterThan100_ShouldCapAt100()
    {
        // Arrange
        var orders = new List<PedidoEntity>();
        _pedidoRepositoryMock.Setup(x => x.GetPedidosAsync(1, 100, null, null))
            .ReturnsAsync((orders, 0));

        var query = new GetPedidosQuery { PageNumber = 1, PageSize = 150 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.PageSize.Should().Be(100);
        _pedidoRepositoryMock.Verify(x => x.GetPedidosAsync(1, 100, null, null), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidStatus_ShouldIgnoreStatusFilter()
    {
        // Arrange
        var orders = new List<PedidoEntity>();
        _pedidoRepositoryMock.Setup(x => x.GetPedidosAsync(1, 10, null, null))
            .ReturnsAsync((orders, 0));

        var query = new GetPedidosQuery 
        { 
            PageNumber = 1, 
            PageSize = 10,
            Status = "InvalidStatus"
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        _pedidoRepositoryMock.Verify(x => x.GetPedidosAsync(1, 10, null, null), Times.Once);
    }

    [Fact]
    public async Task Handle_WithRepositoryException_ShouldReturnEmptyList()
    {
        // Arrange
        _pedidoRepositoryMock.Setup(x => x.GetPedidosAsync(1, 10, null, null))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        var query = new GetPedidosQuery { PageNumber = 1, PageSize = 10 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Pedidos.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }
}

