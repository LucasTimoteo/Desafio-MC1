namespace Pedidos.Api.Domain;

/// <summary>Base para entidades de domínio com identificador e carimbos de tempo.</summary>
public abstract class EntidadeBase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
