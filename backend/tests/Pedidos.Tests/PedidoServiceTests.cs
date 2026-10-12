using Pedidos.Api.Domain;
using Pedidos.Api.Domain.Exceptions;
using Pedidos.Api.Dtos;

namespace Pedidos.Tests;

public class PedidoServiceTests
{
    [Fact]
    public void Criar_DeveCalcularValorTotalESalvarComTimestamps()
    {
        var (service, _, produtos) = TestFactory.Criar();

        var response = service.Criar(TestFactory.CriarRequest(produtos[0].Id, quantidade: 3));

        Assert.Equal(300m, response.ValorTotal); // 100 * 3
        Assert.Equal(StatusPedido.Aberto, response.Status);
        Assert.Single(response.Itens);
        Assert.Equal(response.CreatedAt, response.UpdatedAt);
        Assert.NotEqual(default, response.CreatedAt);
    }

    [Fact]
    public void Criar_ComProdutoInexistente_DeveLancarValidacao()
    {
        var (service, _, _) = TestFactory.Criar();

        var request = TestFactory.CriarRequest(Guid.NewGuid());

        Assert.Throws<ValidacaoException>(() => service.Criar(request));
    }

    [Fact]
    public void ObterPorId_Inexistente_DeveLancarNaoEncontrado()
    {
        var (service, _, _) = TestFactory.Criar();

        Assert.Throws<RegistroNaoEncontradoException>(() => service.ObterPorId(Guid.NewGuid()));
    }

    [Fact]
    public void Listar_ComFiltroTexto_DeveRetornarApenasCorrespondentes()
    {
        var (service, _, produtos) = TestFactory.Criar();
        service.Criar(TestFactory.CriarRequest(produtos[0].Id, "Maria Silva"));
        service.Criar(TestFactory.CriarRequest(produtos[0].Id, "João Souza"));
        service.Criar(TestFactory.CriarRequest(produtos[0].Id, "Mariana Costa"));

        var resultado = service.Listar(new PedidoQueryParams { Texto = "mari" });

        Assert.Equal(2, resultado.Total);
        Assert.All(resultado.Itens, p => Assert.Contains("mari", p.NomeCliente, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Listar_ComFiltroStatus_DeveFiltrarPorStatus()
    {
        var (service, _, produtos) = TestFactory.Criar();
        var pago = service.Criar(TestFactory.CriarRequest(produtos[0].Id, "Cliente Pago"));
        service.Criar(TestFactory.CriarRequest(produtos[0].Id, "Cliente Aberto"));
        service.AtualizarStatus(pago.Id, StatusPedido.Pago);

        var resultado = service.Listar(new PedidoQueryParams { Status = StatusPedido.Pago });

        Assert.Equal(1, resultado.Total);
        Assert.Equal(StatusPedido.Pago, resultado.Itens[0].Status);
    }

    [Fact]
    public void Listar_ComPaginacao_DeveRespeitarPaginaETamanho()
    {
        var (service, _, produtos) = TestFactory.Criar();
        for (var i = 0; i < 25; i++)
        {
            service.Criar(TestFactory.CriarRequest(produtos[0].Id, $"Cliente {i:D2}"));
        }

        var pagina2 = service.Listar(new PedidoQueryParams { Pagina = 2, TamanhoPagina = 10 });

        Assert.Equal(25, pagina2.Total);
        Assert.Equal(10, pagina2.Itens.Count);
        Assert.Equal(3, pagina2.TotalPaginas);
    }

    [Fact]
    public void Atualizar_DeveAlterarCamposEItensEUpdatedAt()
    {
        var (service, _, produtos) = TestFactory.Criar();
        var criado = service.Criar(TestFactory.CriarRequest(produtos[0].Id));

        var atualizado = service.Atualizar(criado.Id, new AtualizarPedidoRequest
        {
            NomeCliente = "Novo Nome",
            Descricao = "Nova descrição",
            Itens = [new CriarPedidoItemRequest { ProdutoId = produtos[1].Id, Quantidade = 4 }],
        });

        Assert.Equal("Novo Nome", atualizado.NomeCliente);
        Assert.Equal(200m, atualizado.ValorTotal); // 50 * 4
        Assert.True(atualizado.UpdatedAt >= criado.UpdatedAt);
        Assert.Equal(criado.CreatedAt, atualizado.CreatedAt);
    }

    [Fact]
    public void AtualizarStatus_DeveAlterarStatus()
    {
        var (service, _, produtos) = TestFactory.Criar();
        var criado = service.Criar(TestFactory.CriarRequest(produtos[0].Id));

        var atualizado = service.AtualizarStatus(criado.Id, StatusPedido.Cancelado);

        Assert.Equal(StatusPedido.Cancelado, atualizado.Status);
    }

    [Fact]
    public void Excluir_DeveRemover_E_Inexistente_DeveLancar()
    {
        var (service, repo, produtos) = TestFactory.Criar();
        var criado = service.Criar(TestFactory.CriarRequest(produtos[0].Id));

        service.Excluir(criado.Id);

        Assert.Equal(0, repo.Contar());
        Assert.Throws<RegistroNaoEncontradoException>(() => service.Excluir(criado.Id));
    }
}
