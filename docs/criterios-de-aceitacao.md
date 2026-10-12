# Critérios de aceitação — Cadastro de Pedidos (MC1)

Checklist verificável para cada requisito obrigatório. Marque um item apenas quando ele
tiver sido efetivamente executado e validado. Rastreabilidade para
[requisitos.md](requisitos.md).

## 1. Critérios funcionais

### RF1 — CRUD de pedidos
- [ ] `POST` cria um pedido e retorna `201 Created` com o recurso/`id`.
- [ ] `GET` (lista) retorna `200` com os pedidos.
- [ ] `GET /{id}` retorna `200` com o pedido existente e `404` para id inexistente.
- [ ] `PUT`/`PATCH` atualiza um pedido existente e retorna `200`/`204`; `404` se não existir.
- [ ] `DELETE` remove um pedido existente e retorna `204`; `404` se não existir.
- [ ] A mudança de status é feita por endpoint dedicado (ex.: `PATCH /pedidos/{id}/status`).
- [ ] Toda entidade expõe `CreatedAt` e `UpdatedAt`; `UpdatedAt` muda ao atualizar o recurso.

### RF2 — Filtro e paginação
- [ ] A listagem aceita um termo de texto e retorna apenas pedidos que correspondem.
- [ ] A listagem aceita parâmetros de página e tamanho de página.
- [ ] A resposta informa o total de itens (para montar o paginador).
- [ ] Páginas diferentes retornam conjuntos diferentes e coerentes.

### RF3 — Memória e concorrência
- [ ] A aplicação funciona sem nenhum banco de dados.
- [ ] Um teste exercita requisições simultâneas (criar/atualizar/excluir) sem corromper o estado nem lançar exceções.

### RF4 — Carga inicial
- [ ] Ao iniciar a aplicação, existem exatamente 10.000 pedidos.
- [ ] Os dados gerados são variados o suficiente para demonstrar o filtro por texto.

### RF5 — Página web (CRUD completo)
- [ ] A página lista os pedidos consumindo a API.
- [ ] A página permite filtrar por texto.
- [ ] A página permite paginar.
- [ ] A página permite criar um pedido.
- [ ] A página permite editar um pedido.
- [ ] A página permite excluir um pedido.

### RF6 — Atualização automática
- [ ] A lista é atualizada automaticamente a cada ~10 segundos.
- [ ] A atualização ocorre sem recarregar a página.
- [ ] O refresh não descarta dados de um formulário aberto em edição.

### RF7 — Validações e mensagens
- [ ] Entradas inválidas na API retornam `400` com mensagem clara (ProblemDetails).
- [ ] A tela exibe mensagens de erro legíveis para o usuário.
- [ ] Campos obrigatórios e regras básicas (ex.: valor > 0, status válido) são validados.

## 2. Critérios de integração

- [ ] O frontend consome a API real (não dados mockados) em todas as operações.
- [ ] CORS está configurado para permitir o frontend.
- [ ] Os contratos (DTOs) usados pela UI correspondem aos retornados pela API.
- [ ] Erros da API são propagados e exibidos na UI.

## 3. Critérios técnicos

- [ ] Backend em ASP.NET Core sobre .NET 10, compila com `dotnet build`.
- [ ] Frontend em Angular com Angular Material, compila com `ng build`.
- [ ] `dotnet test` passa (testes **somente no backend**), incluindo o teste de concorrência.
- [ ] Nenhum segredo/credencial está versionado.

## 4. Critérios de entrega

- [ ] `docker compose up` sobe API e frontend com um único comando.
- [ ] `README.md` explica como rodar, decisões tomadas e o que faria com mais tempo.
- [ ] `README.md` registra o que ficou incompleto (se algo ficou).
- [ ] `AI_USAGE.md` presente na raiz, com ½ a 1 página, respondendo aos 4 pontos exigidos.
- [ ] Repositório Git com acesso de leitura contém todo o código-fonte.

> Nota: esta é uma lista de verificação para a fase de implementação. Nesta etapa de
> preparação do harness, os itens permanecem **não marcados** porque ainda não foram
> executados.
