# **Mini loja virtual - Aplicação Simples com MVC e API RESTful**

## **1. Apresentação**

Bem-vindo ao repositório do projeto **Mini loja virtual**. Este projeto é uma entrega do MBA DevXpert Full Stack .NET e é referente ao módulo **Introdução ao Desenvolvimento ASP.NET Core**.

Desenvolvimento de  uma aplicação web básica utilizando conceitos do Módulo 1 (C#,
ASP.NET Core MVC, SQL, EF Core, APIs REST) para gestão simplificada de
produtos e categorias em um formato tipo e-commerce marketplace.


### **Autor**
- **Guilherme Sant'Anna**
- **Jefferson Molaz**
- **Karollainny Teles**
- **Nelson Campozano**
- **Rafael Batista**
- **Rafael Secco**

## **2. Proposta do Projeto**

O projeto consiste em:

- **Aplicação MVC:** Interface web para interação com a loja. 
- **API RESTful:** Exposição dos recursos da loja para integração com outras aplicações ou desenvolvimento de front-ends alternativos.
- **Autenticação e Autorização:** Autenticação via JWT na API e via Cookies pela interface.
- **Acesso a Dados:** Implementação de acesso ao banco de dados através do EF Core.

## **3. Tecnologias Utilizadas**

- **Linguagem de Programação:** C#
- **Frameworks:**
  - ASP.NET Core MVC
  - ASP.NET Core Web API
  - Entity Framework Core
- **Banco de Dados:** SQL Server
- **Autenticação e Autorização:**
  - ASP.NET Core Identity
  - JWT (JSON Web Token) para autenticação na API
- **Front-end:**
  - Razor Pages/Views
  - HTML/CSS para estilização básica
- **Documentação da API:** Swagger

## **4. Estrutura do Projeto**

A estrutura do projeto é organizada da seguinte forma:


- src/
  - AppMvc/ - Projeto MVC
  - Api/ - API RESTful
  - Core/ - Modelos de Dados, Configuração do EF Core e casos de uso.
- README.md - Arquivo de Documentação do Projeto
- FEEDBACK.md - Arquivo para Consolidação dos Feedbacks
- .gitignore - Arquivo de Ignoração do Git

## **5. Funcionalidades Implementadas**

- **CRUD para produto, categoria:** Permite criar, editar, visualizar e excluir produtos e categorias.
- **Autenticação:** criação e autenticação de usuário.
- **API RESTful:** Exposição de endpoints para operações CRUD via API.
- **Documentação da API:** Documentação automática dos endpoints da API utilizando Swagger.

## **6. Como Executar o Projeto**

### **Pré-requisitos**

- .NET SDK 8.0
- SQL Server(Staging) / SQLite(Development)
- Visual Studio 2022 (ou qualquer IDE de sua preferência)
- Git

### **Passos para Execução**

1. **Clone o Repositório:**
   - `git clone https://github.com/karollainnyteles/mba-projeto-modulo01.git`
   - `cd nome-do-repositorio`

2. **Configuração do Banco de Dados:**
   - No arquivo `appsettings.json`, configure a string de conexão do SQL Server.
   - Rode o projeto para que a configuração do Seed crie o banco e popule com os dados básicos

3. **Usuários criados:**
   - **Admin:** Email: `admin@mail.com` e Senha: `Dev@123`
   - **Vendedor 1:** Email: `vendedor1@mail.com` e Senha: `Dev@123`
   - **Vendedor 2:** Email: `vendedor2@mail.com` e Senha: `Dev@123`
   - **Cliente:** Email: `cliente@mail.com` e Senha: `Dev@123`

4. **Executar a API:**
   - `cd src/Api/`
   - `dotnet run`
   - Acesse a documentação da API em: http://localhost:5001/swagger

## **7. Instruções de Configuração**

- **JWT para API:** As chaves de configuração do JWT estão no `appsettings.json`.
- **Migrações do Banco de Dados:** As migrações são gerenciadas pelo Entity Framework Core. Não é necessário aplicar devido a configuração do Seed de dados.

## **8. Documentação da API**

A documentação da API está disponível através do Swagger. Após iniciar a API, acesse a documentação em:

http://localhost:5001/swagger

## **9. Avaliação**

- Este projeto é parte de um curso acadêmico e não aceita contribuições externas. 
- Para feedbacks ou dúvidas utilize o recurso de Issues
- O arquivo `FEEDBACK.md` é um resumo das avaliações do instrutor e deverá ser modificado apenas por ele.
