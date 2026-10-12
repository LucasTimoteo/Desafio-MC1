using Pedidos.Api.Dtos;
using Pedidos.Api.Infrastructure;

namespace Pedidos.Api.Services;

public class ProdutoService(IProdutoRepository produtoRepository) : IProdutoService
{
    public IReadOnlyList<ProdutoResponse> ListarTodos() =>
        produtoRepository.ObterTodos()
            .OrderBy(p => p.Nome)
            .Select(p => new ProdutoResponse(p.Id, p.Nome, p.Preco))
            .ToList();
}
