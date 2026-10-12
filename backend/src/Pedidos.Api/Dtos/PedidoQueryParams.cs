using Pedidos.Api.Domain;

namespace Pedidos.Api.Dtos;

/// <summary>Parâmetros de consulta para a listagem de pedidos (filtro + paginação).</summary>
public class PedidoQueryParams
{
    private const int TamanhoMaximoPagina = 100;

    private int _pagina = 1;
    private int _tamanhoPagina = 20;

    /// <summary>Filtro por texto aplicado ao nome do cliente e à descrição.</summary>
    public string? Texto { get; set; }

    /// <summary>Filtro opcional por status.</summary>
    public StatusPedido? Status { get; set; }

    public int Pagina
    {
        get => _pagina;
        set => _pagina = value < 1 ? 1 : value;
    }

    public int TamanhoPagina
    {
        get => _tamanhoPagina;
        set => _tamanhoPagina = value switch
        {
            < 1 => 1,
            > TamanhoMaximoPagina => TamanhoMaximoPagina,
            _ => value
        };
    }
}
