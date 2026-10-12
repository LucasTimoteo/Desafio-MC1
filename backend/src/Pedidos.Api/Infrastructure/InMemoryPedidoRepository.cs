using System.Collections.Concurrent;
using Pedidos.Api.Domain;

namespace Pedidos.Api.Infrastructure;

/// <summary>Armazenamento em memória thread-safe de pedidos (sem banco de dados).</summary>
public class InMemoryPedidoRepository : IPedidoRepository
{
    private readonly ConcurrentDictionary<Guid, Pedido> _pedidos = new();

    public IReadOnlyCollection<Pedido> ObterTodos() => _pedidos.Values.ToList();

    public Pedido? ObterPorId(Guid id) => _pedidos.GetValueOrDefault(id);

    public void Adicionar(Pedido pedido) => _pedidos[pedido.Id] = pedido;

    public void AdicionarEmLote(IEnumerable<Pedido> pedidos)
    {
        foreach (var pedido in pedidos)
        {
            _pedidos[pedido.Id] = pedido;
        }
    }

    public bool Atualizar(Pedido pedido)
    {
        if (!_pedidos.ContainsKey(pedido.Id))
        {
            return false;
        }

        _pedidos[pedido.Id] = pedido;
        return true;
    }

    public bool Remover(Guid id) => _pedidos.TryRemove(id, out _);

    public int Contar() => _pedidos.Count;
}
