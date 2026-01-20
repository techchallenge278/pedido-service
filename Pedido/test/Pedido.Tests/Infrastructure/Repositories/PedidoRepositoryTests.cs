using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Pedido.Domain.Entities;
using Pedido.Domain.ValueObjects;
using Pedido.Infrastructure.Data;
using Pedido.Infrastructure.Repositories;
using PedidoEntity = Pedido.Domain.Entities.Pedido;

namespace Pedido.Tests.Infrastructure.Repositories;

public class PedidoRepositoryTests : IDisposable
{
    private readonly PedidoDbContext _context;
    private readonly PedidoRepository _repository;

    public PedidoRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<PedidoDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new PedidoDbContext(options);
        _repository = new PedidoRepository(_context);
    }

    private async Task SeedPedidosAsync()
    {
        var clienteId1 = Guid.NewGuid();
        var clienteId2 = Guid.NewGuid();

        var pedido1 = PedidoEntity.Create(
            clienteId1,
            new List<PedidoItem>
            {
                PedidoItem.Create(Guid.NewGuid(), "Hamburguer", 25m, 1)
            });

        pedido1.UpdateStatus(PedidoStatus.Paid);

        var pedido2 = PedidoEntity.Create(
            clienteId2,
            new List<PedidoItem>
            {
                PedidoItem.Create(Guid.NewGuid(), "Pizza", 50m, 2)
            });

        pedido2.UpdateStatus(PedidoStatus.Pending);

        _context.Pedidos.AddRange(pedido1, pedido2);
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task AddPedido_ShouldPersistInDatabase()
    {
        var pedido = PedidoEntity.Create(
            Guid.NewGuid(),
            new List<PedidoItem>
            {
                PedidoItem.Create(Guid.NewGuid(), "Coxinha", 10m, 2)
            });

        await _repository.CreateAsync(pedido);

        var saved = await _context.Pedidos.FindAsync(pedido.Id);

        saved.Should().NotBeNull();
        saved!.Id.Should().Be(pedido.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnPedido()
    {
        await SeedPedidosAsync();
        var pedido = _context.Pedidos.First();

        var result = await _repository.GetByIdAsync(pedido.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(pedido.Id);
    }

    [Fact]
    public async Task GetByIdWithItemsAsync_ShouldReturnPedidoWithItems()
    {
        await SeedPedidosAsync();
        var pedido = _context.Pedidos.First();

        var result = await _repository.GetByIdWithItemsAsync(pedido.Id);

        result.Should().NotBeNull();
        result!.Items.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPagedPedidos()
    {
        await SeedPedidosAsync();

        var result = await _repository.GetAllAsync(1, 10);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByClienteIdAsync_ShouldFilterPedidos()
    {
        await SeedPedidosAsync();
        var clienteId = _context.Pedidos.First().ClienteId!.Value;

        var result = await _repository.GetByClienteIdAsync(clienteId);

        result.Should().NotBeEmpty();
        result.Should().OnlyContain(p => p.ClienteId == clienteId);
    }

    [Fact]
    public async Task GetByStatusAsync_ShouldFilterByStatus()
    {
        await SeedPedidosAsync();

        var result = await _repository.GetByStatusAsync(PedidoStatus.Paid);

        result.Should().NotBeEmpty();
        result.Should().OnlyContain(p => p.Status == PedidoStatus.Paid);
    }

    [Fact]
    public async Task CustomerHasPedidosAsync_ShouldReturnTrue()
    {
        await SeedPedidosAsync();
        var clienteId = _context.Pedidos.First().ClienteId!.Value;

        var hasPedidos = await _repository.CustomerHasPedidosAsync(clienteId);

        hasPedidos.Should().BeTrue();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
