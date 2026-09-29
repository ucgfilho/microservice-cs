# Microsserviço de Catálogo e Autenticação

Microsserviço RESTful desenvolvido em **C# / ASP.NET Core 10** e **MySQL / MariaDB**, estruturado seguindo os princípios de design de software **SOLID**, controle de acesso baseado em funções (**RBAC**) e arquitetura em camadas.

O projeto conta com persistência assíncrona via Entity Framework Core, autenticação via tokens JWT+Bearer com Claims de perfil, autorização granular de recursos e documentação interativa com Scalar.

---

## Arquitetura

A arquitetura do projeto é estruturada em camadas desacopladas por meio de interfaces e injeção de dependência nativa do ASP.NET Core:

```
[   Cliente HTTP   ]
         ↓
[   Controllers    ]     → Camada de Apresentação (rotas, validação de payload/DTOs, autorização e status codes)
         ↓
[     Services     ]     → Regras de Negócio e Orquestração (hashing de senha, emissão de JWT, CRUD e auditoria)
         ↓
[   AppDbContext   ]     → Persistência de Dados (Entity Framework Core assíncrono com MariaDB/MySQL)
```

---

## Estrutura do Projeto

```
projetoAPI/
├── Controllers/              # Endpoints HTTP da API
│   ├── AuthController.cs
│   ├── CategoriesController.cs
│   └── ProductsController.cs
├── DTOs/                     # Data Transfer Objects e configurações tipadas
│   ├── AuthResult.cs
│   ├── JwtSettings.cs
│   ├── LoginDTO.cs
│   ├── ProductCreateDTO.cs
│   └── RegisterDTO.cs
├── Data/                     # Contexto do Entity Framework Core
│   └── AppDbContext.cs
├── Migrations/               # Histórico e migrações do banco de dados
├── Models/                   # Entidades de Domínio
│   ├── Category.cs
│   ├── Product.cs
│   └── User.cs
├── Services/                 # Camada de serviços e regras de negócio
│   ├── AuthService.cs
│   ├── BcryptPasswordHasher.cs
│   ├── CategoryService.cs
│   ├── ProductService.cs
│   ├── TokenService.cs
│   └── Interfaces/           # Contratos dos serviços
│       ├── IAuthService.cs
│       ├── ICategoryService.cs
│       ├── IPasswordHasher.cs
│       ├── IProductService.cs
│       └── ITokenService.cs
├── Program.cs                # Inicialização, injeção de dependências e middlewares
├── appsettings.json          # Configurações de ambiente
├── docker-compose.yml        # Orquestração de containers da API e banco
├── Dockerfile                # Build e publicação da imagem da aplicação
└── projetoAPI.csproj         # Dependências e metadados do projeto
```

---

## Regras de Negócio e Acesso (RBAC)

O sistema implementa autenticação JWT e autorização baseada em papéis (**Roles**):

### 1. Perfis de Usuário
* **`cliente`**: Perfil voltado para consumidores. Possui permissão apenas de leitura (consulta) no catálogo de produtos.
* **`vendedor`**: Perfil voltado para lojistas/fornecedores. Possui permissão de criação, edição e exclusão de produtos.

### 2. Registro e Autenticação
* Ao se registrar (`POST /api/auth/register`), o usuário deve obrigatoriamente informar o tipo de conta desejado: `"cliente"` ou `"vendedor"`.
* Ao realizar login (`POST /api/auth/login`), o token JWT gerado embute o identificador do usuário (`ClaimTypes.NameIdentifier`) e o seu papel (`ClaimTypes.Role`).

### 3. Gestão de Produtos
* **Criação (`POST /api/products`)**:
  * Permitida exclusivamente para usuários com o perfil `vendedor`.
  * O ID do produto é gerado automaticamente pelo banco de dados (o payload via `ProductCreateDTO` não expõe campos de ID).
  * O produto é vinculado automaticamente ao identificador do usuário que o cadastrou (`id_usuario`).
* **Alteração (`PUT` e `PATCH /api/products/{id}`)**:
  * Permitida apenas para usuários do tipo `vendedor`.
  * O vendedor só pode atualizar produtos que ele mesmo criou. Tentativas de alterar produtos de outros vendedores resultam em `403 Forbidden`.
* **Exclusão (`DELETE /api/products/{id}`)**:
  * Permitida apenas para usuários do tipo `vendedor`.
  * O vendedor só pode excluir produtos criados por ele mesmo (`403 Forbidden` caso pertença a outro usuário).
* **Consulta (`GET /api/products` e `GET /api/products/{id}`)**:
  * Acessível por qualquer usuário autenticado (`cliente` ou `vendedor`).

---

## Stack Tecnológica

* **Runtime & Framework:** C# / .NET 10 (ASP.NET Core Web API)
* **ORM:** Entity Framework Core 9 (Pomelo MySQL)
* **Banco de Dados:** MariaDB / MySQL
* **Autenticação & Segurança:** JWT (Bearer) + BCrypt.Net
* **Documentação OpenAPI:** Scalar API Reference

---

## Como Executar

### 1. Pré-requisitos
* [Docker](https://www.docker.com/) e Docker Compose instalados.

### 2. Variáveis de Ambiente
Copie o arquivo de exemplo para gerar o arquivo `.env`:
```bash
cp .env.example .env
```
*(Nota: O `.env` centraliza as senhas e portas de conexão. A chave de assinatura JWT é configurada no `appsettings.json` ou gerada dinamicamente caso omitida).*

### 3. Subir com Docker Compose
Para iniciar a API e o banco de dados simultaneamente:
```bash
docker-compose up -d --build
```
> **Nota:** As migrações do Entity Framework Core são executadas automaticamente na inicialização da API via `dbContext.Database.Migrate()`. O banco de dados fica exposto na porta `3307` e a API na porta `8080`.

### 4. Execução Local (sem Docker)
Caso deseje rodar a API localmente com o banco em execução:
```bash
dotnet restore
dotnet build
dotnet run
```

### 5. Documentação Interativa
Com a aplicação em execução, acesse a documentação interativa pelo navegador:
* **http://localhost:8080/scalar/v1** (Docker) ou **http://localhost:5096/scalar/v1** (Local)
