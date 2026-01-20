using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Pedido.Api.Controllers;
using Pedido.Application.Commands;
using Pedido.Application.DTOs;
using Pedido.Application.Queries;
using Xunit;

namespace Pedido.Tests.Api.Controllers
{
    public class PedidosControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<PedidosController>> _loggerMock;
        private readonly PedidosController _controller;

        public PedidosControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<PedidosController>>();

            _controller = new PedidosController(
                _mediatorMock.Object,
                _loggerMock.Object
            );
        }

        #region Create

        [Fact]
        public async Task Create_ShouldReturnCreated_WhenRequestIsValid()
        {
            var dto = new CreatePedidoDto
            {
                ClienteId = null,
                ClienteNome = "",
                Items = new List<CreatePedidoItemDto>
                {
                    new CreatePedidoItemDto
                    {
                        ProdutoId = Guid.NewGuid(),
                        ProdutoNome = "",
                        UnitPrice = 10,
                        Quant = 1
                    }
                }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreatePedidoCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new CreatePedidoCommandResult
                {
                    Id = Guid.NewGuid()
                }));

            var result = await _controller.Create(dto);

            result.Should().BeOfType<CreatedAtActionResult>();
        }

        #endregion

        #region GetAll

        [Fact]
        public async Task GetAll_ShouldReturnOk()
        {
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetPedidosQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new GetPedidosQueryResult
                {
                    Pedidos = new List<PedidoListItemDto>(),
                    PageNumber = 1,
                    PageSize = 10,
                }));

            var result = await _controller.GetAll(null, null, 1, 10);

            result.Should().BeOfType<OkObjectResult>();
        }

        #endregion

        #region GetById

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenPedidoDoesNotExist()
        {
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetPedidoByIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new GetPedidoByIdQueryResult
                {
                    Success = false,
                    Error = "Pedido não encontrado"
                }));

            var result = await _controller.GetById(Guid.NewGuid());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenPedidoExists()
        {
            var pedidoDto = new PedidoDto
            {
                Id = Guid.NewGuid(),
                Status = "Criado",
                CreatedAt = DateTime.UtcNow
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetPedidoByIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new GetPedidoByIdQueryResult
                {
                    Success = true,
                    Pedido = pedidoDto
                }));

            var result = await _controller.GetById(pedidoDto.Id);

            result.Should().BeOfType<OkObjectResult>();
        }

        #endregion

        #region UpdateStatus

        [Fact]
        public async Task UpdateStatus_ShouldReturnOk()
        {
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdatePedidoStatusCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new UpdatePedidoStatusCommandResult
                {
                    Id = Guid.NewGuid(),
                    Status = "Pago",
                    UpdatedAt = DateTime.UtcNow,
                    NotificationSent = true
                }));

            var dto = new UpdatePedidoStatusDto
            {
                Status = "Pago"
            };

            var result = await _controller.UpdateStatus(Guid.NewGuid(), dto);

            result.Should().BeOfType<OkObjectResult>();
        }

        #endregion

        #region GetStatus

        [Fact]
        public async Task GetStatus_ShouldReturnNotFound_WhenPedidoDoesNotExist()
        {
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetPedidoByIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new GetPedidoByIdQueryResult
                {
                    Success = false
                }));

            var result = await _controller.GetStatus(Guid.NewGuid());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetStatus_ShouldReturnOk_WhenPedidoExists()
        {
            var pedidoDto = new PedidoDto
            {
                Id = Guid.NewGuid(),
                Status = "Criado",
                CreatedAt = DateTime.UtcNow
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetPedidoByIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new GetPedidoByIdQueryResult
                {
                    Success = true,
                    Pedido = pedidoDto
                }));

            var result = await _controller.GetStatus(pedidoDto.Id);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var statusDto = okResult.Value.Should().BeOfType<PedidoStatusDto>().Subject;

            statusDto.PedidoId.Should().Be(pedidoDto.Id);
            statusDto.Status.Should().Be(pedidoDto.Status);
        }

        #endregion
    }
}
