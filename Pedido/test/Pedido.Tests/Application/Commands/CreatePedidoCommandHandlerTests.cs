using Moq;
using Pedido.Application.Commands;
using Pedido.Domain.Entities;
using Pedido.Domain.Exceptions;
using Pedido.Domain.Repositories;
using Xunit;

namespace Pedido.Tests.Application.Commands
{
    public class CreatePedidoCommandHandlerTests
    {
        private readonly Mock<IPedidoRepository> _repoMock;
        private readonly CreatePedidoCommandHandler _handler;

        public CreatePedidoCommandHandlerTests()
        {
            _repoMock = new Mock<IPedidoRepository>();

            // CreateAsync retorna Task<Pedido>
            _repoMock
                .Setup(r => r.CreateAsync(It.IsAny<Pedido.Domain.Entities.Pedido>()))
                .ReturnsAsync((Pedido.Domain.Entities.Pedido pedido) => pedido);

            _handler = new CreatePedidoCommandHandler(_repoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenItemsIsNull()
        {
            var command = new CreatePedidoCommand
            {
                ClienteId = Guid.NewGuid(),
                Items = null!
            };

            await Assert.ThrowsAsync<PedidoDomainException>(
                () => _handler.Handle(command, default)
            );
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenItemsIsEmpty()
        {
            var command = new CreatePedidoCommand
            {
                ClienteId = Guid.NewGuid(),
                Items = new List<CreatePedidoItemCommand>()
            };

            await Assert.ThrowsAsync<PedidoDomainException>(
                () => _handler.Handle(command, default)
            );
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUnitPriceIsZeroOrNegative()
        {
            var command = new CreatePedidoCommand
            {
                ClienteId = Guid.NewGuid(),
                Items = new List<CreatePedidoItemCommand>
                {
                    new()
                    {
                        ProdutoId = Guid.NewGuid(),
                        ProdutoNome = "Teste",
                        UnitPrice = 0,
                        Quant = 1
                    }
                }
            };

            await Assert.ThrowsAsync<PedidoDomainException>(
                () => _handler.Handle(command, default)
            );
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenQuantIsZeroOrNegative()
        {
            var command = new CreatePedidoCommand
            {
                ClienteId = Guid.NewGuid(),
                Items = new List<CreatePedidoItemCommand>
                {
                    new()
                    {
                        ProdutoId = Guid.NewGuid(),
                        ProdutoNome = "Teste",
                        UnitPrice = 10,
                        Quant = 0
                    }
                }
            };

            await Assert.ThrowsAsync<PedidoDomainException>(
                () => _handler.Handle(command, default)
            );
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenGroupedUnitPriceIsZeroOrNegative()
        {
            var produtoId = Guid.NewGuid();

            var command = new CreatePedidoCommand
            {
                ClienteId = Guid.NewGuid(),
                Items = new List<CreatePedidoItemCommand>
                {
                    new()
                    {
                        ProdutoId = produtoId,
                        ProdutoNome = "Produto A",
                        UnitPrice = 10,
                        Quant = 1
                    },
                    new()
                    {
                        ProdutoId = produtoId,
                        ProdutoNome = "Produto A",
                        UnitPrice = 0,
                        Quant = 2
                    }
                }
            };

            await Assert.ThrowsAsync<PedidoDomainException>(
                () => _handler.Handle(command, default)
            );
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenGroupedQuantIsZeroOrNegative()
        {
            var produtoId = Guid.NewGuid();

            var command = new CreatePedidoCommand
            {
                ClienteId = Guid.NewGuid(),
                Items = new List<CreatePedidoItemCommand>
                {
                    new()
                    {
                        ProdutoId = produtoId,
                        ProdutoNome = "Produto A",
                        UnitPrice = 10,
                        Quant = 2
                    },
                    new()
                    {
                        ProdutoId = produtoId,
                        ProdutoNome = "Produto A",
                        UnitPrice = 10,
                        Quant = 0
                    }
                }
            };

            await Assert.ThrowsAsync<PedidoDomainException>(
                () => _handler.Handle(command, default)
            );
        }

        [Fact]
        public async Task Handle_ShouldUseProdutoAnonimo_WhenProdutoNomeIsNullOrEmpty()
        {
            var command = new CreatePedidoCommand
            {
                ClienteId = Guid.NewGuid(),
                Items = new List<CreatePedidoItemCommand>
                {
                    new()
                    {
                        ProdutoId = Guid.NewGuid(),
                        ProdutoNome = null,
                        UnitPrice = 10,
                        Quant = 1
                    }
                }
            };

            var result = await _handler.Handle(command, default);

            Assert.Single(result.Items);
            Assert.Equal("Produto Anônimo", result.Items[0].ProdutoNome);
        }

        [Fact]
        public async Task Handle_ShouldConsolidateItemsWithSameProdutoId()
        {
            var produtoId = Guid.NewGuid();

            var command = new CreatePedidoCommand
            {
                ClienteId = Guid.NewGuid(),
                Items = new List<CreatePedidoItemCommand>
                {
                    new()
                    {
                        ProdutoId = produtoId,
                        ProdutoNome = "Produto A",
                        UnitPrice = 10,
                        Quant = 2
                    },
                    new()
                    {
                        ProdutoId = produtoId,
                        ProdutoNome = "Produto A",
                        UnitPrice = 10,
                        Quant = 3
                    }
                }
            };

            var result = await _handler.Handle(command, default);

            Assert.Single(result.Items);
            Assert.Equal(5, result.Items[0].Quant);
        }

        [Fact]
        public async Task Handle_ShouldReturnValidResult_WhenCommandIsValid()
        {
            var command = new CreatePedidoCommand
            {
                ClienteId = Guid.NewGuid(),
                ClienteNome = "Amanda",
                Items = new List<CreatePedidoItemCommand>
                {
                    new()
                    {
                        ProdutoId = Guid.NewGuid(),
                        ProdutoNome = "Produto Teste",
                        UnitPrice = 10,
                        Quant = 2
                    }
                }
            };

            var result = await _handler.Handle(command, default);

            Assert.NotNull(result);
            Assert.Equal("Amanda", result.ClienteName);
            Assert.Equal(20, result.TotalPrice);
            Assert.Single(result.Items);
            Assert.Equal(20, result.Items[0].SubTotal);

            _repoMock.Verify(
                r => r.CreateAsync(It.IsAny<Pedido.Domain.Entities.Pedido>()),
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnNullClienteName_WhenClienteNomeIsNullOrEmpty()
        {
            var command = new CreatePedidoCommand
            {
                ClienteId = Guid.NewGuid(),
                ClienteNome = "",
                Items = new List<CreatePedidoItemCommand>
                {
                    new()
                    {
                        ProdutoId = Guid.NewGuid(),
                        ProdutoNome = "Produto Teste",
                        UnitPrice = 10,
                        Quant = 2
                    }
                }
            };

            var result = await _handler.Handle(command, default);

            Assert.Null(result.ClienteName);
        }

        [Fact]
        public async Task Handle_ShouldMapAllItemsCorrectly()
        {
            var command = new CreatePedidoCommand
            {
                ClienteId = Guid.NewGuid(),
                ClienteNome = "Amanda",
                Items = new List<CreatePedidoItemCommand>
                {
                    new()
                    {
                        ProdutoId = Guid.NewGuid(),
                        ProdutoNome = "Produto A",
                        UnitPrice = 5,
                        Quant = 1
                    },
                    new()
                    {
                        ProdutoId = Guid.NewGuid(),
                        ProdutoNome = "Produto B",
                        UnitPrice = 15,
                        Quant = 2
                    }
                }
            };

            var result = await _handler.Handle(command, default);

            Assert.Equal(2, result.Items.Count);
            Assert.Equal(5, result.Items[0].SubTotal);
            Assert.Equal(30, result.Items[1].SubTotal);
        }

        [Fact]
        public async Task Handle_ShouldConsolidateItemsWithSameProdutoIdAndKeepFirstUnitPrice()
        {
            var produtoId = Guid.NewGuid();

            var command = new CreatePedidoCommand
            {
                ClienteId = Guid.NewGuid(),
                Items = new List<CreatePedidoItemCommand>
                {
                    new()
                    {
                        ProdutoId = produtoId,
                        ProdutoNome = "Produto X",
                        UnitPrice = 10,
                        Quant = 1
                    },
                    new()
                    {
                        ProdutoId = produtoId,
                        ProdutoNome = "Produto X",
                        UnitPrice = 15,
                        Quant = 2
                    }
                }
            };

            var result = await _handler.Handle(command, default);

            Assert.Single(result.Items);
            Assert.Equal(3, result.Items[0].Quant);
            Assert.Equal(10, result.Items[0].UnitPrice);
        }
    }
}
