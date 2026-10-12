## Desafio Técnico – Desenvolvedor Web Pleno .NET MC1

## Objetivo

Você vai construir uma pequena aplicação de Cadastro de Pedidos em .NET, com API REST, uma página web para consumir os dados e Docker. O tempo sugerido é de 2 a 3 horas; não esperamos um produto completo.

Queremos entender como você decide, organiza e revisa código. Por isso o uso de IA é permitido e incentivado, e pedimos que você conte como utilizou.

## O que construir

Um pedido tem, no mínimo: identificador, nome do cliente, descrição, valor total, status (Aberto, Pago, Cancelado) e data de criação. Você pode acrescentar campos se fizer sentido.

- 1. API REST com as operações de criar, listar, obter por identificador, atualizar e excluir pedidos.

- 2. Listagem com filtro por texto e paginação.

- 3. Persistência em memória, sem banco de dados. A aplicação deve se comportar corretamente com várias requisições simultâneas.

- 4. Carga inicial de 10.000 pedidos gerados ao iniciar a aplicação.

- 5. Página web que consome a API e permite listar, filtrar, paginar, criar, editar e excluir pedidos.

- 6. A página deve atualizar a lista automaticamente a cada 10 segundos, sem recarregar a página.

- 7. Validações básicas de entrada, com mensagens de erro claras na API e na tela.

## Requisitos técnicos

| Item | Esperado |
| --- | --- |
| Backend | C# com ASP.NET Core (.NET 6 ou superior) |
| Frontend | Framework que desejar e alguma biblioteca de estilização |
| Execução | Dockerfile que permita subir a aplicação com um único comando |
| Documentação | README curto: como rodar, decisões tomadas e o que faria com mais tempo |
| Testes | Opcionais, mas contam a favor |

Organize o código como faria em um projeto real da equipe. Não há uma arquitetura "certa" esperada; queremos ver e entender as suas escolhas.


## Uso de IA

Use qualquer ferramenta de IA que preferir (assistentes de chat, agentes de código, autocompletar na

IDE). Isso faz parte do nosso dia a dia e não conta contra você.

Inclua na raiz do projeto um arquivo AI_USAGE.md com um relato curto, de meia página a uma página, respondendo:

- Quais ferramentas você usou e em quais partes do desafio.

- Dois ou três prompts que considerou mais úteis, e como montou o contexto para a ferramenta.

- O que você aceitou, como veio, o que corrigiu e o que descartou, e por quê.

- Algum erro ou sugestão ruim da IA que você percebeu e como identificou.

Não estamos avaliando a ferramenta, e sim a sua forma de pensar ao trabalhar com ela. Um relato honesto e curto vale mais que um longo.

## Entrega

Envie um link de repositório Git com acesso de leitura.

- Código-fonte completo

- Dockerfile funcional

- README.md com instruções e decisões

- AI_USAGE.md com o relato de uso de IA

Não inclua chaves, senhas ou qualquer credencial no código ou nos arquivos de configuração. Se algo ficou incompleto, registre no README o que faltou e como faria.
