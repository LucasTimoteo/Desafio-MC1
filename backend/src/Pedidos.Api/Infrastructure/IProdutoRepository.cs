using Pedidos.Api.Domain;

namespace Pedidos.Api.Infrastructure;

public interface IProdutoRepository
{
    IReadOnlyCollection<Produto> ObterTodos();
    Produto? ObterPorId(Guid id);
    void AdicionarEmLote(IEnumerable<Produto> produtos);
}
