using MediatR;
using Microsoft.Extensions.Logging;
using Pedido.Application.DTOs;
using Pedido.Domain.Repositories;

namespace Pedido.Application.Queries
{
    public class GetPedidoByIdQueryHandler
        : IRequestHandler<GetPedidoByIdQuery, GetPedidoByIdQueryResult>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly ILogger<GetPedidoByIdQueryHandler> _logger;

        public GetPedidoByIdQueryHandler(
            IPedidoRepository pedidoRepository,
            ILogger<GetPedidoByIdQueryHandler> logger)
        {
            _pedidoRepository = pedidoRepository;
            _logger = logger;
        }

        public async Task<GetPedidoByIdQueryResult> Handle(
            GetPedidoByIdQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var pedido = await _pedidoRepository
                    .GetByIdWithItemsAsync(request.Id);

                if (pedido == null)
                {
                    return new GetPedidoByIdQueryResult
                    {
                        Success = false,
                        Error = $"Pedido com ID {request.Id} não encontrado."
                    };
                }

                var pedidoDto = new PedidoDto
                {
                    Id = pedido.Id,
                    ClienteId = pedido.ClienteId,

                    // 🔴 PONTO CRÍTICO CORRIGIDO
                    ClienteName = pedido.Cliente != null && pedido.Cliente.Nome != null
                        ? pedido.Cliente.Nome.Value
                        : null,

                    Status = pedido.Status.ToString(),
                    TotalPrice = pedido.TotalPrice,
                    CreatedAt = pedido.CreatedAt,
                    UpdatedAt = pedido.UpdatedAt,

                    Items = pedido.Items.Select(item => new PedidoItemDto
                    {
                        ProdutoId = item.ProdutoId,
                        ProdutoNome = item.ProdutoNome,
                        UnitPrice = item.UnitPrice,
                        Quant = item.Quant,
                        SubTotal = item.SubTotal
                    }).ToList()
                };

                return new GetPedidoByIdQueryResult
                {
                    Success = true,
                    Pedido = pedidoDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao buscar pedido {PedidoId}",
                    request.Id
                );

                return new GetPedidoByIdQueryResult
                {
                    Success = false,
                    Error = "Ocorreu um erro"
                };
            }
        }
    }
}
