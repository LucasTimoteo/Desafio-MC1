namespace Pedidos.Api.Dtos;

public record PedidoItemResponse(
    Guid Id,
    Guid ProdutoId,
    string ProdutoNome,
    int Quantidade,
    decimal PrecoUnitario,
    decimal Subtotal);
