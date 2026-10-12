---
mode: 'agent'
description: 'Revisar a entrega final contra os critérios de aceitação, segurança e itens obrigatórios do desafio.'
---

# Revisar a entrega

Antes de começar, leia [docs/criterios-de-aceitacao.md](../../docs/criterios-de-aceitacao.md),
[docs/requisitos.md](../../docs/requisitos.md) e
[docs/original-challenge.md](../../docs/original-challenge.md).

## Escopo (apenas revisão)

- Conferir cada requisito obrigatório (RF1–RF7, RT1–RT5, EN1–EN6) contra o que foi entregue.
- Verificar o checklist de [docs/criterios-de-aceitacao.md](../../docs/criterios-de-aceitacao.md).
- Revisar segurança: ausência de segredos, validação de entrada, CORS, OWASP aplicável.
- Conferir `README.md` (como rodar, decisões, o que faria, incompletos) e `AI_USAGE.md`
  (preenchido com informações reais).
- **Não** adicionar novas funcionalidades; apenas apontar e, se solicitado, corrigir defeitos.

## Critérios de aceitação

- Todos os itens obrigatórios estão atendidos **ou** registrados como incompletos no README.
- `dotnet build`, `dotnet test` e `ng build` passam; `docker compose up` funciona.
- Nenhum segredo/credencial versionado.
- `AI_USAGE.md` não contém ferramentas, prompts, erros ou resultados inventados.

## Validação

- Rodar a suíte completa (`dotnet test`) e os builds.
- Subir via `docker compose up` e validar o fluxo ponta a ponta.
- Produzir uma lista de pendências classificadas por severidade.
