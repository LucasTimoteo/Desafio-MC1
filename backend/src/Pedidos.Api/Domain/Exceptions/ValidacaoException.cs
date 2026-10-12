namespace Pedidos.Api.Domain.Exceptions;

/// <summary>Lançada em violações de regra de negócio. Mapeada para HTTP 400.</summary>
public class ValidacaoException(string mensagem) : Exception(mensagem);
