# Requisitos — Cadastro de Pedidos (MC1)

Fonte de verdade: [original-challenge.md](original-challenge.md). Este documento
organiza os requisitos extraídos do enunciado e separa, de forma explícita, o que o
desafio **exige** do que **nós decidimos**.

Legenda de rastreabilidade: o enunciado original tem 2 páginas.
`P1` = página 1, `P2` = página 2.

---

## 1. Requisitos funcionais explícitos (exigidos pelo enunciado)

Modelo de **Pedido** (P1): no mínimo `identificador`, `nome do cliente`, `descrição`,
`valor total`, `status` (`Aberto`, `Pago`, `Cancelado`) e `data de criação`. Campos
extras são permitidos "se fizer sentido".

| ID | Requisito | Origem | Como verificar |
|----|-----------|--------|----------------|
| RF1 | API REST com **criar, listar, obter por id, atualizar e excluir** pedidos | P1 | Chamar cada endpoint e conferir status HTTP e payload |
| RF2 | Listagem com **filtro por texto** e **paginação** | P1 | Requisição com termo de busca + parâmetros de página retorna subconjunto e total corretos |
| RF3 | **Persistência em memória**, sem banco; correto sob **requisições simultâneas** | P1 | Sem dependência de DB; teste de concorrência não corrompe estado |
| RF4 | **Carga inicial de 10.000 pedidos** gerada ao iniciar a aplicação | P1 | Logo após subir a API, a contagem total é 10.000 |
| RF5 | **Página web** que lista, filtra, pagina, **cria, edita e exclui** pedidos | P1 | Executar cada operação pela interface |
| RF6 | A página **atualiza a lista a cada 10 segundos**, sem recarregar | P1 | Observar refresh automático periódico sem reload da página |
| RF7 | **Validações básicas** de entrada, com **mensagens de erro claras na API e na tela** | P1 | Enviar dados inválidos: API responde 400 com mensagem; UI exibe mensagem legível |

## 2. Requisitos técnicos explícitos (exigidos pelo enunciado)

| ID | Requisito | Origem | Como verificar |
|----|-----------|--------|----------------|
| RT1 | Backend em **C# com ASP.NET Core (.NET 6 ou superior)** | P1 | Projeto compila em .NET ≥ 6 |
| RT2 | Frontend com **framework à escolha** + **biblioteca de estilização** | P1 | App frontend presente usando lib de estilo |
| RT3 | **Dockerfile** que suba a aplicação com **um único comando** | P1 | `build` + `run` sobem a aplicação com um comando |
| RT4 | **README curto**: como rodar, decisões tomadas e o que faria com mais tempo | P1 | README contém as três seções |
| RT5 | **Testes** são **opcionais**, mas contam a favor | P1 | Se houver, `dotnet test` passa |

> Observação: o enunciado afirma que **não há arquitetura "certa" esperada** (P1). A
> organização do código é decisão nossa, não requisito.

## 3. Itens obrigatórios da entrega (exigidos pelo enunciado)

| ID | Item | Origem | Como verificar |
|----|------|--------|----------------|
| EN1 | **Código-fonte completo** em repositório Git com acesso de leitura | P2 | Repositório acessível |
| EN2 | **Dockerfile funcional** | P2 | Build e execução bem-sucedidos |
| EN3 | **README.md** com instruções e decisões | P2 | Arquivo presente na raiz |
| EN4 | **AI_USAGE.md** (relato de ½ a 1 página) com os 4 pontos exigidos | P2 | Arquivo presente na raiz respondendo aos tópicos |
| EN5 | **Nenhuma** chave, senha ou credencial no código/configuração | P2 | Revisão: nenhum segredo versionado |
| EN6 | Registrar no README **o que ficou incompleto** e como faria | P2 | Seção correspondente no README |

Conteúdo obrigatório do **AI_USAGE.md** (P2):

- Quais ferramentas usou e em quais partes do desafio.
- Dois ou três prompts mais úteis e como montou o contexto.
- O que aceitou como veio, o que corrigiu e o que descartou, e por quê.
- Algum erro/sugestão ruim da IA e como identificou.

---

## 4. Decisões propostas (NÃO são exigências do enunciado)

Estas escolhas foram tomadas por nós para guiar a implementação. O enunciado deixa
cada ponto em aberto.

| ID | Decisão | Enunciado permite | Justificativa resumida |
|----|---------|-------------------|------------------------|
| DP1 | **Angular + Angular Material** como frontend | "framework que desejar" + "biblioteca de estilização" | Familiaridade e Material cobre a estilização |
| DP2 | **.NET 10** como versão alvo | ".NET 6 ou superior" | Versão mais recente disponível no ambiente |
| DP3 | **docker-compose com 2 serviços** (API + frontend) | "Dockerfile ... um único comando" | `docker compose up` cumpre o comando único com serviços separados |
| DP4 | **Incluir testes automatizados somente no backend** (xUnit) | testes são opcionais | Contam a favor e validam concorrência; frontend fica fora do escopo de testes |
| DP5 | Persistência com `ConcurrentDictionary` | memória, sem banco | Estrutura thread-safe simples |
| DP6 | Auto-refresh via **polling** (ex.: RxJS `interval`) | "sem recarregar a página" | Simples e suficiente para 10s |
| DP7 | Paginação por `page`/`pageSize`; filtro por termo em cliente/descrição | filtro + paginação | Convenção REST comum |
| DP8 | Toda entidade de domínio tem **`CreatedAt`** e **`UpdatedAt`** | campos extras permitidos | `CreatedAt` cumpre a "data de criação"; `UpdatedAt` registra a última alteração |
| DP9 | Atualização de status via **endpoint dedicado** (ex.: `PATCH /pedidos/{id}/status`) | não especificado | Separa a mudança de status do update completo |

> Os campos `CreatedAt`/`UpdatedAt` e qualquer outro campo adicional são decisão nossa
> (o enunciado permite, mas não exige). O `CreatedAt` atende ao requisito de "data de
> criação" exigido no enunciado.

---

## 5. Fora de escopo (não exigido)

- Banco de dados / persistência em disco.
- Autenticação e autorização.
- Deploy em nuvem, CI/CD, observabilidade avançada.
- Internacionalização além do português.
