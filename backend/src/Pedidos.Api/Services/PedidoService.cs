using Pedidos.Api.Domain;
using Pedidos.Api.Domain.Exceptions;
using Pedidos.Api.Dtos;
using Pedidos.Api.Infrastructure;

namespace Pedidos.Api.Services;

public class PedidoService(IPedidoRepository pedidoRepository, IProdutoRepository produtoRepository) : IPedidoService
{
    public ResultadoPaginado<PedidoResponse> Listar(PedidoQueryParams query)
    {
        IEnumerable<Pedido> pedidos = pedidoRepository.ObterTodos();

        if (!string.IsNullOrWhiteSpace(query.Texto))
        {
            var termo = query.Texto.Trim();
            pedidos = pedidos.Where(p =>
                p.NomeCliente.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
                p.Descricao.Contains(termo, StringComparison.OrdinalIgnoreCase));
        }

        if (query.Status is not null)
        {
            pedidos = pedidos.Where(p => p.Status == query.Status);
        }

        var ordenados = pedidos
            .OrderByDescending(p => p.DataCriacao)
            .ToList();

        var total = ordenados.Count;
        var itens = ordenados
            .Skip((query.Pagina - 1) * query.TamanhoPagina)
            .Take(query.TamanhoPagina)
            .Select(MapToResponse)
            .ToList();

        return new ResultadoPaginado<PedidoResponse>(itens, total, query.Pagina, query.TamanhoPagina);
    }

    public PedidoResponse ObterPorId(Guid id) => MapToResponse(BuscarOuFalhar(id));

    public PedidoResponse Criar(CriarPedidoRequest request)
    {
        var agora = DateTime.UtcNow;
        var pedido = new Pedido
        {
            NomeCliente = request.NomeCliente.Trim(),
            Descricao = request.Descricao.Trim(),
            Status = StatusPedido.Aberto,
            DataCriacao = agora,
            CreatedAt = agora,
            UpdatedAt = agora,
            Itens = ConstruirItens(request.Itens, agora),
        };

        pedidoRepository.Adicionar(pedido);
        return MapToResponse(pedido);
    }

    public PedidoResponse Atualizar(Guid id, AtualizarPedidoRequest request)
    {
        var pedido = BuscarOuFalhar(id);
        var agora = DateTime.UtcNow;

        pedido.NomeCliente = request.NomeCliente.Trim();
        pedido.Descricao = request.Descricao.Trim();
        pedido.Itens = ConstruirItens(request.Itens, agora);
        pedido.UpdatedAt = agora;

        pedidoRepository.Atualizar(pedido);
        return MapToResponse(pedido);
    }

    public PedidoResponse AtualizarStatus(Guid id, StatusPedido status)
    {
        var pedido = BuscarOuFalhar(id);
        pedido.Status = status;
        pedido.UpdatedAt = DateTime.UtcNow;

        pedidoRepository.Atualizar(pedido);
        return MapToResponse(pedido);
    }

    public void Excluir(Guid id)
    {
        if (!pedidoRepository.Remover(id))
        {
            throw new RegistroNaoEncontradoException($"Pedido {id} não encontrado.");
        }
    }

    private Pedido BuscarOuFalhar(Guid id) =>
        pedidoRepository.ObterPorId(id)
        ?? throw new RegistroNaoEncontradoException($"Pedido {id} não encontrado.");

    private List<PedidoItem> ConstruirItens(IReadOnlyCollection<CriarPedidoItemRequest> itens, DateTime agora)
    {
        if (itens.Count == 0)
        {
            throw new ValidacaoException("O pedido deve conter ao menos um item.");
        }

        var construidos = new List<PedidoItem>(itens.Count);
        foreach (var item in itens)
        {
            if (item.ProdutoId == Guid.Empty)
            {
                throw new ValidacaoException("O produto do item é obrigatório.");
            }

            var produto = produtoRepository.ObterPorId(item.ProdutoId)
                ?? throw new ValidacaoException($"Produto {item.ProdutoId} não encontrado.");

            construidos.Add(new PedidoItem
            {
                ProdutoId = produto.Id,
                ProdutoNome = produto.Nome,
                PrecoUnitario = produto.Preco,
                Quantidade = item.Quantidade,
                CreatedAt = agora,
                UpdatedAt = agora,
            });
        }

        return construidos;
    }

    private static PedidoResponse MapToResponse(Pedido pedido) => new(
        pedido.Id,
        pedido.NomeCliente,
        pedido.Descricao,
        pedido.ValorTotal,
        pedido.Status,
        pedido.DataCriacao,
        pedido.CreatedAt,
        pedido.UpdatedAt,
        pedido.Itens
            .Select(i => new PedidoItemResponse(i.Id, i.ProdutoId, i.ProdutoNome, i.Quantidade, i.PrecoUnitario, i.Subtotal))
            .ToList());
}
