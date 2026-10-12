namespace Pedidos.Api.Dtos;

/// <summary>Página de resultados com metadados para o paginador da UI.</summary>
public record ResultadoPaginado<T>(
    IReadOnlyList<T> Itens,
    int Total,
    int Pagina,
    int TamanhoPagina)
{
    public int TotalPaginas => TamanhoPagina > 0
        ? (int)Math.Ceiling(Total / (double)TamanhoPagina)
        : 0;
}
