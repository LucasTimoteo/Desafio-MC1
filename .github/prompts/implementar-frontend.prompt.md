---
mode: 'agent'
description: 'Implementar a página web em Angular + Material que consome a API com CRUD, filtro, paginação e polling de 10s.'
---

# Implementar o frontend

Antes de começar, leia [.github/copilot-instructions.md](../copilot-instructions.md),
[docs/requisitos.md](../../docs/requisitos.md) e
[docs/arquitetura.md](../../docs/arquitetura.md). O backend deve estar disponível.

## Escopo (apenas o frontend)

- Aplicação **Angular** com **Angular Material** (biblioteca de estilização).
- Serviço HTTP e modelos (interfaces) alinhados aos contratos da API.
- Listagem com **filtro por texto** e **paginação** (RF5).
- Criar, editar e excluir pedidos pela interface (RF5).
- **Atualização automática** da lista a cada **10 segundos**, sem recarregar a página (RF6).
- Validações na tela com **mensagens de erro claras**, inclusive erros vindos da API (RF7 — lado UI).
- **Não** alterar o backend nem finalizar o Docker nesta tarefa.

## Critérios de aceitação

- Todas as operações usam a API real (sem dados mockados).
- O refresh de 10s **não** descarta um formulário aberto em edição.
- Entradas inválidas são bloqueadas e mensagens legíveis são exibidas.
- Itens correspondentes marcáveis em [docs/criterios-de-aceitacao.md](../../docs/criterios-de-aceitacao.md).

## Validação

- `ng build` sem erros.
- Teste manual de cada operação (listar, filtrar, paginar, criar, editar, excluir).
- Verificar o polling observando a lista atualizar sozinha a cada ~10s.
