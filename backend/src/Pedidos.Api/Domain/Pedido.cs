namespace Pedidos.Api.Domain;

public class Pedido : EntidadeBase
{
    public required string NomeCliente { get; set; }
    public required string Descricao { get; set; }
    public StatusPedido Status { get; set; } = StatusPedido.Aberto;
    public DateTime DataCriacao { get; set; }
    public List<PedidoItem> Itens { get; set; } = [];

    public decimal ValorTotal => Itens.Sum(i => i.Subtotal);
}
