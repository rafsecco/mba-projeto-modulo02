# Feedback - Avaliação Geral

## Front End

### Navegação
  * Pontos positivos:
    - Camada MVC com navegação clara e estruturada.
    - Views implementadas para gerenciamento de produtos, categorias e autenticação de usuários.
    - CRUD completo operando de forma funcional.

  * Pontos negativos:
    - Nenhum identificado neste aspecto.

### Design
  - A interface web é funcional, simples e adequada ao propósito administrativo. Navegação fluida entre funcionalidades.

### Funcionalidade
  * Pontos positivos:
    - CRUD completo para produtos e categorias implementado tanto em MVC quanto na API.
    - Controle de autenticação e autorização corretamente configurado com ASP.NET Identity nas duas camadas.
    - Registro de usuário com associação de vendedor no momento do cadastro.
    - Acesso restrito a ações protegidas.
    - Exibição pública de produtos na home.

  * Pontos negativos:
    - Nenhum ponto funcional negativo.

## Back End

### Arquitetura
  * Pontos positivos:
    - Arquitetura enxuta com separação clara em três camadas: Api, AppMvc e Application.
    - Uso coerente de abstrações e responsabilidades distribuídas de forma coesa.
    - Projeto bem estruturado com organização lógica de arquivos e diretórios.

  * Pontos negativos:
    - A linguagem utilizada para nomes de arquivos, entidades e rotas é majoritariamente em inglês, o que conflita com o idioma de negócio especificado como português no documento de escopo.

### Funcionalidade
  * Pontos positivos:
    - Registro simultâneo de usuário e entidade vendedor implementado corretamente.
    - Autenticação com Identity (cookie no MVC e JWT na API).
    - SQLite implementado com configuração adequada.
    - Migrations automáticas e seed de dados funcionais no startup da aplicação.

  * Pontos negativos:
    - Nenhum ponto negativo neste aspecto.

### Modelagem
  * Pontos positivos:
    - Modelagem simples, adequada ao domínio e com separação entre User Identity e Vendedor.
    - Regras de integridade e relacionamentos coerentes.

  * Pontos negativos:
    - Nenhum ponto negativo relevante observado.

## Projeto

### Organização
  * Pontos positivos:
    - Projeto bem organizado com uso correto da pasta `src`.
    - Solução `.sln` presente e incluindo todos os projetos.
    - Separação por camadas de forma clara.

  * Pontos negativos:
    - Linguagem dos arquivos e nomenclaturas majoritariamente em inglês, divergente do padrão solicitado no escopo do projeto.

### Documentação
  * Pontos positivos:
    - README.md presente com instruções básicas.
    - Swagger configurado na API.
    - FEEDBACK.md presente.

### Instalação
  * Pontos positivos:
    - SQLite corretamente configurado.
    - Seed de dados automático e migrations configuradas no início da aplicação.

  * Pontos negativos:
    - Nenhum ponto negativo encontrado.

---

# 📊 Matriz de Avaliação de Projetos

| **Critério**                   | **Peso** | **Nota** | **Resultado Ponderado**                  |
|-------------------------------|----------|----------|------------------------------------------|
| **Funcionalidade**            | 30%      | 10       | 3,0                                      |
| **Qualidade do Código**       | 20%      | 10       | 2,0                                      |
| **Eficiência e Desempenho**   | 20%      | 10       | 2,0                                      |
| **Inovação e Diferenciais**   | 10%      | 10       | 1,0                                      |
| **Documentação e Organização**| 10%      | 8        | 0,8                                      |
| **Resolução de Feedbacks**    | 10%      | 10       | 1,0                                      |
| **Total**                     | 100%     | -        | **9,8**                                  |

## 🎯 **Nota Final: 9,8 / 10**
