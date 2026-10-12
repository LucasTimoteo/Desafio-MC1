---
mode: 'agent'
description: 'Implementar a API REST de pedidos em ASP.NET Core (.NET 10) com memória, filtro, paginação e seed.'
---

# Implementar o backend

Antes de começar, leia [.github/copilot-instructions.md](../copilot-instructions.md),
[docs/requisitos.md](../../docs/requisitos.md) e
[docs/arquitetura.md](../../docs/arquitetura.md).

## Escopo (apenas o backend)

- API ASP.NET Core em **.NET 10** com Controllers e Swagger/OpenAPI.
- Entidades de domínio (`Pedido`, `PedidoItem`, `Produto`) e enum `StatusPedido`
  (`Aberto`, `Pago`, `Cancelado`). O `Pedido` tem id, nome do cliente, descrição, valor
  total, status e data de criação.
- Toda entidade tem `CreatedAt` e `UpdatedAt` (`CreatedAt` cumpre a "data de criação";
  atualize `UpdatedAt` a cada modificação).
- CRUD completo: criar, listar, obter por id, atualizar, excluir (RF1).
- Atualização de status por **endpoint dedicado** (ex.: `PATCH /pedidos/{id}/status`).
- Listagem com filtro por texto e paginação, retornando o total (RF2).
- Store **em memória** thread-safe (`ConcurrentDictionary`), sem banco (RF3).
- Seeder de **10.000 pedidos** no startup, com dados variados (RF4).
- Validações nas DTOs com respostas `400`/ProblemDetails (RF7 — lado API).
- CORS configurado para o frontend.
- **Não** implementar o frontend nem finalizar o Docker nesta tarefa.

## Critérios de aceitação

- Status HTTP corretos: `201` criar, `200` obter/listar, `204` excluir, `404` inexistente, `400` inválido.
- Após iniciar, a contagem total de pedidos é 10.000.
- Filtro e paginação retornam subconjuntos coerentes com o total informado.
- Nenhum segredo/credencial no código ou configuração.
- Itens correspondentes marcáveis em [docs/criterios-de-aceitacao.md](../../docs/criterios-de-aceitacao.md).

## Validação

- `dotnet build` sem erros e `dotnet test` passando (inclua ou atualize testes de
  unidade e um teste de concorrência).
- Exercite os endpoints via Swagger ou arquivo `.http`, incluindo casos inválidos.
