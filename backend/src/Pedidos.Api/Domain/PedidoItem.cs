namespace Pedidos.Api.Domain;

/// <summary>Linha de um pedido. Guarda um instantâneo do produto no momento da compra.</summary>
public class PedidoItem : EntidadeBase
{
    public Guid ProdutoId { get; set; }
    public required string ProdutoNome { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal => PrecoUnitario * Quantidade;
}
