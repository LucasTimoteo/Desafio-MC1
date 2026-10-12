---
mode: 'agent'
description: 'Configurar Docker (docker-compose com 2 serviços) para subir API e frontend com um único comando.'
---

# Configurar o Docker

Antes de começar, leia [docs/arquitetura.md](../../docs/arquitetura.md) e
[docs/requisitos.md](../../docs/requisitos.md). Backend e frontend já devem existir.

## Escopo (apenas Docker)

- `Dockerfile` para a **API** (.NET 10) usando build multi-stage.
- `Dockerfile` para o **frontend** Angular (build + servidor estático, ex.: Nginx).
- `docker-compose.yml` com **2 serviços** (`api` e `frontend`) e a rede entre eles.
- Garantir que a aplicação suba com **um único comando**: `docker compose up`.
- `.dockerignore` adequado para cada contexto de build.
- **Não** colocar segredos/credenciais em nenhum arquivo.

## Critérios de aceitação

- `docker compose up` sobe API e frontend sem passos manuais adicionais.
- O frontend acessa a API corretamente (CORS/variáveis de ambiente ajustadas).
- Nenhuma credencial versionada.
- Itens correspondentes marcáveis em [docs/criterios-de-aceitacao.md](../../docs/criterios-de-aceitacao.md).

## Validação

- Executar `docker compose build` e `docker compose up` em ambiente limpo.
- Abrir o frontend no navegador e confirmar o consumo da API.
- Conferir que a carga inicial de 10.000 pedidos aparece na listagem.
