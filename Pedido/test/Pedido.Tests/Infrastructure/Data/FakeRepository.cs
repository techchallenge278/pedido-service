using Pedido.Infrastructure.Data;

namespace Pedido.Tests.Infrastructure.Data
{
    public class FakeRepository : RepositoryBase<FakeEntity>
    {
        public FakeRepository(PedidoDbContext context) : base(context)
        {
        }
    }
}
