using MediatR;
using Pedido.Application.Commands;
using Pedido.Domain.Entities;
using Pedido.Domain.Exceptions;
using Pedido.Domain.Repositories;
using Pedido.Domain.ValueObjects;

namespace Pedido.Application.Commands
{
    public class CreatePedidoCommandHandler
        : IRequestHandler<CreatePedidoCommand, CreatePedidoCommandResult>
    {
        private readonly IPedidoRepository _orderRepository;

        public CreatePedidoCommandHandler(IPedidoRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<CreatePedidoCommandResult> Handle(
            CreatePedidoCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Items == null || !request.Items.Any())
                throw new PedidoDomainException("O pedido deve ter pelo menos um item.");

            // 🔥 CONSOLIDA ITENS PELO PRODUTO ID
            var pedidoItems = request.Items
                .GroupBy(i => i.ProdutoId)
                .Select(group =>
                {
                    var first = group.First();

                    var produtoNome = string.IsNullOrWhiteSpace(first.ProdutoNome)
                        ? "Produto Anônimo"
                        : first.ProdutoNome;

                    if (group.Any(i => i.UnitPrice <= 0))
                        throw new PedidoDomainException(
                            $"O preço unitário do produto {produtoNome} deve ser maior que zero.");

                    if (group.Any(i => i.Quant <= 0))
                        throw new PedidoDomainException(
                            $"A quantidade do produto {produtoNome} deve ser maior que zero.");

                    return PedidoItem.Create(
                        first.ProdutoId,
                        produtoNome,
                        first.UnitPrice,
                        group.Sum(i => i.Quant)
                    );
                })
                .ToList();

            var pedido = Pedido.Domain.Entities.Pedido.Create(
                request.ClienteId,
                pedidoItems
            );

            await _orderRepository.CreateAsync(pedido);

            return new CreatePedidoCommandResult
            {
                Id = pedido.Id,
                ClienteId = pedido.ClienteId,
                ClienteName = string.IsNullOrWhiteSpace(request.ClienteNome)
                    ? null
                    : request.ClienteNome,
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
