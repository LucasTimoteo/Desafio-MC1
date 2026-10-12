using System.ComponentModel.DataAnnotations;

namespace Pedidos.Api.Dtos;

public class CriarPedidoItemRequest
{
    [Required(ErrorMessage = "O produto do item é obrigatório.")]
    public Guid ProdutoId { get; set; }

    [Range(1, 1000, ErrorMessage = "A quantidade deve estar entre 1 e 1000.")]
    public int Quantidade { get; set; }
}

public class CriarPedidoRequest
{
    [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
    [StringLength(120, MinimumLength = 2, ErrorMessage = "O nome do cliente deve ter entre 2 e 120 caracteres.")]
    public string NomeCliente { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(500, MinimumLength = 1, ErrorMessage = "A descrição deve ter até 500 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "O pedido deve conter ao menos um item.")]
    [MinLength(1, ErrorMessage = "O pedido deve conter ao menos um item.")]
    public List<CriarPedidoItemRequest> Itens { get; set; } = [];
}
