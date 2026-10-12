using Pedidos.Api.Domain;

namespace Pedidos.Api.Infrastructure;

public interface IPedidoRepository
{
    IReadOnlyCollection<Pedido> ObterTodos();
    Pedido? ObterPorId(Guid id);
    void Adicionar(Pedido pedido);
    void AdicionarEmLote(IEnumerable<Pedido> pedidos);
    bool Atualizar(Pedido pedido);
    bool Remover(Guid id);
    int Contar();
}
