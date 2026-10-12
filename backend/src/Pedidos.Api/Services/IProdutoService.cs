using Pedidos.Api.Dtos;

namespace Pedidos.Api.Services;

public interface IProdutoService
{
    IReadOnlyList<ProdutoResponse> ListarTodos();
}
