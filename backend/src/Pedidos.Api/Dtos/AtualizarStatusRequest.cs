using System.ComponentModel.DataAnnotations;
using Pedidos.Api.Domain;

namespace Pedidos.Api.Dtos;

public class AtualizarStatusRequest
{
    [Required(ErrorMessage = "O status é obrigatório.")]
    [EnumDataType(typeof(StatusPedido), ErrorMessage = "Status inválido. Use Aberto, Pago ou Cancelado.")]
    public StatusPedido Status { get; set; }
}
