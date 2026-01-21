using Pedido.Application.Queries;
using Pedido.Domain.Entities;
using Pedido.Domain.Repositories;
using Pedido.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using PedidoEntity = Pedido.Domain.Entities.Pedido;
using PedidoItemValueObject = Pedido.Domain.ValueObjects.PedidoItem;
using System.Text;

namespace FastFood.Pedido.Tests.Application.Queries;

public class GetPedidoByIdQueryHandlerTests
{
    private readonly Mock<IPedidoRepository> _repositoryMock;
    private readonly GetPedidoByIdQueryHandler _handler;

    public GetPedidoByIdQueryHandlerTests()
    {
        _repositoryMock = new Mock<IPedidoRepository>();
        _handler = new GetPedidoByIdQueryHandler(
            _repositoryMock.Object,
            Microsoft.Extensions.Logging.Abstractions.NullLogger<GetPedidoByIdQueryHandler>.Instance);
    }

    [Fact(DisplayName = "Deve retornar pedido existente corretamente")]
    public async Task Handle_ExistingPedido_ShouldReturnPedidoWithItems()
    {
        var clienteId = Guid.NewGuid();
        var pedidoItems = new List<PedidoItemValueObject>
        {
            PedidoItemValueObject.Create(Guid.NewGuid(), "Hambúrguer", 10.50m, 2),
            PedidoItemValueObject.Create(Guid.NewGuid(), "Batata Frita", 5.00m, 1)
        };
        var pedido = PedidoEntity.Create(clienteId, pedidoItems);
        var pedidoId = pedido.Id;

        _repositoryMock.Setup(r => r.GetByIdWithItemsAsync(pedidoId))
                       .ReturnsAsync(pedido);

        var query = new GetPedidoByIdQuery { Id = pedidoId };
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Pedido.Should().NotBeNull();
        result.Pedido!.Id.Should().Be(pedidoId);
        result.Pedido.ClienteId.Should().Be(clienteId);
        result.Pedido.Items.Should().HaveCount(2);
        _repositoryMock.Verify(r => r.GetByIdWithItemsAsync(pedidoId), Times.Once);
    }

    [Fact(DisplayName = "Pedido inexistente deve retornar erro de não encontrado")]
    public async Task Handle_NonExistentPedido_ShouldReturnFailure()
    {
        var pedidoId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdWithItemsAsync(pedidoId))
                       .ReturnsAsync((PedidoEntity?)null);
        var query = new GetPedidoByIdQuery { Id = pedidoId };

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Pedido.Should().BeNull();

        // Normaliza acentos antes da verificação
        var normalized = result.Error.Normalize(NormalizationForm.FormKD)
            .Replace(@"[^\u0000-\u007F]", "");
        normalized.Should().Contain("nao encontrado");
    }

    [Fact(DisplayName = "Exceção no repositório deve ser capturada e retornada como erro")]
    public async Task Handle_RepositoryThrowsException_ShouldReturnErrorResult()
    {
        var pedidoId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdWithItemsAsync(pedidoId))
                       .ThrowsAsync(new InvalidOperationException("Erro no banco"));
        var query = new GetPedidoByIdQuery { Id = pedidoId };

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Success.Should().BeFalse();
        result.Pedido.Should().BeNull();

        var normalized = result.Error.Normalize(NormalizationForm.FormKD)
            .Replace(@"[^\u0000-\u007F]", "");
        normalized.Should().Contain("Ocorreu um erro");
    }

    [Fact(DisplayName = "Pedido sem cliente deve retornar com ClienteId nulo")]
    public async Task Handle_PedidoWithoutCliente_ShouldReturnPedidoWithNullClienteId()
    {
        var pedidoItems = new List<PedidoItemValueObject>
        {
            PedidoItemValueObject.Create(Guid.NewGuid(), "Hambúrguer", 10.50m, 2)
        };
        var pedido = PedidoEntity.Create(null, pedidoItems);
        var pedidoId = pedido.Id;

        _repositoryMock.Setup(r => r.GetByIdWithItemsAsync(pedidoId))
                       .ReturnsAsync(pedido);

        var query = new GetPedidoByIdQuery { Id = pedidoId };
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Success.Should().BeTrue();
        result.Pedido.Should().NotBeNull();
        result.Pedido!.ClienteId.Should().BeNull();
        result.Pedido.ClienteName.Should().BeNull();
        result.Pedido.Items.Should().HaveCount(1);
    }

    [Fact(DisplayName = "Pedido com múltiplos itens retorna todos corretamente")]
    public async Task Handle_PedidoWithMultipleItems_ShouldReturnAllItems()
    {
        var clienteId = Guid.NewGuid();
        var pedidoItems = new List<PedidoItemValueObject>
        {
            PedidoItemValueObject.Create(Guid.NewGuid(), "Hambúrguer", 10.50m, 2),
            PedidoItemValueObject.Create(Guid.NewGuid(), "Batata Frita", 5.00m, 1)
        };
        var pedido = PedidoEntity.Create(clienteId, pedidoItems);
        var pedidoId = pedido.Id;

        _repositoryMock.Setup(r => r.GetByIdWithItemsAsync(pedidoId))
                       .ReturnsAsync(pedido);

        var query = new GetPedidoByIdQuery { Id = pedidoId };
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Success.Should().BeTrue();
        result.Pedido.Should().NotBeNull();
        result.Pedido!.Items.Should().HaveCount(2);
        result.Pedido.Items[0].ProdutoNome.Should().Be("Hambúrguer");
        result.Pedido.Items[1].ProdutoNome.Should().Be("Batata Frita");
    }

    [Fact(DisplayName = "Se algum item do pedido não existir, o handler deve retornar erro")]
    public async Task Handle_NonExistentItem_ShouldReturnFailure()
    {
        var pedidoId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdWithItemsAsync(pedidoId))
                       .ReturnsAsync((PedidoEntity?)null);

        var query = new GetPedidoByIdQuery { Id = pedidoId };
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Success.Should().BeFalse();
        result.Pedido.Should().BeNull();

        var normalized = result.Error.Normalize(NormalizationForm.FormKD)
            .Replace(@"[^\u0000-\u007F]", "");
        normalized.Should().Contain("nao encontrado");
    }

    [Fact(DisplayName = "Pedido com itens repetidos deve retornar todas as entradas separadas")]
    public async Task Handle_PedidoWithRepeatedItems_ShouldReturnItemsIndividually()
    {
        var clienteId = Guid.NewGuid();
        var produtoId = Guid.NewGuid();
        var pedidoItems = new List<PedidoItemValueObject>
        {
            PedidoItemValueObject.Create(produtoId, "Hambúrguer", 10.50m, 2),
            PedidoItemValueObject.Create(produtoId, "Hambúrguer", 10.50m, 3)
        };
        var pedido = PedidoEntity.Create(clienteId, pedidoItems);

        _repositoryMock.Setup(r => r.GetByIdWithItemsAsync(pedido.Id))
                       .ReturnsAsync(pedido);

        var query = new GetPedidoByIdQuery { Id = pedido.Id };
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Success.Should().BeTrue();
        result.Pedido!.Items.Should().HaveCount(2);
        result.Pedido.Items[0].Quant.Should().Be(2);
        result.Pedido.Items[1].Quant.Should().Be(3);
    }
}
