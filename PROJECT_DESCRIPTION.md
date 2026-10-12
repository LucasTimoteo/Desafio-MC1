# Projeto Cadastro de Pedidos — guia para apresentação

Este documento explica a implementação como eu a apresentaria em uma entrevista:
qual problema resolve, como as partes se comunicam, quais decisões foram tomadas e
quais limites são intencionais. O [enunciado original](docs/original-challenge.md) é a
fonte de verdade dos requisitos; escolhas como Angular, .NET 10 e dois containers são
decisões deste projeto, não exigências do desafio.

## Apresentação em aproximadamente um minuto

> Desenvolvi uma aplicação de cadastro de pedidos com uma API REST em ASP.NET Core
> sobre .NET 10, uma interface Angular com Angular Material e execução local em Docker
> Compose. A API mantém pedidos e catálogo em memória, gera 10.000 pedidos na
> inicialização e oferece CRUD, filtros por texto e status e paginação. Um pedido
> contém itens associados a produtos; o total é calculado a partir dos preços e
> quantidades desses itens. A interface consome a API, permite pesquisar, paginar,
> criar, editar, excluir e alterar o status, e consulta a lista a cada dez segundos.
> Escolhi uma estrutura direta, com controllers, serviços e repositórios, e testes
> xUnit no backend, incluindo cenários concorrentes. Como a persistência é volátil,
> os dados são reinicializados quando a API reinicia — banco de dados não fazia parte
> do escopo original.

## O que o desafio pede e o que foi acrescentado

O enunciado pede uma entidade de pedido com identificador, cliente, descrição, valor
total, status (`Aberto`, `Pago` ou `Cancelado`) e data de criação; endpoints REST de
CRUD; pesquisa por texto e paginação; armazenamento em memória que tolere requisições
simultâneas; 10.000 pedidos gerados no início; uma página para operar os pedidos;
atualização da lista a cada dez segundos; mensagens claras para entradas inválidas;
e execução em Docker.

Para representar os pedidos com mais contexto, a implementação acrescenta um catálogo
de produtos e itens por pedido. `ValorTotal` é derivado da soma de `Subtotal`, e cada
subtotal é preço unitário multiplicado pela quantidade. Os itens guardam o nome e o
preço do produto no momento em que o pedido é montado, funcionando como um retrato
daquela compra. `CreatedAt` e `UpdatedAt` existem nas entidades; além disso, o pedido
mantém `DataCriacao`, que é o campo de criação apresentado pela API e pela interface.

Angular + Material, .NET 10, dois serviços no Compose, `ConcurrentDictionary`,
polling via RxJS e testes apenas no backend são escolhas deste projeto. O enunciado
permite diferentes frameworks e arquiteturas, pede .NET 6 ou superior e considera
testes opcionais.

## Backend

### Organização e responsabilidades

O backend está em `backend/src/Pedidos.Api`:

- **`Domain`** contém `Pedido`, `PedidoItem`, `Produto`, `StatusPedido` e a classe
  base `EntidadeBase`. As entidades herdam identificador e timestamps da classe base.
- **`Dtos`** define os formatos de entrada e saída HTTP. Os requests de criação e
  atualização têm DataAnnotations; os responses expõem os campos necessários à UI,
  sem devolver diretamente as entidades.
- **`Controllers`** define os endpoints e traduz operações bem-sucedidas em respostas
  HTTP. A lógica de negócio permanece nos serviços.
- **`Services`** aplica filtro, ordenação e paginação; valida referências a produtos;
  monta e altera pedidos; e converte entidades para DTOs.
- **`Infrastructure`** implementa os repositórios em memória, a carga inicial e o
  tratamento global de exceções.
- **`Program.cs`** registra dependências, JSON, CORS, OpenAPI/Swagger e o seeder.

Os serviços são `scoped`; os repositórios e o seeder são `singleton`. Assim, as
requisições compartilham o mesmo estado em memória durante a vida do processo.

### Contrato HTTP

Todos os endpoints usam o prefixo `/api`:

| Método e rota | Comportamento |
|---|---|
| `GET /api/pedidos?texto=...&status=...&pagina=1&tamanhoPagina=20` | Lista pedidos filtrando opcionalmente por texto e status, ordenados pela data de criação mais recente; retorna itens e metadados de paginação. |
| `GET /api/pedidos/{id}` | Obtém um pedido por identificador. |
| `POST /api/pedidos` | Cria um pedido e responde `201 Created`, incluindo a localização do recurso. O status inicial é `Aberto`. |
| `PUT /api/pedidos/{id}` | Atualiza cliente, descrição e itens de um pedido existente. |
| `PATCH /api/pedidos/{id}/status` | Atualiza somente o status, com um request próprio. |
| `DELETE /api/pedidos/{id}` | Exclui um pedido e responde `204 No Content`. |
| `GET /api/produtos` | Lista o catálogo disponível para selecionar itens. |

