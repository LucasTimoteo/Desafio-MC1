namespace Pedidos.Api.Domain.Exceptions;

/// <summary>Lançada quando um recurso solicitado não existe. Mapeada para HTTP 404.</summary>
public class RegistroNaoEncontradoException(string mensagem) : Exception(mensagem);
