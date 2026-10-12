using Microsoft.AspNetCore.Mvc;
using Pedidos.Api.Dtos;
using Pedidos.Api.Services;

namespace Pedidos.Api.Controllers;

[ApiController]
[Route("api/pedidos")]
public class PedidosController(IPedidoService pedidoService) : ControllerBase
{
    [HttpGet]
    public ActionResult<ResultadoPaginado<PedidoResponse>> Listar([FromQuery] PedidoQueryParams query)
        => Ok(pedidoService.Listar(query));

    [HttpGet("{id:guid}")]
    public ActionResult<PedidoResponse> ObterPorId(Guid id)
        => Ok(pedidoService.ObterPorId(id));

    [HttpPost]
    public ActionResult<PedidoResponse> Criar([FromBody] CriarPedidoRequest request)
    {
        var pedido = pedidoService.Criar(request);
        return CreatedAtAction(nameof(ObterPorId), new { id = pedido.Id }, pedido);
    }

    [HttpPut("{id:guid}")]
    public ActionResult<PedidoResponse> Atualizar(Guid id, [FromBody] AtualizarPedidoRequest request)
        => Ok(pedidoService.Atualizar(id, request));

    [HttpPatch("{id:guid}/status")]
    public ActionResult<PedidoResponse> AtualizarStatus(Guid id, [FromBody] AtualizarStatusRequest request)
        => Ok(pedidoService.AtualizarStatus(id, request.Status));

    [HttpDelete("{id:guid}")]
    public IActionResult Excluir(Guid id)
    {
        pedidoService.Excluir(id);
        return NoContent();
    }
}
