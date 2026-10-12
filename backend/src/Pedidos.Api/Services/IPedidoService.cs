using Pedidos.Api.Domain;
using Pedidos.Api.Dtos;

namespace Pedidos.Api.Services;

public interface IPedidoService
{
    ResultadoPaginado<PedidoResponse> Listar(PedidoQueryParams query);
    PedidoResponse ObterPorId(Guid id);
    PedidoResponse Criar(CriarPedidoRequest request);
    PedidoResponse Atualizar(Guid id, AtualizarPedidoRequest request);
    PedidoResponse AtualizarStatus(Guid id, StatusPedido status);
    void Excluir(Guid id);
}
