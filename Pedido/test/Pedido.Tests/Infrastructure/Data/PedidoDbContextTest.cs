using Microsoft.EntityFrameworkCore;
using Pedido.Infrastructure.Data;

namespace Pedido.Tests.Infrastructure.Data
{
    public class PedidoDbContextTest : PedidoDbContext
    {
        public PedidoDbContextTest(DbContextOptions<PedidoDbContext> options)
            : base(options)
        {
        }

        public DbSet<FakeEntity> FakeEntities => Set<FakeEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FakeEntity>(builder =>
            {
                builder.HasKey(x => x.Id);
                builder.Property(x => x.CreatedAt).IsRequired();
                builder.Property(x => x.UpdatedAt);
            });
        }
    }
}
