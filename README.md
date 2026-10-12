# Cadastro de Pedidos — Desafio Técnico MC1

Aplicação de **Cadastro de Pedidos** (API REST + página web + Docker) desenvolvida como
resposta ao desafio técnico para Desenvolvedor Web Pleno .NET da MC1.

> **Status:** implementação da aplicação disponível. Para uma explicação detalhada
> voltada à apresentação em entrevista, consulte [PROJECT_DESCRIPTION.md](PROJECT_DESCRIPTION.md).

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

## Tecnologias utilizadas

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
- [PROJECT_DESCRIPTION.md](PROJECT_DESCRIPTION.md) — explicação detalhada para apresentação em entrevista.
- [.github/copilot-instructions.md](.github/copilot-instructions.md) — instruções para desenvolvimento assistido por IA.

## Como rodar

Suba a API e o frontend com Docker Compose a partir da raiz:

```bash
docker compose up --build
```

Acesse a interface em <http://localhost:8080>. A API é acessada pelo frontend através
do proxy reverso do Nginx. Para detalhes da comunicação e da execução local, consulte
[PROJECT_DESCRIPTION.md](PROJECT_DESCRIPTION.md).

## Decisões tomadas

As escolhas estão resumidas na tabela de tecnologias acima e detalhadas em
[docs/arquitetura.md](docs/arquitetura.md) e no [guia de apresentação](PROJECT_DESCRIPTION.md).

## O que faria com mais tempo

Persistência durável, autenticação, testes de frontend, CI/CD e observabilidade
avançada não fazem parte do escopo implementado. Com mais tempo, eu adicionaria
persistência durável e testes de interface.

## O que ficou incompleto

Não há persistência em disco nem testes automatizados de frontend; esses pontos não
fazem parte das decisões de escopo descritas em [docs/requisitos.md](docs/requisitos.md).

## Uso de IA

O relato de uso de IA está em [AI_USAGE.md](AI_USAGE.md).
