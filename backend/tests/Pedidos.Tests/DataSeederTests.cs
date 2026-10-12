using Pedidos.Api.Infrastructure;

namespace Pedidos.Tests;

public class DataSeederTests
{
    [Fact]
    public void Popular_DeveGerar10MilPedidos_E_Produtos()
    {
        var produtoRepo = new InMemoryProdutoRepository();
        var pedidoRepo = new InMemoryPedidoRepository();
        var seeder = new DataSeeder(produtoRepo, pedidoRepo);

        seeder.Popular();

        Assert.Equal(DataSeeder.QuantidadePedidos, pedidoRepo.Contar());
        Assert.NotEmpty(produtoRepo.ObterTodos());
        Assert.All(pedidoRepo.ObterTodos(), p =>
        {
            Assert.NotEmpty(p.Itens);
            Assert.True(p.ValorTotal > 0);
        });
    }
}