A listagem pesquisa sem diferenciar maiúsculas de minúsculas no nome do cliente e na
descrição. O parâmetro `status` é opcional. A paginação começa em 1; os valores são
normalizados para um mínimo de 1, e o tamanho é limitado a 100 (padrões: página 1,
20 itens). `ResultadoPaginado` informa `itens`, `total`, `pagina`, `tamanhoPagina` e
`totalPaginas`.

Para criar ou editar, o cliente envia nome, descrição e pelo menos um item, cada um
com `produtoId` e quantidade. A API obtém o preço e o nome atuais do catálogo pelo ID
do produto e calcula os subtotais e o total. Portanto, o navegador não é a fonte de
verdade do preço nem do total. O status tem um endpoint próprio para deixar essa
operação separada da edição dos demais campos.

### Validação, erros e concorrência

Os requests usam DataAnnotations para regras como nome obrigatório, limites de
comprimento e quantidade de 1 a 1000. Como os controllers usam `[ApiController]`,
erros de validação do modelo são respondidos automaticamente em formato de problema
de validação. Regras que dependem do domínio ficam no serviço: por exemplo, o pedido
precisa conter itens e cada produto referenciado deve existir.

O `GlobalExceptionHandler` converte registro inexistente em `404` e erros de validação
de domínio em `400`, ambos usando `ProblemDetails`. Erros inesperados são registrados
no log e retornam uma mensagem genérica, sem expor detalhes internos ao consumidor.

Os repositórios usam `ConcurrentDictionary<Guid, ...>` para armazenar e acessar
pedidos e produtos sem banco de dados. Essa estrutura protege operações sobre o
dicionário; não representa transações de banco nem garante atomicidade de uma sequência
de alterações no objeto. O teste de concorrência exercita criação paralela e uma
mistura de leitura, mudança de status e exclusão em paralelo, verificando exceções e
contagem final.

### Carga inicial e testes

No startup, `DataSeeder` cria um catálogo fixo de 20 produtos e exatamente 10.000
pedidos. Os nomes, descrições, status, datas e itens variam; uma seed fixa para o
gerador aleatório torna os dados repetíveis para demonstração. As datas dos pedidos
são distribuídas pelos seis meses anteriores. Como o armazenamento é volátil, esse
conjunto é gerado novamente ao reiniciar a API.

Os testes xUnit estão em `backend/tests/Pedidos.Tests`. Eles cobrem cálculo do total,
status e timestamps, produto inexistente, pedido inexistente, filtro de texto, filtro
por status, paginação, atualização, exclusão, carga inicial e operações concorrentes.
Não há suíte de testes frontend neste projeto.

### Inicialização e configuração

`Program.cs` registra controllers, conversão de enums em strings no JSON, Swagger e
uma política CORS baseada em `Cors:AllowedOrigins` (com fallback de desenvolvimento).
Swagger e Swagger UI são habilitados somente no ambiente Development. A configuração
de CORS é explícita; no Compose, o navegador acessa a API pelo proxy de mesma origem
do Nginx.

## Frontend

O frontend está em `frontend/pedidos-web` e usa Angular standalone, TypeScript,
Angular Material, formulários reativos e RxJS. Os contratos TypeScript em
`src/app/models/pedido.models.ts` espelham os DTOs JSON da API.

`PedidoService` centraliza as chamadas HTTP: listar com os parâmetros de consulta,
obter, criar, atualizar, mudar status, excluir e listar produtos. Isso mantém os
componentes fora da montagem de URLs e permite tipar as respostas.

`PedidosListComponent` é a tela principal. Apresenta tabela, busca por cliente ou
descrição, filtro por status, paginador, formatação de moeda/data e ações. A busca de
texto aguarda 400 ms após a digitação antes de chamar a API; alterar um filtro volta
à primeira página. Os pedidos são ordenados no backend, não reordenados localmente.

O polling usa `interval(10_000)`: ao receber cada pulso, a tela recarrega a página
atual silenciosamente se não houver diálogo aberto. As inscrições são encerradas em
`ngOnDestroy`. A lista é atualizada sem recarregar o documento; a guarda do diálogo
evita que o refresh interrompa uma edição em andamento.

`PedidoFormDialogComponent` é usado tanto para criação quanto para edição. Mantém
campos de cliente e descrição e permite adicionar/remover itens, selecionar produtos
do catálogo e informar quantidades. Validadores do formulário ajudam o usuário antes
do envio; a prévia do total é calculada no navegador para feedback, mas a API recalcula
o valor definitivo. Em caso de erro, o formulário apresenta mensagens retornadas pela
API.

`ConfirmDialogComponent` solicita confirmação antes de excluir. A mudança de status
é oferecida como ação separada na tabela. O helper `extrairMensagensErro` converte
`ProblemDetails`, erros de validação e falhas de conexão em mensagens legíveis; a tela
usa alertas e snackbars para apresentá-las.

Em desenvolvimento, `environment.ts` usa `http://localhost:5029/api`. No build de
produção, a configuração substituída usa `/api`, encaminhado pelo Nginx. As
dependências e versões Angular estão em `frontend/pedidos-web/package.json`.

