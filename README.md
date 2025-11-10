# 🎭 DanceWaves - Sistema de Gerenciamento de Competições de Dança

![.NET](https://img.shields.io/badge/.NET-10.0-purple?style=flat-square)
![C#](https://img.shields.io/badge/C%23-Latest-green?style=flat-square)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Azure-blue?style=flat-square)
![License](https://img.shields.io/badge/License-MIT-red?style=flat-square)

## 📋 Índice

- [Visão Geral](#-visão-geral)
- [Recursos](#-recursos)
- [Arquitetura](#-arquitetura)
- [Requisitos](#-requisitos)
- [Instalação](#-instalação)
- [Estrutura de Pastas](#-estrutura-de-pastas)
- [Banco de Dados](#-banco-de-dados)
- [Como Usar](#-como-usar)
- [API Endpoints](#-api-endpoints)
- [Guia de Desenvolvimento](#-guia-de-desenvolvimento)
- [Contribuindo](#-contribuindo)

---

## 🎯 Visão Geral

**DanceWaves** é uma plataforma web completa para gerenciar competições de dança, incluindo:

- 📝 Registro e gerenciamento de entries (inscrições)
- 👥 Administração de usuários e permissões
- 💃 Gerenciamento de competições e categorias
- 📊 Dashboard de registros e estatísticas
- 🔐 Sistema de autenticação e autorização por roles
- 📱 Interface responsiva moderna

**Stack Tecnológico:**
- **Backend:** ASP.NET Core 10.0 (Blazor Server)
- **Frontend:** Blazor Interactive (Server + WebAssembly)
- **Database:** SQL Server (Azure)
- **ORM:** Entity Framework Core 8.0.10
- **Arquitetura:** Hexagonal (Ports & Adapters)

---

## ✨ Recursos

### 🔐 Sistema de Usuários
- 4 Roles de Usuários com permissões distintas:
  - **SuperAdmin:** Acesso total ao sistema
  - **FranchiseAdmin:** Gerencia usuários, competições e resultados conectados
  - **User:** Visualiza dados próprios e competições inscritas
  - **Jury:** Pode inserir resultados em competições conectadas

### 🎪 Gerenciamento de Competições
- Criar e editar competições
- Categorias por: Estilo, Faixa Etária, Nível, Gênero
- Status de competição: Aberta para Registro, Fechada, Finalizada
- Gerenciamento de jurados

### 📝 Sistema de Entries
- Inscrever equipes em categorias
- Gerenciar membros da equipe
- Rastreamento de pagamentos
- Upload de músicas

### 🏫 Gestão de Escolas
- Registrar escolas de dança
- Associar usuários a escolas
- Gerenciar franquias

### 📊 Dashboard e Relatórios
- Estatísticas de registros
- Tracking de status de pagamentos
- Visualização de resultados

---

## 🏛️ Arquitetura

### Arquitetura Hexagonal (Clean Architecture)

O projeto segue rigorosamente a **Arquitetura Hexagonal** com **Ports & Adapters**, garantindo:

```
┌─────────────────────────────────────────────────────┐
│           CAMADA DE APRESENTAÇÃO (UI)              │
│  Blazor Components, Razor Pages, ASP.NET Core      │
└──────────────────┬──────────────────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────────────────┐
│       ADAPTADORES (Presenters & Persistence)       │
│  NavigationPresenterAdapter                        │
│  EntryPersistenceAdapter, UserPersistenceAdapter   │
└──────────────────┬──────────────────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────────────────┐
│      PORTS (Interfaces - Contrato de Negócio)      │
│  INavigationPresenterPort, IEntryPersistencePort   │
│  IUserPersistencePort, ICompetitionPersistencePort │
└──────────────────┬──────────────────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────────────────┐
│      NÚCLEO (Use Cases - Lógica de Negócio)        │
│  GetNavigationMenuUseCase, ListEntriesUseCase      │
│  (Independente de frameworks!)                     │
└──────────────────┬──────────────────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────────────────┐
│       ADAPTADORES (Entity Framework Core)          │
│  Implementações de Persistência                    │
└──────────────────┬──────────────────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────────────────┐
│        CAMADA DE DADOS (SQL Server/Azure)          │
│  Tabelas: Users, Entries, Competitions, etc...     │
└─────────────────────────────────────────────────────┘
```

### Estrutura de Camadas

```
DanceWaves/
├── Application/              🔷 NÚCLEO (Lógica Pura de Negócio)
│   ├── Ports/                📍 Interfaces (Contratos)
│   │   ├── IEntryPersistencePort
│   │   ├── IUserPersistencePort
│   │   ├── ICompetitionPersistencePort
│   │   └── INavigationPresenterPort
│   └── UseCases/             🎯 Casos de Uso (Orquestração)
│       ├── GetNavigationMenuUseCase
│       └── ListEntriesUseCase
│
├── Adapters/                 🔶 ADAPTADORES (Implementações Concretas)
│   ├── Persistence/          💾 Adaptadores de Persistência
│   │   ├── EntryPersistenceAdapter
│   │   ├── UserPersistenceAdapter
│   │   └── CompetitionPersistenceAdapter
│   └── Presenters/           🎨 Adaptadores de Apresentação
│       └── NavigationPresenterAdapter
│
├── Components/               🧩 INTERFACE DO USUÁRIO (Blazor)
│   ├── Layout/
│   │   ├── MainLayout.razor
│   │   ├── NavMenu.razor     ← Menu dinâmico por Use Case
│   │   └── ReconnectModal.razor
│   └── Pages/
│       ├── Entries.razor     📝 Gerenciar Entries
│       ├── Administration.razor ⚙️ Configurações
│       ├── SignUp.razor      📋 Criar Conta
│       └── Registrations.razor ✅ Gerenciar Registros
│
├── Models/                   📦 ENTIDADES DE DOMÍNIO
│   ├── User.cs
│   ├── Entry.cs
│   ├── Competition.cs
│   ├── UserRolePermission.cs
│   ├── CompetitionStatus.cs  (Enum)
│   ├── EntryStatus.cs        (Enum)
│   └── ... (11 modelos no total)
│
├── Data/                     🔌 CAMADA EF CORE
│   ├── ApplicationDbContext.cs
│   ├── DesignTimeDbContextFactory.cs
│   ├── DatabaseInitializer.cs
│   └── UserRolePermissionSeeder.cs
│
└── Migrations/               📜 Histórico de Banco de Dados
    └── 20251110181952_InitialCreate.cs
```

---

## 🔧 Requisitos

### Sistema Operacional
- Windows 10+ / MacOS / Linux

### Ferramentas Necessárias
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) ou superior
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/sql-server/) ou conexão com Azure SQL Database
- [Git](https://git-scm.com/)

### Bibliotecas NuGet
```xml
<ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.10" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.10" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.10" />
</ItemGroup>
```

---

## 🚀 Instalação

### 1. Clonar Repositório
```bash
git clone https://github.com/seu-usuario/DanceWaves.git
cd DanceWaves
```

### 2. Restaurar Dependências
```bash
dotnet restore
```

### 3. Configurar Connection String
Edite `DanceWaves/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=seu-servidor;Database=DanceWaves;User Id=admin;Password=sua-senha;Trusted_Connection=False;Encrypt=True;"
  }
}
```

### 4. Aplicar Migrations
```bash
cd DanceWaves
dotnet ef database update
```

### 5. Executar Aplicação
```bash
dotnet run
```

A aplicação estará disponível em: `https://localhost:5001`

---

## 📁 Estrutura de Pastas

### Explicação Detalhada

#### 1️⃣ `Application/Ports/`
Define os **contratos (interfaces)** entre o núcleo e os adaptadores. Não depende de nenhuma implementação concreta.

```csharp
public interface IEntryPersistencePort
{
    Task<Entry> GetByIdAsync(int id);
    Task<IEnumerable<Entry>> GetAllAsync();
    Task<Entry> CreateAsync(Entry entry);
}
```

#### 2️⃣ `Application/UseCases/`
Contém a **lógica pura de negócio**. Cada use case orquestra a comunicação entre portas.

```csharp
public class ListEntriesUseCase
{
    public async Task<IEnumerable<Entry>> ExecuteAsync()
    {
        return await _entryPersistencePort.GetAllAsync();
    }
}
```

#### 3️⃣ `Adapters/Persistence/`
Implementações concretas das portas de persistência usando **Entity Framework Core**.

```csharp
public class EntryPersistenceAdapter : IEntryPersistencePort
{
    public async Task<IEnumerable<Entry>> GetAllAsync()
    {
        return _dbContext.Entries;
    }
}
```

#### 4️⃣ `Adapters/Presenters/`
Adaptadores que fornecem dados para a UI (Blazor).

```csharp
public class NavigationPresenterAdapter : INavigationPresenterPort
{
    public async Task<NavigationViewModel> GetNavigationMenuAsync()
    {
        // Retorna menu dinâmico
    }
}
```

#### 5️⃣ `Components/Pages/`
Páginas Razor que utilizam os Use Cases via injeção de dependência.

---

## 💾 Banco de Dados

### Diagrama ER (Entidade-Relacionamento)

```
┌─────────────────────────────────────────────────────────┐
│                     TABELAS CRIADAS                     │
├─────────────────────────────────────────────────────────┤

Franchises (1) ──── (N) Users
             └──── (N) DanceSchools

Users (1) ──── (N) Entries
      ├─ (1) DanceSchool (FK)
      ├─ (1) Franchise (FK)
      ├─ (1) AgeGroup (FK)
      └─ (1) UserRolePermission (FK)

DanceSchools (1) ──── (N) Entries
             └──── (N) Users

Competitions (1) ──── (N) CompetitionCategories

CompetitionCategories (1) ──── (N) Entries
                        ├──── (N) JudgePanels
                        ├─ (1) Style (FK)
                        ├─ (1) AgeGroup (FK)
                        └─ (1) Level (FK)

Entries (1) ──── (N) EntryMembers
        └─ (N) Scores

EntryMembers (1) ─ (1) Users

Scores (1) ─ (1) Judges (Users)
       └─ (1) Entries

UserRolePermissions (1) ──── (N) Users
```

### Tabelas e Campos

| Tabela | Campos Principais | Chave Primária |
|--------|------------------|-----------------|
| **Users** | Id, Email, FirstName, LastName, RolePermissionId | Id (Identity) |
| **Entries** | Id, CompetitionCategoryId, StartNumber, Status, PaymentStatus | Id (Identity) |
| **Competitions** | Id, Name, Status (Enum), MaxContestants, Location | Id (Identity) |
| **CompetitionCategories** | Id, CompetitionId, StyleId, AgeGroupId, LevelId, GenderMix (Bool) | Id (Identity) |
| **UserRolePermissions** | Id, Name, Description | Id (Identity) |
| **Styles** | Id, Code, Name | Id (Identity) |
| **AgeGroups** | Id, Code, Name, MinAge, MaxAge | Id (Identity) |
| **Levels** | Id, Code, Name | Id (Identity) |

### Seed Data (Dados Iniciais)

A aplicação insere automaticamente 4 roles ao iniciar:

```sql
INSERT INTO UserRolePermissions (Name, Description) VALUES
('SuperAdmin', 'Sees everything'),
('FranchiseAdmin', 'Manages all connected users, contests, results'),
('User', 'Sees his own data and joined contests'),
('Jury', 'Can put results in the system per connected contest');
```

---

## 📖 Como Usar

### 🏠 Página Inicial
Acesse `https://localhost:5001` para ver a página inicial com menu dinâmico.

### 📝 Menu Entries
- **Rota:** `/entries`
- **Ícone:** 📝
- Visualize todas as entries registradas
- Clique em "Edit" para modificar ou "Delete" para remover

### ⚙️ Menu Administration
- **Rota:** `/administration`
- **Ícone:** ⚙️
- **Submenus:**
  - Users Management
  - Competitions Management
  - System Settings
- Gerenciar configurações do sistema

### 📋 Menu Sign-up
- **Rota:** `/signup`
- **Ícone:** 📋
- Formulário completo para criar nova conta
- Campos: Name, Email, Phone, Password
- Validação de termos de serviço

### ✅ Menu Registrations
- **Rota:** `/registrations`
- **Ícone:** ✅
- Dashboard de registros
- Filtro de pesquisa
- Estatísticas: Total, Pending, Approved, Rejected

---

## 🔌 API Endpoints

### Futuro: REST API

Quando implementado, os endpoints seguirão o padrão RESTful:

```http
# Entries
GET    /api/entries              - Listar todas as entries
GET    /api/entries/{id}         - Obter entry específico
POST   /api/entries              - Criar nova entry
PUT    /api/entries/{id}         - Atualizar entry
DELETE /api/entries/{id}         - Deletar entry

# Users
GET    /api/users                - Listar todos os usuários
GET    /api/users/{id}           - Obter usuário específico
POST   /api/users/signup         - Criar novo usuário
PUT    /api/users/{id}           - Atualizar usuário
DELETE /api/users/{id}           - Deletar usuário

# Competitions
GET    /api/competitions         - Listar competições
POST   /api/competitions         - Criar competição
PUT    /api/competitions/{id}    - Atualizar competição
DELETE /api/competitions/{id}    - Deletar competição
```

---

## 👨‍💻 Guia de Desenvolvimento

### Adicionar Novo Use Case

**Passo 1:** Criar a porta (interface)
```csharp
// Application/Ports/IMyNewPort.cs
public interface IMyNewPort
{
    Task<MyEntity> GetByIdAsync(int id);
}
```

**Passo 2:** Criar o use case
```csharp
// Application/UseCases/MyNewUseCase.cs
public class MyNewUseCase
{
    private readonly IMyNewPort _port;
    
    public MyNewUseCase(IMyNewPort port)
    {
        _port = port;
    }
    
    public async Task<MyEntity> ExecuteAsync(int id)
    {
        return await _port.GetByIdAsync(id);
    }
}
```

**Passo 3:** Criar o adaptador
```csharp
// Adapters/Persistence/MyNewAdapter.cs
public class MyNewAdapter : IMyNewPort
{
    private readonly ApplicationDbContext _dbContext;
    
    public async Task<MyEntity> GetByIdAsync(int id)
    {
        return await _dbContext.MyEntities.FindAsync(id);
    }
}
```

**Passo 4:** Registrar no DI (Program.cs)
```csharp
builder.Services.AddScoped<IMyNewPort, MyNewAdapter>();
builder.Services.AddScoped<MyNewUseCase>();
```

### Executar Testes

```bash
# Executar todos os testes
dotnet test

# Teste específico
dotnet test --filter "TestClass.TestMethod"
```

### Build da Aplicação

```bash
# Debug
dotnet build

# Release
dotnet build -c Release

# Build e publicação
dotnet publish -c Release -o ./publish
```

### Gerenciamento de Migrations

```bash
# Criar nova migration
dotnet ef migrations add NomeDaMigration

# Aplicar migrations
dotnet ef database update

# Remover última migration
dotnet ef migrations remove

# Listar migrations
dotnet ef migrations list
```

---

## 🐛 Troubleshooting

### ❌ Erro: "Cannot open database 'DanceWaves'"
**Solução:** Verifique se o SQL Server está rodando e se a connection string está correta em `appsettings.json`.

### ❌ Erro: "Entity type 'X' is not mapped"
**Solução:** Verifique se o DbSet foi adicionado em `ApplicationDbContext.cs`:
```csharp
public DbSet<MyEntity> MyEntities { get; set; }
```

### ❌ Erro: "The instance of entity type cannot be tracked"
**Solução:** Use `.AsNoTracking()` em consultas somente leitura:
```csharp
_dbContext.Entries.AsNoTracking().ToList()
```

---

## 📚 Recursos Úteis

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Blazor Tutorial](https://docs.microsoft.com/en-us/aspnet/core/blazor/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Hexagonal Architecture](https://alistair.cockburn.us/hexagonal-architecture/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

---

## 🤝 Contribuindo

1. **Faça um Fork** do repositório
2. **Crie uma Branch** para sua feature (`git checkout -b feature/AmazingFeature`)
3. **Commit suas mudanças** (`git commit -m 'Add some AmazingFeature'`)
4. **Push para a Branch** (`git push origin feature/AmazingFeature`)
5. **Abra um Pull Request**

### Padrões de Código

- Use **PascalCase** para nomes de classes e métodos
- Use **camelCase** para variáveis locais
- Sempre use **async/await** para operações I/O
- Documente classes e métodos públicos com **XML Comments**

---

## 📄 Licença

Este projeto está sob a licença **MIT**. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

## 👥 Autores

- **Desenvolvedor:** [Seu Nome]
- **Email:** seu-email@example.com
- **GitHub:** [@seu-usuario](https://github.com/seu-usuario)

---

## 📞 Suporte

Para reportar bugs ou sugerir features, abra uma [Issue](https://github.com/seu-usuario/DanceWaves/issues).

---

## 🎉 Agradecimentos

Obrigado a todos que contribuem para melhorar o **DanceWaves**!

**Última atualização:** 10 de Novembro de 2025

---

<div align="center">

### 💜 Se você encontrou este projeto útil, por favor dê uma ⭐!

**DanceWaves** - Transformando o Mundo da Dança com Tecnologia 🎭✨

</div>
