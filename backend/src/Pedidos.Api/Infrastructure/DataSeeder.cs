using Pedidos.Api.Domain;

namespace Pedidos.Api.Infrastructure;

/// <summary>Gera o catálogo de produtos e a carga inicial de 10.000 pedidos no startup.</summary>
public class DataSeeder(IProdutoRepository produtoRepository, IPedidoRepository pedidoRepository)
{
    public const int QuantidadePedidos = 10_000;
    private const int SeedAleatorio = 20260101;

    private static readonly (string Nome, decimal Preco)[] CatalogoProdutos =
    [
        ("Teclado mecânico", 350m),
        ("Mouse sem fio", 180m),
        ("Monitor 27\"", 1450m),
        ("Headset gamer", 420m),
        ("Webcam Full HD", 260m),
        ("Notebook 14\"", 4200m),
        ("Cadeira ergonômica", 1200m),
        ("Hub USB-C", 190m),
        ("SSD 1TB", 540m),
        ("Memória RAM 16GB", 380m),
        ("Carregador 65W", 220m),
        ("Suporte para notebook", 150m),
        ("Microfone condensador", 640m),
        ("Mousepad grande", 90m),
        ("Cabo HDMI 2m", 60m),
        ("Dock station", 980m),
        ("Impressora multifuncional", 1100m),
        ("Roteador Wi-Fi 6", 520m),
        ("Smartphone", 2800m),
        ("Tablet 10\"", 1900m),
    ];

    private static readonly string[] Nomes =
    [
        "Ana", "Bruno", "Carla", "Diego", "Eduarda", "Felipe", "Gabriela", "Henrique",
        "Isabela", "João", "Larissa", "Marcos", "Natália", "Otávio", "Patrícia",
        "Rafael", "Sofia", "Thiago", "Vanessa", "William",
    ];

    private static readonly string[] Sobrenomes =
    [
        "Silva", "Souza", "Oliveira", "Santos", "Pereira", "Lima", "Carvalho",
        "Ferreira", "Rodrigues", "Almeida", "Costa", "Gomes", "Martins", "Araújo",
    ];

    public void Popular()
    {
        var random = new Random(SeedAleatorio);

        var produtos = CriarProdutos();
        produtoRepository.AdicionarEmLote(produtos);

        var pedidos = CriarPedidos(random, produtos);
        pedidoRepository.AdicionarEmLote(pedidos);
    }

    private static List<Produto> CriarProdutos()
    {
        var agora = DateTime.UtcNow;
        return CatalogoProdutos
            .Select(p => new Produto
            {
                Nome = p.Nome,
                Preco = p.Preco,
                CreatedAt = agora,
                UpdatedAt = agora,
            })
            .ToList();
    }

    private static List<Pedido> CriarPedidos(Random random, List<Produto> produtos)
    {
        var agora = DateTime.UtcNow;
        var pedidos = new List<Pedido>(QuantidadePedidos);

        for (var i = 0; i < QuantidadePedidos; i++)
        {
            var dataCriacao = agora.AddMinutes(-random.Next(0, 60 * 24 * 180));
            var pedido = new Pedido
            {
                NomeCliente = $"{Nomes[random.Next(Nomes.Length)]} {Sobrenomes[random.Next(Sobrenomes.Length)]}",
                Descricao = $"Pedido #{i + 1:D5} gerado automaticamente",
                Status = SortearStatus(random),
                DataCriacao = dataCriacao,
                CreatedAt = dataCriacao,
                UpdatedAt = dataCriacao,
                Itens = CriarItens(random, produtos, dataCriacao),
            };

            pedidos.Add(pedido);
        }

        return pedidos;
    }

    private static List<PedidoItem> CriarItens(Random random, List<Produto> produtos, DateTime data)
    {
        var quantidadeItens = random.Next(1, 6);
        var itens = new List<PedidoItem>(quantidadeItens);

        for (var i = 0; i < quantidadeItens; i++)
        {
            var produto = produtos[random.Next(produtos.Count)];
            itens.Add(new PedidoItem
            {
                ProdutoId = produto.Id,
                ProdutoNome = produto.Nome,
                PrecoUnitario = produto.Preco,
                Quantidade = random.Next(1, 6),
                CreatedAt = data,
                UpdatedAt = data,
            });
        }

        return itens;
    }

    private static StatusPedido SortearStatus(Random random) => random.Next(0, 10) switch
    {
        < 6 => StatusPedido.Aberto,
        < 9 => StatusPedido.Pago,
        _ => StatusPedido.Cancelado,
    };
}
