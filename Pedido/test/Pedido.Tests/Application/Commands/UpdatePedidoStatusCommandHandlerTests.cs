using Moq;
using Microsoft.Extensions.Logging;
using Pedido.Application.Commands;
using Pedido.Application.Common;
using Pedido.Domain.Entities;
using Pedido.Domain.Repositories;
using Pedido.Domain.Services;
using Pedido.Domain.ValueObjects;
using Xunit;

namespace Pedido.Tests.Application.Commands
{
    public class UpdatePedidoStatusCommandHandlerTests
    {
        private readonly Mock<IPedidoRepository> _repoMock;
        private readonly Mock<INotificationService> _notificationMock;
        private readonly Mock<ILogger<UpdatePedidoStatusCommandHandler>> _loggerMock;
        private readonly UpdatePedidoStatusCommandHandler _handler;

        public UpdatePedidoStatusCommandHandlerTests()
        {
            _repoMock = new Mock<IPedidoRepository>();
            _notificationMock = new Mock<INotificationService>();
            _loggerMock = new Mock<ILogger<UpdatePedidoStatusCommandHandler>>();

            _handler = new UpdatePedidoStatusCommandHandler(
                _repoMock.Object,
                _notificationMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldUpdateStatus_WhenPedidoExists()
        {
            // Arrange
            var pedidoItem = PedidoItem.Create(
                Guid.NewGuid(),
                "Produto Teste",
                10m,
                2
            );

            var pedido = Pedido.Domain.Entities.Pedido.Create(
                Guid.NewGuid(),
                new List<PedidoItem> { pedidoItem }
            );

            // ⚡ Ajuste: Inicializa como Processing para permitir Ready
            pedido.UpdateStatus(PedidoStatus.Processing);

            _repoMock
                .Setup(r => r.GetByIdWithItemsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(pedido);

            _repoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Pedido.Domain.Entities.Pedido>()))
                .Returns(Task.CompletedTask);

            _notificationMock
                .Setup(n => n.NotifyPedidoStatusChangeAsync(
                    It.IsAny<Pedido.Domain.Entities.Pedido>(),
                    It.IsAny<PedidoStatus>()
                ))
                .Returns(Task.CompletedTask);

            _notificationMock
                .Setup(n => n.NotifyPedidoReadyAsync(It.IsAny<Pedido.Domain.Entities.Pedido>()))
                .Returns(Task.CompletedTask);

            var command = new UpdatePedidoStatusCommand
            {
                Id = pedido.Id,
                Status = PedidoStatus.Ready.ToString()
            };

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(PedidoStatus.Ready.ToString(), result.Status);
            Assert.True(result.NotificationSent);

            _repoMock.Verify(
                r => r.UpdateAsync(It.IsAny<Pedido.Domain.Entities.Pedido>()),
                Times.Once
            );

            _notificationMock.Verify(
                n => n.NotifyPedidoStatusChangeAsync(
                    It.IsAny<Pedido.Domain.Entities.Pedido>(),
                    It.IsAny<PedidoStatus>()
                ),
                Times.Once
            );

            _notificationMock.Verify(
                n => n.NotifyPedidoReadyAsync(It.IsAny<Pedido.Domain.Entities.Pedido>()),
                Times.Once
            );
        }
    }
}
