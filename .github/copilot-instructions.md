# Instruções do projeto — Cadastro de Pedidos (MC1)

Estas instruções são permanentes e valem para todo o repositório. Leia-as antes de
alterar qualquer arquivo. A fonte de verdade do escopo é
[docs/original-challenge.md](../docs/original-challenge.md). Em caso de dúvida, o
enunciado original prevalece sobre qualquer documento derivado.

## Antes de alterar código

Sempre leia a documentação relevante antes de implementar:

1. [docs/original-challenge.md](../docs/original-challenge.md) — enunciado original (fonte de verdade).
2. [docs/requisitos.md](../docs/requisitos.md) — requisitos explícitos vs. decisões propostas.
3. [docs/arquitetura.md](../docs/arquitetura.md) — arquitetura mínima e responsabilidades.
4. [docs/criterios-de-aceitacao.md](../docs/criterios-de-aceitacao.md) — checklist de verificação.

Não invente requisitos que o enunciado não exige. Não trate uma decisão técnica
nossa como se fosse exigência do desafio.

## Escopo do projeto

Aplicação de **Cadastro de Pedidos** com:

- API REST em .NET com CRUD, filtro por texto e paginação.
- Persistência **em memória** (sem banco de dados), segura para requisições concorrentes.
- Carga inicial de **10.000 pedidos** gerados no startup.
- Página web (Angular) que lista, filtra, pagina, cria, edita e exclui pedidos.
- Atualização automática da lista a cada **10 segundos**, sem recarregar a página.
- Validações de entrada com mensagens claras na API e na tela.
- Execução via Docker com um único comando.

Um **Pedido** tem no mínimo: identificador, nome do cliente, descrição, valor total,
status (`Aberto`, `Pago`, `Cancelado`) e data de criação. Toda entidade de domínio
(`Pedido`, `PedidoItem`, `Produto`) carrega `CreatedAt` e `UpdatedAt` — o `CreatedAt`
cumpre o requisito de "data de criação".

## Tecnologias previstas

- **Backend:** C# / ASP.NET Core em **.NET 10** (Controllers + OpenAPI/Swagger).
- **Frontend:** **Angular** com **Angular Material** (biblioteca de estilização).
- **Execução:** **docker-compose com 2 serviços** (API e frontend).
- **Testes:** xUnit **somente no backend** (incluindo um teste de concorrência).

> Estas são as decisões aprovadas para este projeto. Alternativas estão descritas em
> [docs/arquitetura.md](../docs/arquitetura.md).

## Padrões de código

- Mantenha a organização simples e condizente com um desafio de 2 a 3 horas. Evite
  over-engineering: nada de camadas, abstrações ou padrões que não agreguem valor direto.
- **Backend**
  - Separe responsabilidades em pastas claras (ex.: `Domain`, `Dtos`, `Services`,
    `Infrastructure`, `Controllers`). Não crie interfaces ou genéricos sem uso real.
  - Use `async`/`await` nos pontos de I/O quando fizer sentido.
  - Nomeie em português o domínio (ex.: `Pedido`, `StatusPedido`) de forma consistente.
  - Retorne os status HTTP corretos: `201` ao criar, `200` ao obter/listar, `204` ao
    excluir, `404` quando não encontrado, `400` para entrada inválida.
  - Valide a entrada nas DTOs (DataAnnotations) e devolva erros no formato ProblemDetails.
  - Toda entidade tem `CreatedAt` e `UpdatedAt`; atualize `UpdatedAt` a cada modificação.
  - Exponha a mudança de status em um endpoint dedicado (ex.: `PATCH /pedidos/{id}/status`).
- **Frontend**
  - Componentes standalone, serviço HTTP dedicado e tipos (interfaces) para os modelos.
  - Não bloqueie a edição em andamento quando o refresh automático de 10s ocorrer.
  - Exiba mensagens de erro vindas da API de forma legível.
- Respeite `.editorconfig` para formatação. Não reformate arquivos não relacionados.

## Segurança

- **Nunca** inclua chaves, senhas, tokens ou qualquer credencial no código, em arquivos
  de configuração ou no histórico do Git.
- Configure **CORS** de forma explícita para permitir o frontend consumir a API.
- Valide e normalize toda entrada do usuário antes de usá-la.
- Siga boas práticas contra as vulnerabilidades do OWASP Top 10 no que for aplicável.

## Testes

- Cubra a lógica de negócio do backend com testes unitários (filtro, paginação, CRUD e
  validações).
- Inclua ao menos **um teste de concorrência** que exercite o store em memória com
  múltiplas requisições simultâneas sem corromper o estado.
- Os testes devem passar com `dotnet test` antes de considerar a tarefa concluída.

## Critérios para concluir uma tarefa

Uma tarefa só está concluída quando:

1. Atende aos requisitos correspondentes em [docs/requisitos.md](../docs/requisitos.md).
2. Passa nos itens aplicáveis de [docs/criterios-de-aceitacao.md](../docs/criterios-de-aceitacao.md).
3. Compila sem erros (`dotnet build`, `ng build`) e os testes passam (`dotnet test`).
4. Não introduz segredos nem credenciais.
5. A documentação afetada (README, AI_USAGE) foi atualizada quando necessário.

Nunca marque como concluída uma etapa que não foi efetivamente executada e validada.