## Docker e caminho da requisição

O `docker-compose.yml` orquestra dois serviços na rede privada `pedidos-net`:

1. **`api`** constrói a imagem a partir de `backend/Dockerfile`. O Dockerfile usa uma
   etapa SDK .NET 10 para restaurar/publicar e uma imagem ASP.NET Runtime 10 para
   executar. O processo roda como usuário não-root `app` e escuta na porta 8080
   dentro do container. Essa porta é exposta à rede Compose, mas não publicada
   diretamente no host.
2. **`frontend`** constrói `frontend/pedidos-web/Dockerfile`. A etapa Node 22 instala
   as dependências com `npm ci` e compila o Angular; a imagem final Nginx 1.27 serve
   os arquivos estáticos. A porta 80 do container é publicada como `8080` no host.

No container, o Nginx faz duas coisas: entrega o SPA e encaminha `/api/` para
`http://api:8080/api/` pelo DNS interno do Compose. Assim, em produção, o navegador
faz chamadas de mesma origem para `http://localhost:8080/api/...`; a API não precisa
ser publicada separadamente para esse fluxo. O Nginx também devolve `index.html` em
rotas do SPA.

Para subir os dois serviços a partir da raiz do projeto:

```bash
docker compose up --build
```

Depois, acesse `http://localhost:8080`. A disponibilidade da API é gerida pelo
Compose; `depends_on` estabelece a ordem de início, mas não configura uma verificação
de prontidão da aplicação.

## Decisões e trade-offs para discutir

- **Memória em vez de banco:** atende diretamente ao desafio e reduz o setup, mas
  perde os dados ao reiniciar e não compartilha estado entre réplicas da API.
- **`ConcurrentDictionary`:** fornece acesso concorrente simples ao armazenamento,
  mas não implementa transações ou isolamento entre alterações de uma entidade.
- **Itens e catálogo:** permitem compor o pedido e derivar um total verificável.
  O preço é copiado para o item no momento da operação; o cliente só envia IDs e
  quantidades.
- **Polling em vez de push:** atende ao intervalo pedido sem introduzir WebSockets ou
  infraestrutura de eventos. Há uma chamada periódica enquanto a tela está ativa.
- **Dois containers:** separa frontend e API, mantendo um endereço de entrada simples
  pelo Nginx, ao custo de uma configuração adicional de proxy.
- **Testes no backend:** concentram-se nas regras de negócio e no risco de concorrência;
  testes automatizados de interface não fazem parte do escopo implementado.
- **Escopo deliberadamente enxuto:** não há autenticação, autorização, persistência
  durável, CI/CD ou observabilidade avançada, pois não são pedidos pelo enunciado.

## Perguntas comuns em entrevista

**Por que o backend calcula o valor total?**  
O navegador pode mostrar uma prévia para melhorar a experiência, mas não deve ser
considerado confiável para definir valores. O serviço resolve os produtos pelo ID,
usa os preços do catálogo e calcula o total a partir das quantidades.

**O que acontece se eu reiniciar a API?**  
Os pedidos em memória são perdidos e a carga determinística de 10.000 pedidos e o
catálogo são recriados. Para persistência real, o próximo passo seria escolher um
banco e definir migrações e estratégia de concorrência adequadas.

**Como a lista se mantém atualizada?**  
O componente agenda chamadas com RxJS a cada dez segundos, preserva os filtros e a
página atual e não recarrega a página do navegador. Enquanto um diálogo de edição ou
confirmação está aberto, o polling é ignorado.

**Onde fica a validação?**  
As regras simples e de formato ficam nos DTOs e também têm feedback nos formulários.
As regras dependentes do estado da aplicação, como existência do produto, ficam no
serviço. A API devolve erros em formato ProblemDetails para que a UI possa exibi-los.

**Como provar que o requisito de concorrência foi considerado?**  
Os repositórios usam coleções concorrentes, e os testes exercitam criação paralela e
operações mistas. Isso valida os cenários cobertos, sem afirmar garantias de
transação que um store em memória não fornece.

## Arquivos úteis para aprofundar

- [Enunciado original](docs/original-challenge.md)
- [Requisitos e decisões](docs/requisitos.md)
- [Arquitetura](docs/arquitetura.md)
- [API e composição](backend/src/Pedidos.Api/Program.cs)
- [Endpoints de pedidos](backend/src/Pedidos.Api/Controllers/PedidosController.cs)
- [Regras de pedidos](backend/src/Pedidos.Api/Services/PedidoService.cs)
- [Carga inicial](backend/src/Pedidos.Api/Infrastructure/DataSeeder.cs)
- [Lista Angular](frontend/pedidos-web/src/app/pedidos/pedidos-list.component.ts)
- [Formulário Angular](frontend/pedidos-web/src/app/pedidos/pedido-form-dialog.component.ts)
- [Serviço HTTP do frontend](frontend/pedidos-web/src/app/services/pedido.service.ts)
- [Docker Compose](docker-compose.yml)
