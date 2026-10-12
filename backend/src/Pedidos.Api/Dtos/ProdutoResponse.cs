namespace Pedidos.Api.Dtos;

public record ProdutoResponse(
    Guid Id,
    string Nome,
    decimal Preco);
