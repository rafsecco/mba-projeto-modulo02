
# Feedback - Avaliação Geral

## Front End
### Navegação
  * **Positivo**: Projeto MVC planejado para interface web
  * **Negativo**: A parte MVC não foi concluída conforme mencionado no README

### Design
 - Será avaliado na entrega final

### Funcionalidade
  * **Negativo**: Funcionalidades MVC não implementadas

## Back End
### Arquitetura
  * **Positivo**: 
    - Segue a estrutura em 3 camadas (MVC, API, Application)

  * **Negativo**: 
    - Falta de implementação da camada MVC
    - Uso desnecessário de CQRS para um projeto desta complexidade
    - A camada Application faz muito mais do que o nome diz, o ideal seria "Core" (pois faz negócios/acesso a dados)

### Funcionalidade
  * **Positivo**:
    - API RESTful implementada
    - CRUD para produtos e categorias na API
    - Autenticação via JWT
    - Documentação Swagger

  * **Negativo**:
    - Funcionalidades MVC não implementadas
    - Possível falta de validações de negócio

### Modelagem
  * **Positivo**: 
    - Modelagem simples

## Projeto
### Organização
  * **Positivo**:
    - Estrutura de pastas bem definida
    - Separação clara entre MVC, API e Application
  * **Negativo**:
    - Falta de implementação completa do MVC

### Documentação
  * **Positivo**:
    - README bem estruturado
    - Instruções claras de instalação
    - Documentação da API via Swagger

### Instalação
  * **Positivo**:
    - Instruções claras de configuração
    - Pré-requisitos bem definidos
    - Configuração de ambiente detalhada

  * **Negativo**:
    - Configuração de Seed para dados iniciais inexistente
    - Sem migracoes automaticas