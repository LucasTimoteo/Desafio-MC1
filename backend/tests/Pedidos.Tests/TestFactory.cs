using Pedidos.Api.Domain;
using Pedidos.Api.Dtos;
using Pedidos.Api.Infrastructure;
using Pedidos.Api.Services;

namespace Pedidos.Tests;

/// <summary>Monta um PedidoService com repositórios em memória e um catálogo de produtos conhecido.</summary>
internal static class TestFactory
{
    public static (PedidoService Service, IPedidoRepository Pedidos, List<Produto> Produtos) Criar()
    {
        var produtoRepo = new InMemoryProdutoRepository();
        var agora = DateTime.UtcNow;
        var produtos = new List<Produto>
        {
            new() { Nome = "Produto A", Preco = 100m, CreatedAt = agora, UpdatedAt = agora },
            new() { Nome = "Produto B", Preco = 50m, CreatedAt = agora, UpdatedAt = agora },
        };
        produtoRepo.AdicionarEmLote(produtos);

        var pedidoRepo = new InMemoryPedidoRepository();
        var service = new PedidoService(pedidoRepo, produtoRepo);
        return (service, pedidoRepo, produtos);
    }

    public static CriarPedidoRequest CriarRequest(Guid produtoId, string cliente = "Cliente Teste", int quantidade = 2) => new()
    {
        NomeCliente = cliente,
        Descricao = "Pedido de teste",
        Itens = [new CriarPedidoItemRequest { ProdutoId = produtoId, Quantidade = quantidade }],
    };
}
