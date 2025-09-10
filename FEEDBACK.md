## Funcionalidade 30%

Avalie se o projeto atende a todos os requisitos funcionais definidos.
* Ao adicionar mais um favorito, apenas o primeiro produto é exibido.
* Na loja, não é "interessante" exibir produtos sem estoque, ou se está ativo.
* Eu desativei um produto e ele continuou aparecendo na loja.
* Alterei o nome de um produto, mas na loja continuou aparecendo o nome antigo.
* Notei que o Id do projeto da Loja é diferente do Id do produto cadastrado. Não sei se isso é intencional, mas não faz sentido.
* Os projetos de API e MVC deveriam usar o mesmo banco de dados, para que compartilhem os dados de usuarios e produtos.

## Qualidade do Código 20%

Considere clareza, organização e uso de padrões de codificação.

### Data
* Em `ApplicationDbContext` existe uso de "hacks" para configurar relacionamentos. Aparentam ser desnecessários.
  - O "hack" de auto incremento também serviria para o `SQL Server`, ou a geração do Id pode ser feita pela aplicação sem depender do banco.
  - O "hack" de `varchar(max)` não é necessário, pois o EF Core já mapeia `string` para `nvarchar(max)` por padrão ou respeitando as anotações de `MaxLength`.
* A abstração `Repository<T>` possui métodos públicos que permitem alterações na entidade sem passar pelos repositórios especializados.
* Em `ClienteRepository`, o método `AtualizaAtivoAsync()` não falha quando o `cliente` não é encontrado. Isso causa um falso positivo.
* Em `ProdutoRepository`:
  - o método `GetValidProductsAsync()` causa confusão, deveria ser `GetActiveProdutcs()`.
  - o método `FindAsync()` deve retornar `Produto?` já que `FirstOrDefaultAsync()` pode retornar `null`.

### Business
* A classe `Entity` deve ser abstrata.
* Quando se tem uma pasta chamada `Util`, quer dizer que tudo fora dela é `Inútil`. Evite essas generecidades.
* A classe `Upload` poderia ser um `FileManagementService` ou algo do tipo, e ser hospedada em `Services`.
* Em `ClienteService` foi adicionada uma dependencia à camanda de `AspNetCore` ao injetar `IHttpContextAccessor`. A intenção é boa, mas existe melhores maneiras de saber o usuario logado sem essa super acoplagem.
* Em `VendedorService` o método `GetBeVendedorId()` não espera um `Id` como parametro, fazendo o nome ser confuso.

### Geral
* Remova códigos comentados ou não utilizados.
* Remova `usings` não utilizados.
* Padronize a nomenclatura de classes, métodos e variáveis. Adote ou Português ou Inglês.
* Métodos com prefixo `Get`/`Obter` devem retornar algo singular, e `List`/`Listar` ou `Find`/`Buscar` retornar coleções.

## Eficiência e Desempenho 20%

Avalie o desempenho e a eficiência das soluções implementadas.
* Muito bom o uso de `async/await` e `CancellationToken`.

## Inovação e Diferenciais 10%

Considere a criatividade e inovação na solução proposta.
* Projeto sólido, com boa separação de responsabilidades e uso de boas práticas.

## Documentação e Organização 10%

Verifique a qualidade e completude da documentação, incluindo README.md.

* Mensiona `appsettings.json` mas não menciona de qual projeto.
* Em `Configuração do Banco de Dados` não diz qual projeto rodar para criar o banco e popular os dados.
* Em Markdown, não utilizar crase para links, senão o link não funciona.
* Não diz como iniciar a aplicação de Adminstração (MVC). Eu assumir que era fazer `dotnet run` na pasta `src/AppMvc`, mas não está no README.

## Resolução de Feedbacks 10%

Avalie a resolução dos problemas apontados na primeira avaliação de frontend
* Todos os pontos negativos foram endereçados.


## Notas

| Critério                     | Peso | Nota | Nota Ponderada |
|------------------------------|------|-----:|---------------:|
| Funcionalidade               | 30%  |    8 |            2.4 |
| Qualidade do Código          | 20%  |    8 |            1.6 |
| Eficiência e Desempenho      | 20%  |    9 |            1.8 |
| Inovação e Diferenciais      | 10%  |    9 |            0.9 |
| Documentação e Organização   | 10%  |    7 |            0.7 |
| Resolução de Feedbacks       | 10%  |   10 |            1.0 |
| **Total**                    |      |      |        **8.4** |
