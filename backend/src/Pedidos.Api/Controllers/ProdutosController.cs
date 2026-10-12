using Microsoft.AspNetCore.Mvc;
using Pedidos.Api.Dtos;
using Pedidos.Api.Services;

namespace Pedidos.Api.Controllers;

[ApiController]
[Route("api/produtos")]
public class ProdutosController(IProdutoService produtoService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyList<ProdutoResponse>> Listar()
        => Ok(produtoService.ListarTodos());
}
