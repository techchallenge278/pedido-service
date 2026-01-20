using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Pedido.Infrastructure.Data;
using Xunit;

namespace Pedido.Tests.Infrastructure.Data
{
    public class RepositoryBaseTests
    {
        private static PedidoDbContextTest CreateContext()
        {
            var options = new DbContextOptionsBuilder<PedidoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new PedidoDbContextTest(options);
        }

        [Fact]
        public async Task CreateAsync_ShouldPersistEntity()
        {
            var context = CreateContext();
            var repo = new FakeRepository(context);
            var entity = new FakeEntity(Guid.NewGuid());

            var result = await repo.CreateAsync(entity);

            result.Should().NotBeNull();
            context.Set<FakeEntity>().Should().HaveCount(1);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity()
        {
            var context = CreateContext();
            var repo = new FakeRepository(context);
            var entity = new FakeEntity(Guid.NewGuid());

            context.Set<FakeEntity>().Add(entity);
            await context.SaveChangesAsync();

            var result = await repo.GetByIdAsync(entity.Id);

            result.Should().NotBeNull();
            result!.Id.Should().Be(entity.Id);
        }

        [Fact]
        public async Task GetByIdAsync_WithCancellationToken_ShouldReturnEntity()
        {
            var context = CreateContext();
            var repo = new FakeRepository(context);
            var entity = new FakeEntity(Guid.NewGuid());

            context.Set<FakeEntity>().Add(entity);
            await context.SaveChangesAsync();

            var token = new CancellationTokenSource().Token;
            var result = await repo.GetByIdAsync(entity.Id, token);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnPaginatedData()
        {
            var context = CreateContext();
            var repo = new FakeRepository(context);

            for (int i = 0; i < 5; i++)
                context.Set<FakeEntity>().Add(new FakeEntity(Guid.NewGuid()));

            await context.SaveChangesAsync();

            var result = await repo.GetAllAsync(1, 2);

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEntity()
        {
            var context = CreateContext();
            var repo = new FakeRepository(context);
            var entity = new FakeEntity(Guid.NewGuid());

            context.Set<FakeEntity>().Add(entity);
            await context.SaveChangesAsync();

            await repo.UpdateAsync(entity);

            context.Set<FakeEntity>().Should().HaveCount(1);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveEntity()
        {
            var context = CreateContext();
            var repo = new FakeRepository(context);
            var entity = new FakeEntity(Guid.NewGuid());

            context.Set<FakeEntity>().Add(entity);
            await context.SaveChangesAsync();

            await repo.DeleteAsync(entity);

            context.Set<FakeEntity>().Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteAsync_WithCancellationToken_ShouldRemoveEntity()
        {
            var context = CreateContext();
            var repo = new FakeRepository(context);
            var entity = new FakeEntity(Guid.NewGuid());

            context.Set<FakeEntity>().Add(entity);
            await context.SaveChangesAsync();

            var token = new CancellationTokenSource().Token;
            await repo.DeleteAsync(entity, token);

            context.Set<FakeEntity>().Should().BeEmpty();
        }
    }
}
