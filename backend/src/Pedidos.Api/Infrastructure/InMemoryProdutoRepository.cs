using System.Collections.Concurrent;
using Pedidos.Api.Domain;

namespace Pedidos.Api.Infrastructure;

/// <summary>Catálogo de produtos em memória thread-safe.</summary>
public class InMemoryProdutoRepository : IProdutoRepository
{
    private readonly ConcurrentDictionary<Guid, Produto> _produtos = new();

    public IReadOnlyCollection<Produto> ObterTodos() => _produtos.Values.ToList();

    public Produto? ObterPorId(Guid id) => _produtos.GetValueOrDefault(id);

    public void AdicionarEmLote(IEnumerable<Produto> produtos)
    {
        foreach (var produto in produtos)
        {
            _produtos[produto.Id] = produto;
        }
    }
}
