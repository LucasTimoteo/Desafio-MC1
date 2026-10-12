# Cadastro de Pedidos — Desafio Técnico MC1

Aplicação de **Cadastro de Pedidos** (API REST + página web + Docker) desenvolvida como
resposta ao desafio técnico para Desenvolvedor Web Pleno .NET da MC1.

> **Status:** preparação do projeto. Esta etapa contém apenas o *harness* de
> desenvolvimento (instruções, documentação e critérios de validação).
> **A implementação da aplicação ainda será realizada.**

## Objetivo do desafio

Construir uma pequena aplicação de cadastro de pedidos em .NET, com API REST, uma página
web para consumir os dados e Docker. O enunciado valoriza as decisões, a organização e a
revisão do código — mais do que um produto completo. O tempo sugerido é de 2 a 3 horas.

Enunciado original (fonte de verdade): [docs/original-challenge.md](docs/original-challenge.md).

## Requisitos (resumo)

Um **Pedido** tem no mínimo: identificador, nome do cliente, descrição, valor total,
status (`Aberto`, `Pago`, `Cancelado`) e data de criação.

- API REST com criar, listar, obter por id, atualizar e excluir.
- Listagem com filtro por texto e paginação.
- Persistência em memória (sem banco), segura para requisições simultâneas.
- Carga inicial de 10.000 pedidos no startup.
- Página web que lista, filtra, pagina, cria, edita e exclui pedidos.
- Atualização automática da lista a cada 10 segundos, sem recarregar a página.
- Validações de entrada com mensagens claras na API e na tela.

Detalhamento e rastreabilidade em [docs/requisitos.md](docs/requisitos.md).

## Tecnologias planejadas

| Camada | Tecnologia |
|--------|-----------|
| Backend | C# / ASP.NET Core em **.NET 10** (Controllers + Swagger) |
| Frontend | **Angular** + **Angular Material** |
| Execução | **docker-compose** com 2 serviços (API + frontend) |
| Testes | xUnit (incluindo teste de concorrência) |

> As escolhas de frontend, versão do .NET, empacotamento Docker e testes são **decisões
> nossas**; o enunciado as deixa em aberto. Ver [docs/arquitetura.md](docs/arquitetura.md).

## Documentação do projeto

- [docs/original-challenge.md](docs/original-challenge.md) — enunciado original (fonte de verdade).
- [docs/requisitos.md](docs/requisitos.md) — requisitos explícitos vs. decisões propostas.
- [docs/arquitetura.md](docs/arquitetura.md) — arquitetura mínima e responsabilidades.
- [docs/criterios-de-aceitacao.md](docs/criterios-de-aceitacao.md) — checklist de verificação.
- [.github/copilot-instructions.md](.github/copilot-instructions.md) — instruções para desenvolvimento assistido por IA.

## Como rodar

> A ser preenchido quando a implementação for feita. A meta é subir a aplicação com um
> único comando:
>
> ```bash
> docker compose up
> ```

## Decisões tomadas

Resumidas na tabela de tecnologias acima e detalhadas em
[docs/arquitetura.md](docs/arquitetura.md).

## O que faria com mais tempo

> A ser preenchido durante a implementação (ex.: persistência real, autenticação,
> testes de frontend, CI).

## O que ficou incompleto

> A ser preenchido ao final da implementação, caso algo não seja concluído.

## Uso de IA

O relato de uso de IA está em [AI_USAGE.md](AI_USAGE.md).
