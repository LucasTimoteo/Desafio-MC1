using Pedidos.Api.Domain;
using Pedidos.Api.Dtos;

namespace Pedidos.Tests;

/// <summary>Valida que o store em memória se comporta corretamente sob requisições simultâneas (RF3).</summary>
public class ConcurrencyTests
{
    [Fact]
    public void Criar_EmParalelo_DeveManterTodosOsPedidos()
    {
        var (service, repo, produtos) = TestFactory.Criar();
        const int total = 2000;

        Parallel.For(0, total, i =>
            service.Criar(TestFactory.CriarRequest(produtos[i % produtos.Count].Id, $"Cliente {i}")));

        Assert.Equal(total, repo.Contar());
    }

    [Fact]
    public async Task OperacoesMistas_EmParalelo_NaoDevemLancarNemCorromper()
    {
        var (service, repo, produtos) = TestFactory.Criar();

        var criados = Enumerable.Range(0, 500)
            .Select(i => service.Criar(TestFactory.CriarRequest(produtos[0].Id, $"Cliente {i}")).Id)
            .ToList();

        var tarefas = new List<Task>();

        // Leituras concorrentes
        tarefas.AddRange(Enumerable.Range(0, 50).Select(_ => Task.Run(() =>
            service.Listar(new PedidoQueryParams { Pagina = 1, TamanhoPagina = 50 }))));

        // Atualizações de status concorrentes
        tarefas.AddRange(criados.Take(250).Select(id => Task.Run(() =>
            service.AtualizarStatus(id, StatusPedido.Pago))));

        // Exclusões concorrentes
        tarefas.AddRange(criados.Skip(250).Select(id => Task.Run(() =>
            service.Excluir(id))));

        var excecao = await Record.ExceptionAsync(() => Task.WhenAll(tarefas));

        Assert.Null(excecao);
        Assert.Equal(250, repo.Contar());
    }
}
