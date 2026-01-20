namespace Pedido.Application.DTOs
{
    public class PedidoItemDto
    {
        public required Guid ProdutoId { get; set; }
        public required string ProdutoNome { get; set; }
        public required decimal UnitPrice { get; set; }
        public required int Quant { get; set; }
        public required decimal SubTotal { get; set; }
        public string? Observation { get; set; }
    }
}
