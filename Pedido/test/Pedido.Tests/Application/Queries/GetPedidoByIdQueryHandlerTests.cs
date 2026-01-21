using Pedido.Application.Queries;
using Pedido.Domain.Repositories;
using FluentAssertions;
using Moq;
using PedidoEntity = Pedido.Domain.Entities.Pedido;
using PedidoItemValueObject = Pedido.Domain.ValueObjects.PedidoItem;
using System.Globalization;
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

    private static string Normalize(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var c in normalized)
        {
            var category = CharUnicodeInfo.GetUnicodeCategory(c);
            if (category != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        return sb.ToString().ToLowerInvariant();
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

        _repositoryMock.Setup(r => r.GetByIdWithItemsAsync(pedido.Id))
                       .ReturnsAsync(pedido);

        var result = await _handler.Handle(
            new GetPedidoByIdQuery { Id = pedido.Id },
            CancellationToken.None);

        result.Success.Should().BeTrue();
        result.Pedido.Should().NotBeNull();
        result.Pedido!.Items.Should().HaveCount(2);
    }

    [Fact(DisplayName = "Pedido inexistente deve retornar erro de nao encontrado")]
    public async Task Handle_NonExistentPedido_ShouldReturnFailure()
    {
        var pedidoId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetByIdWithItemsAsync(pedidoId))
                       .ReturnsAsync((PedidoEntity?)null);

        var result = await _handler.Handle(
            new GetPedidoByIdQuery { Id = pedidoId },
            CancellationToken.None);

        result.Success.Should().BeFalse();
        Normalize(result.Error).Should().Contain("nao encontrado");
    }

    [Fact(DisplayName = "Exceção no repositório deve ser capturada e retornada como erro")]
    public async Task Handle_RepositoryThrowsException_ShouldReturnErrorResult()
    {
        var pedidoId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetByIdWithItemsAsync(pedidoId))
                       .ThrowsAsync(new InvalidOperationException("Erro no banco"));

        var result = await _handler.Handle(
            new GetPedidoByIdQuery { Id = pedidoId },
            CancellationToken.None);

        result.Success.Should().BeFalse();
        Normalize(result.Error).Should().Contain("ocorreu um erro");
    }

    [Fact(DisplayName = "Se algum item do pedido nao existir, o handler deve retornar erro")]
    public async Task Handle_NonExistentItem_ShouldReturnFailure()
    {
        var pedidoId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetByIdWithItemsAsync(pedidoId))
                       .ReturnsAsync((PedidoEntity?)null);

        var result = await _handler.Handle(
            new GetPedidoByIdQuery { Id = pedidoId },
            CancellationToken.None);

        result.Success.Should().BeFalse();
        Normalize(result.Error).Should().Contain("nao encontrado");
    }
}
