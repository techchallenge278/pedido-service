using MediatR;
using Pedido.Application.Commands;
using Pedido.Domain.Entities;
using Pedido.Domain.Exceptions;
using Pedido.Domain.Repositories;
using Pedido.Domain.ValueObjects;

namespace Pedido.Application.Commands
{
    public class CreatePedidoCommandHandler : IRequestHandler<CreatePedidoCommand, CreatePedidoCommandResult>
    {
        private readonly IPedidoRepository _orderRepository;

        public CreatePedidoCommandHandler(IPedidoRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<CreatePedidoCommandResult> Handle(CreatePedidoCommand request, CancellationToken cancellationToken)
        {
            if (request.Items == null || !request.Items.Any())
                throw new PedidoDomainException("O pedido deve ter pelo menos um item.");

            var pedidoItems = request.Items.Select(item =>
            {
                // Validar produtoNome
                var produtoNome = string.IsNullOrWhiteSpace(item.ProdutoNome)
                    ? "Produto Anônimo"
                    : item.ProdutoNome;

                // Validar preço unitário
                if (item.UnitPrice <= 0)
                    throw new PedidoDomainException($"O preço unitário do produto {produtoNome} deve ser maior que zero.");

                return PedidoItem.Create(
                    item.ProdutoId,
                    produtoNome,
                    item.UnitPrice,
                    item.Quant
                );
            }).ToList();

            // Criar pedido (ClienteId pode ser null)
            var pedido = Pedido.Domain.Entities.Pedido.Create(
                request.ClienteId,
                pedidoItems
            );

            await _orderRepository.CreateAsync(pedido);

            return new CreatePedidoCommandResult
            {
                Id = pedido.Id,
                ClienteId = pedido.ClienteId,
                ClienteName = request.ClienteNome,
                Status = pedido.Status.ToString(),
                TotalPrice = pedido.TotalPrice,
                CreatedAt = pedido.CreatedAt,
                Items = pedido.Items.Select(item => new CreatePedidoItemCommandResult
                {
                    Id = item.Id,
                    ProdutoId = item.ProdutoId,
                    ProdutoNome = item.ProdutoNome,
                    UnitPrice = item.UnitPrice,
                    Quant = item.Quant,
                    SubTotal = item.SubTotal
                }).ToList()
            };
        }
    }
}
