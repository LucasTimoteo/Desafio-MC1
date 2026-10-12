namespace Pedidos.Api.Domain;

/// <summary>Item do catálogo que pode compor um pedido.</summary>
public class Produto : EntidadeBase
{
    public required string Nome { get; set; }
    public decimal Preco { get; set; }
}
