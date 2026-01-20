using MediatR;
using Microsoft.Extensions.Logging;
using Pedido.Application.DTOs;
using Pedido.Domain.Repositories;
using Pedido.Domain.ValueObjects;

namespace Pedido.Application.Queries
{
    public class GetPedidosQueryHandler
        : IRequestHandler<GetPedidosQuery, GetPedidosQueryResult>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly ILogger<GetPedidosQueryHandler> _logger;

        public GetPedidosQueryHandler(
            IPedidoRepository pedidoRepository,
            ILogger<GetPedidosQueryHandler> logger)
        {
            _pedidoRepository = pedidoRepository;
            _logger = logger;
        }

        public async Task<GetPedidosQueryResult> Handle(
            GetPedidosQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                // 1️⃣ Normalizar paginação
                var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;

                var pageSize = request.PageSize < 1
                    ? 10
                    : request.PageSize > 100
                        ? 100
                        : request.PageSize;

                // 2️⃣ Parse de status
                PedidoStatus? status = null;
                if (!string.IsNullOrWhiteSpace(request.Status) &&
                    Enum.TryParse(request.Status, true, out PedidoStatus parsedStatus))
                {
                    status = parsedStatus;
                }

                // 3️⃣ Buscar no repositório
                var (pedidos, totalCount) =
                    await _pedidoRepository.GetPedidosAsync(
                        pageNumber,
                        pageSize,
                        request.ClienteId,
                        status);

                // 4️⃣ Mapear DTO (SEM NullReference)
                var pedidosDto = pedidos.Select(p => new PedidoListItemDto
                {
                    Id = p.Id,
                    ClienteId = p.ClienteId,
                    ClienteNome = p.Cliente?.Nome != null
                        ? p.Cliente.Nome.Value
                        : null,
                    TotalPrice = p.TotalPrice,
                    Status = p.Status.ToString(),
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    ItemsCount = p.Items.Count
                }).ToList();

                return new GetPedidosQueryResult
                {
                    Pedidos = pedidosDto,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lista de pedidos");

                return new GetPedidosQueryResult
                {
                    Pedidos = new List<PedidoListItemDto>(),
                    PageNumber = 1,
                    PageSize = 10,
                    TotalCount = 0,
                };
            }
        }
    }
}
