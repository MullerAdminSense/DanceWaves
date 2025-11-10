# 🎭 DanceWaves - Dance Competition Management System

![.NET](https://img.shields.io/badge/.NET-10.0-purple?style=flat-square)
![C#](https://img.shields.io/badge/C%23-Latest-green?style=flat-square)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Azure-blue?style=flat-square)
![License](https://img.shields.io/badge/License-MIT-red?style=flat-square)

## 📋 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Architecture](#-architecture)
- [Requirements](#-requirements)
- [Installation](#-installation)
- [Folder Structure](#-folder-structure)
- [Database](#-database)
- [How to Use](#-how-to-use)
- [API Endpoints](#-api-endpoints)
- [Development Guide](#-development-guide)
- [Contributing](#-contributing)

---

## 🎯 Overview

**DanceWaves** is a comprehensive web platform for managing dance competitions, including:

- 📝 Entry registration and management
- 👥 User and permission administration
- 💃 Competition and category management
- 📊 Registration dashboard and statistics
- 🔐 Authentication and authorization system with role-based access
- 📱 Modern responsive interface

**Technology Stack:**
- **Backend:** ASP.NET Core 10.0 (Blazor Server)
- **Frontend:** Blazor Interactive (Server + WebAssembly)
- **Database:** SQL Server (Azure)
- **ORM:** Entity Framework Core 8.0.10
- **Architecture:** Hexagonal (Ports & Adapters)

---

## ✨ Features

### 🔐 User System
- 4 User Roles with distinct permissions:
  - **SuperAdmin:** Full system access
  - **FranchiseAdmin:** Manages connected users, competitions, and results
  - **User:** Views own data and enrolled competitions
  - **Jury:** Can enter results for connected competitions

### 🎪 Competition Management
- Create and edit competitions
- Categories by: Style, Age Group, Level, Gender
- Competition Status: Open for Registration, Closed, Completed
- Jury management

### 📝 Entry System
- Enroll teams in categories
- Manage team members
- Payment tracking
- Music upload

### 🏫 School Management
- Register dance schools
- Associate users to schools
- Manage franchises

### 📊 Dashboard and Reports
- Registration statistics
- Payment status tracking
- Results visualization

---

## 🏛️ Architecture

### Hexagonal Architecture (Clean Architecture)

The project strictly follows **Hexagonal Architecture** with **Ports & Adapters**, ensuring:

```
┌─────────────────────────────────────────────────────┐
│           PRESENTATION LAYER (UI)                   │
│  Blazor Components, Razor Pages, ASP.NET Core       │
└──────────────────┬──────────────────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────────────────┐
│       ADAPTERS (Presenters & Persistence)           │
│  NavigationPresenterAdapter                         │
│  EntryPersistenceAdapter, UserPersistenceAdapter    │
└──────────────────┬──────────────────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────────────────┐
│      PORTS (Interfaces - Business Contract)         │
│  INavigationPresenterPort, IEntryPersistencePort    │
│  IUserPersistencePort, ICompetitionPersistencePort  │
└──────────────────┬──────────────────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────────────────┐
│      CORE (Use Cases - Business Logic)              │
│  GetNavigationMenuUseCase, ListEntriesUseCase       │
│  (Framework independent!)                           │
└──────────────────┬──────────────────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────────────────┐
│       ADAPTERS (Entity Framework Core)              │
│  Persistence Implementations                        │
└──────────────────┬──────────────────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────────────────┐
│        DATA LAYER (SQL Server/Azure)                │
│  Tables: Users, Entries, Competitions, etc...       │
└─────────────────────────────────────────────────────┘
```

### Layer Structure

```
DanceWaves/
├── Application/              🔷 CORE (Pure Business Logic)
│   ├── Ports/                📍 Interfaces (Contracts)
│   │   ├── IEntryPersistencePort
│   │   ├── IUserPersistencePort
│   │   ├── ICompetitionPersistencePort
│   │   └── INavigationPresenterPort
│   └── UseCases/             🎯 Use Cases (Orchestration)
│       ├── GetNavigationMenuUseCase
│       └── ListEntriesUseCase
│
├── Adapters/                 🔶 ADAPTERS (Concrete Implementations)
│   ├── Persistence/          💾 Persistence Adapters
│   │   ├── EntryPersistenceAdapter
│   │   ├── UserPersistenceAdapter
│   │   └── CompetitionPersistenceAdapter
│   └── Presenters/           🎨 Presentation Adapters
│       └── NavigationPresenterAdapter
│
├── Components/               🧩 USER INTERFACE (Blazor)
│   ├── Layout/
│   │   ├── MainLayout.razor
│   │   ├── NavMenu.razor     ← Dynamic menu via Use Case
│   │   └── ReconnectModal.razor
│   └── Pages/
│       ├── Entries.razor     📝 Manage Entries
│       ├── Administration.razor ⚙️ Settings
│       ├── SignUp.razor      📋 Create Account
│       └── Registrations.razor ✅ Manage Registrations
│
├── Models/                   📦 DOMAIN ENTITIES
│   ├── User.cs
│   ├── Entry.cs
│   ├── Competition.cs
│   ├── UserRolePermission.cs
│   ├── CompetitionStatus.cs  (Enum)
│   ├── EntryStatus.cs        (Enum)
│   └── ... (11 models total)
│
├── Data/                     🔌 EF CORE LAYER
│   ├── ApplicationDbContext.cs
│   ├── DesignTimeDbContextFactory.cs
│   ├── DatabaseInitializer.cs
│   └── UserRolePermissionSeeder.cs
│
└── Migrations/               📜 Database History
    └── 20251110181952_InitialCreate.cs
```

---

## 🔧 Requirements

### Operating System
- Windows 10+ / MacOS / Linux

### Required Tools
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) or higher
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/sql-server/) or Azure SQL Database connection
- [Git](https://git-scm.com/)

### NuGet Libraries
```xml
<ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.10" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.10" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.10" />
</ItemGroup>
```

---

## 🚀 Installation

### 1. Clone Repository
```bash
git clone https://github.com/seu-usuario/DanceWaves.git
cd DanceWaves
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Configure Connection String
Edit `DanceWaves/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=seu-servidor;Database=DanceWaves;User Id=admin;Password=sua-senha;Trusted_Connection=False;Encrypt=True;"
  }
}
```

### 4. Apply Migrations
```bash
cd DanceWaves
dotnet ef database update
```

### 5. Run Application
```bash
dotnet run
```

The application will be available at: `https://localhost:5001`

---

## 📁 Folder Structure

### Detailed Explanation

#### 1️⃣ `Application/Ports/`
Defines the **contracts (interfaces)** between the core and adapters. Does not depend on any concrete implementation.

```csharp
public interface IEntryPersistencePort
{
    Task<Entry> GetByIdAsync(int id);
    Task<IEnumerable<Entry>> GetAllAsync();
    Task<Entry> CreateAsync(Entry entry);
}
```

#### 2️⃣ `Application/UseCases/`
Contains the **pure business logic**. Each use case orchestrates communication between ports.

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
Concrete implementations of persistence ports using **Entity Framework Core**.

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
Adapters that provide data for the UI (Blazor).

```csharp
public class NavigationPresenterAdapter : INavigationPresenterPort
{
    public async Task<NavigationViewModel> GetNavigationMenuAsync()
    {
        // Returns dynamic menu
    }
}
```

#### 5️⃣ `Components/Pages/`
Razor pages that use Use Cases via dependency injection.

---

## 💾 Database

### Entity-Relationship Diagram

```
┌─────────────────────────────────────────────────────┐
│                  CREATED TABLES                     │
├─────────────────────────────────────────────────────┤

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

### Tables and Fields

| Table | Main Fields | Primary Key |
|-------|-------------|-------------|
| **Users** | Id, Email, FirstName, LastName, RolePermissionId | Id (Identity) |
| **Entries** | Id, CompetitionCategoryId, StartNumber, Status, PaymentStatus | Id (Identity) |
| **Competitions** | Id, Name, Status (Enum), MaxContestants, Location | Id (Identity) |
| **CompetitionCategories** | Id, CompetitionId, StyleId, AgeGroupId, LevelId, GenderMix (Bool) | Id (Identity) |
| **UserRolePermissions** | Id, Name, Description | Id (Identity) |
| **Styles** | Id, Code, Name | Id (Identity) |
| **AgeGroups** | Id, Code, Name, MinAge, MaxAge | Id (Identity) |
| **Levels** | Id, Code, Name | Id (Identity) |

### Seed Data (Initial Data)

The application automatically inserts 4 roles on startup:

```sql
INSERT INTO UserRolePermissions (Name, Description) VALUES
('SuperAdmin', 'Sees everything'),
('FranchiseAdmin', 'Manages all connected users, contests, results'),
('User', 'Sees his own data and joined contests'),
('Jury', 'Can put results in the system per connected contest');
```

---


## 🔐 Authentication & Login

### Supported Authentication Methods
- **Local Account:** Register and login with email and password
- **Federated Login:** Microsoft, Google, and Apple (via Microsoft Entra External ID / Azure AD B2C)

### How to Register
1. Go to `/register` or click the **Register** button in the navigation bar.
2. Fill in your details (First Name, Last Name, Email, Password, Accept Terms).
3. Submit the form. You will be redirected to login after successful registration.

### How to Login
1. Go to `/login` or click the **Login** button in the navigation bar.
2. Enter your email and password for local accounts, or use one of the federated login buttons (Microsoft, Google, Apple).
3. On successful login, you will be redirected to the home page.

### Profile Management
- Access your profile at `/profile`.
- Update your personal information and change your password (local accounts only).
- Federated accounts display provider info and do not allow password changes.

### Logout
- Click the **Logout** button in the navigation bar to securely sign out and clear your session.

### Authentication State
- The navigation bar updates automatically based on your authentication state (shows Login/Register or Profile/Logout).
- Authentication state is managed using JWT tokens stored in browser local storage.

### Secure API Calls
To make authenticated API requests from Blazor components or services:

```csharp
@inject IHttpClientFactory HttpClientFactory

@code {
    private async Task CallSecureApi()
    {
        var client = HttpClientFactory.CreateClient("SecureApiClient");
        var response = await client.GetAsync("https://your-api-endpoint/protected-resource");
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            // Handle your data
        }
        else
        {
            // Handle error
        }
    }
}
```
- The access token is automatically attached to requests.
- Use `"SecureApiClient"` as the named client for all authenticated API calls.

---

## 📖 How to Use

### 🏠 Home Page
Access `https://localhost:5001` to see the home page with dynamic menu.

### 📝 Entries Menu
- **Route:** `/entries`
- **Icon:** 📝
- View all registered entries
- Click "Edit" to modify or "Delete" to remove

### ⚙️ Administration Menu
- **Route:** `/administration`
- **Icon:** ⚙️
- **Submenus:**
  - Users Management
  - Competitions Management
  - System Settings
- Manage system settings

### 📋 Sign-up Menu
- **Route:** `/signup`
- **Icon:** 📋
- Complete form to create new account
- Fields: Name, Email, Phone, Password
- Terms of service validation

### ✅ Registrations Menu
- **Route:** `/registrations`
- **Icon:** ✅
- Registrations dashboard
- Search filter
- Statistics: Total, Pending, Approved, Rejected

---

## 🔌 API Endpoints

### Future: REST API

When implemented, endpoints will follow RESTful pattern:

```http
# Entries
GET    /api/entries              - List all entries
GET    /api/entries/{id}         - Get specific entry
POST   /api/entries              - Create new entry
PUT    /api/entries/{id}         - Update entry
DELETE /api/entries/{id}         - Delete entry

# Users
GET    /api/users                - List all users
GET    /api/users/{id}           - Get specific user
POST   /api/users/signup         - Create new user
PUT    /api/users/{id}           - Update user
DELETE /api/users/{id}           - Delete user

# Competitions
GET    /api/competitions         - List competitions
POST   /api/competitions         - Create competition
PUT    /api/competitions/{id}    - Update competition
DELETE /api/competitions/{id}    - Delete competition
```

---

## 👨‍💻 Development Guide

### Add New Use Case

**Step 1:** Create the port (interface)
```csharp
// Application/Ports/IMyNewPort.cs
public interface IMyNewPort
{
    Task<MyEntity> GetByIdAsync(int id);
}
```

**Step 2:** Create the use case
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

**Step 3:** Create the adapter
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

**Step 4:** Register in DI (Program.cs)
```csharp
builder.Services.AddScoped<IMyNewPort, MyNewAdapter>();
builder.Services.AddScoped<MyNewUseCase>();
```

### Run Tests

```bash
# Run all tests
dotnet test

# Specific test
dotnet test --filter "TestClass.TestMethod"
```

### Build Application

```bash
# Debug
dotnet build

# Release
dotnet build -c Release

# Build and publish
dotnet publish -c Release -o ./publish
```

### Manage Migrations

```bash
# Create new migration
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# Remove last migration
dotnet ef migrations remove

# List migrations
dotnet ef migrations list
```

---

## 📚 Useful Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Blazor Tutorial](https://docs.microsoft.com/en-us/aspnet/core/blazor/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Hexagonal Architecture](https://alistair.cockburn.us/hexagonal-architecture/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

---

### Code Standards

- Use **PascalCase** for class and method names
- Use **camelCase** for local variables
- Always use **async/await** for I/O operations
- Document public classes and methods with **XML Comments**

---

## 📄 License

This project is under the **MIT License**. See the [LICENSE](LICENSE) file for more details.

---

## 📞 Support

To report bugs or suggest features, open an [Issue](https://github.com/seu-usuario/DanceWaves/issues).

---

<div align="center">

### 💜 If you found this project useful, please give it a ⭐!

**DanceWaves** - Transforming the Dance World with Technology 🎭✨

</div>
