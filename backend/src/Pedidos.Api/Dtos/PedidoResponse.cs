using Pedidos.Api.Domain;

namespace Pedidos.Api.Dtos;

public record PedidoResponse(
    Guid Id,
    string NomeCliente,
    string Descricao,
    decimal ValorTotal,
    StatusPedido Status,
    DateTime DataCriacao,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IReadOnlyList<PedidoItemResponse> Itens);
