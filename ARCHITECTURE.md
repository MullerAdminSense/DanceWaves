# 🏛️ DanceWaves - Hexagonal Architecture Implementation

## Architecture Overview

Este projeto implementa a **Arquitetura Hexagonal (Ports & Adapters)**, também conhecida como **Clean Architecture**, garantindo:

- ✅ Independência de frameworks
- ✅ Testabilidade
- ✅ Manutenibilidade
- ✅ Flexibilidade para mudanças

## Estrutura de Diretórios

```
DanceWaves/
├── Application/                    # 🔷 Núcleo da Aplicação
│   ├── Ports/                      # Interfaces (Portas de Entrada e Saída)
│   │   ├── IEntryPersistencePort.cs
│   │   ├── IUserPersistencePort.cs
│   │   ├── ICompetitionPersistencePort.cs
│   │   └── INavigationPresenterPort.cs
│   └── UseCases/                   # Casos de Uso (Lógica de Negócio)
│       ├── GetNavigationMenuUseCase.cs
│       └── ListEntriesUseCase.cs
│
├── Adapters/                       # 🔶 Adaptadores Externos
│   ├── Persistence/                # Adaptadores de Persistência (BD)
│   │   ├── EntryPersistenceAdapter.cs
│   │   ├── UserPersistenceAdapter.cs
│   │   └── CompetitionPersistenceAdapter.cs (preparado)
│   └── Presenters/                 # Adaptadores de Apresentação (UI)
│       └── NavigationPresenterAdapter.cs
│
├── Components/                     # 🎨 Componentes Blazor
│   ├── Layout/
│   │   └── NavMenu.razor           # Menu dinâmico (Hexagonal)
│   └── Pages/
│       ├── Entries.razor           # 📝 Entries
│       ├── Administration.razor    # ⚙️ Administration
│       ├── SignUp.razor            # 📋 Sign-up
│       └── Registrations.razor     # ✅ Registrations
│
├── Models/                         # 📦 Entidades de Domínio
│   └── *.cs
│
└── Data/                           # 🔌 Camada de Dados (EF Core)
    └── ApplicationDbContext.cs
```

## Fluxo de Dados (Hexagonal Architecture)

```
┌─────────────────────────────────────────────────────────────┐
│                   HEXAGONAL ARCHITECTURE                     │
└─────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────┐
│                    EXTERNA (Interfaces)                          │
│                                                                  │
│  NavMenu.razor  →  Administration.razor  →  Entries.razor       │
│  ↓                                                               │
│  Adaptadores de Apresentação                                    │
│  ↓                                                               │
├──────────────────────────────────────────────────────────────────┤
│                   PORTS (Interfaces)                             │
│                                                                  │
│  INavigationPresenterPort  →  IEntryPersistencePort            │
│  IUserPersistencePort      →  ICompetitionPersistencePort      │
│  ↓                                                               │
├──────────────────────────────────────────────────────────────────┤
│                  NÚCLEO (Use Cases)                              │
│                                                                  │
│  GetNavigationMenuUseCase                                        │
│  ListEntriesUseCase                                              │
│  ↓                                                               │
├──────────────────────────────────────────────────────────────────┤
│                   PORTS (Interfaces)                             │
│                                                                  │
│  IEntryPersistencePort  ←  IUserPersistencePort                │
│  ↓                                                               │
│  Adaptadores de Persistência                                    │
│  ↓                                                               │
├──────────────────────────────────────────────────────────────────┤
│                   EXTERNA (Implementação)                        │
│                                                                  │
│  Entity Framework Core  →  SQL Server (Azure)                  │
│  ↓                                                               │
│  Database                                                       │
└──────────────────────────────────────────────────────────────────┘
```

## Menus Implementados

### 1. **📝 Entries**
- **Rota:** `/entries`
- **Função:** Gerenciar entries de competições
- **Componente:** `Entries.razor`
- **Use Case:** `ListEntriesUseCase`
- **Persistence:** `IEntryPersistencePort`

### 2. **⚙️ Administration**
- **Rota:** `/administration`
- **Função:** Configurações do sistema
- **Submenus:**
  - Users Management
  - Competitions Management
  - System Settings

### 3. **📋 Sign-up**
- **Rota:** `/signup`
- **Função:** Criar nova conta de usuário
- **Formulário:** First Name, Last Name, Email, Phone, Password

### 4. **✅ Registrations**
- **Rota:** `/registrations`
- **Função:** Gerenciar registros de competições
- **Recursos:**
  - Search registrations
  - Status tracking
  - Statistics dashboard

## Dependency Injection (Program.cs)

```csharp
// Registrar Ports (Interfaces)
builder.Services.AddScoped<IEntryPersistencePort, EntryPersistenceAdapter>();
builder.Services.AddScoped<IUserPersistencePort, UserPersistenceAdapter>();
builder.Services.AddScoped<INavigationPresenterPort, NavigationPresenterAdapter>();

// Registrar Use Cases
builder.Services.AddScoped<GetNavigationMenuUseCase>();
builder.Services.AddScoped<ListEntriesUseCase>();
```

## Benefícios da Arquitetura Hexagonal

| Aspecto | Benefício |
|--------|-----------|
| **Independência** | Código de negócio não depende de frameworks |
| **Testabilidade** | Fácil criar mocks das portas |
| **Flexibilidade** | Trocar BD sem afetar casos de uso |
| **Manutenibilidade** | Responsabilidades bem definidas |
| **Escalabilidade** | Adicionar novos adaptadores facilmente |

## Próximos Passos

1. **Implementar CompetitionPersistenceAdapter** completo
2. **Adicionar Use Cases** para CRUD completo
3. **Implementar Testes Unitários** com mocks das portas
4. **Adicionar DTOs** para transferência de dados entre camadas
5. **Implementar Validações** no núcleo (Use Cases)

---

**Arquitetura seguida rigorosamente:** Clean Architecture + Hexagonal Pattern + Ports & Adapters 🚀
