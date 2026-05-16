# 🏋️ Gym Management System

A comprehensive ASP.NET Core MVC application for managing gym operations, built with Clean N-Tier Architecture, advanced Entity Framework Core patterns, and enterprise-grade logging infrastructure.

[![.NET Version](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![EF Core](https://img.shields.io/badge/EF%20Core-10.0.8-512BD4)](https://docs.microsoft.com/en-us/ef/core/)
[![Autofac](https://img.shields.io/badge/Autofac-9.1.0-00ADD8)](https://autofac.org/)
[![Serilog](https://img.shields.io/badge/Serilog-10.0.0-0080FF)](https://serilog.net/)

---

## 📋 Table of Contents

- [Project Overview](#-project-overview)
- [Architecture](#-architecture)
- [Key Features](#-key-features)
- [Technology Stack](#-technology-stack)
- [Project Structure](#-project-structure)
- [Core Implementation Details](#-core-implementation-details)
  - [1. Clean N-Tier Architecture](#1-clean-n-tier-architecture)
  - [2. Autofac Dependency Injection](#2-autofac-dependency-injection)
  - [3. Dynamic Soft Delete Pattern](#3-dynamic-soft-delete-pattern)
  - [4. Secure Configuration Management](#4-secure-configuration-management)
  - [5. Background Job Processing](#5-background-job-processing)
  - [6. Structured Logging](#6-structured-logging)
- [Database Schema](#-database-schema)
- [Getting Started](#-getting-started)
- [Configuration](#-configuration)
- [Development Guidelines](#-development-guidelines)

---

## 🎯 Project Overview

The Gym Management System is an enterprise-level web application designed to streamline gym operations including member management, trainer scheduling, session bookings, and membership plans. The system implements advanced software engineering patterns and practices suitable for production environments.

### Business Domain

- **Member Management**: Track member profiles, health records, and membership status
- **Trainer Management**: Manage trainer profiles, specializations, and availability
- **Session Scheduling**: Create and manage training sessions with booking capabilities
- **Membership Plans**: Define flexible membership plans with pricing and duration
- **Category Management**: Organize sessions and trainers by categories

---

## 🏗️ Architecture

### Clean N-Tier Architecture

The application follows a strict **3-layer architecture** with clear separation of concerns:

```
┌─────────────────────────────────────────────────────────────┐
│                    Presentation Layer                        │
│              (ASP.NET Core MVC - Port 5000)                  │
│  Controllers │ Views │ ViewModels │ Exception Handlers       │
└────────────────────────┬────────────────────────────────────┘
                         │ depends on
                         ↓
┌─────────────────────────────────────────────────────────────┐
│                   Infrastructure Layer                       │
│         (Data Access, External Services, Jobs)               │
│  DbContext │ Repositories │ Configurations │ Interceptors    │
│  Background Jobs │ Migrations │ Seeding                      │
└────────────────────────┬────────────────────────────────────┘
                         │ depends on
                         ↓
┌─────────────────────────────────────────────────────────────┐
│                      Domain Layer                            │
│              (Business Logic & Entities)                     │
│  Entities │ Value Objects │ Enums │ Repository Interfaces    │
│  (Currently integrated with Infrastructure)                  │
└─────────────────────────────────────────────────────────────┘
```

### Layer Responsibilities

#### **Presentation Layer** (`GymManagement.Presentation`)
- ASP.NET Core MVC controllers and views
- User interface and HTTP request handling
- Exception handling middleware
- Static assets and client-side resources

#### **Infrastructure Layer** (`GymManagement.Infrastructure`)
- Entity Framework Core DbContext and configurations
- Repository implementations
- Database migrations and seeding
- EF Core interceptors for cross-cutting concerns
- Background services for scheduled tasks
- Autofac IoC container modules

#### **Domain Layer** (`GymManagement.Domain`)
- Domain entities and business models
- Value objects and enumerations
- Repository interfaces (contracts)
- Domain service orchestration

---

## ✨ Key Features

### 1. **Advanced Soft Delete Pattern**
- Global query filters automatically exclude soft-deleted records
- EF Core interceptors handle audit timestamps
- Configurable hard delete for administrative operations
- Periodic cleanup job removes old soft-deleted records

### 2. **Autofac IoC Container**
- Custom dependency injection modules per layer
- Lifetime scope management (Scoped, Transient, Singleton)
- Clean separation from built-in DI container

### 3. **Structured Logging with Serilog**
- Centralized logging to Console and Seq
- Structured log enrichment (Machine Name, Thread ID, Context)
- Application-wide exception logging
- Performance and diagnostic insights

### 4. **Background Job Processing**
- Periodic data cleanup using `BackgroundService`
- Scoped service resolution within background tasks
- Graceful shutdown with cancellation token support

### 5. **Secure Configuration**
- User Secrets for sensitive data (connection strings, API keys)
- Environment-specific configuration files
- Configuration validation at startup

### 6. **Entity Framework Core Best Practices**
- Fluent API configurations in separate files
- Table-Per-Hierarchy (TPH) inheritance for User entities
- Database constraints and indexes
- Owned entities for value objects

---

## 🛠️ Technology Stack

### Core Framework
- **.NET 10.0** - Latest LTS framework
- **ASP.NET Core MVC** - Web application framework
- **C# 13** - Programming language

### Data Access
- **Entity Framework Core 10.0.8** - ORM
- **SQL Server** - Relational database
- **EF Core Interceptors** - Cross-cutting concerns

### Dependency Injection
- **Autofac 9.1.0** - IoC container
- **Autofac.Extensions.DependencyInjection 11.0.0** - ASP.NET Core integration

### Logging
- **Serilog.AspNetCore 10.0.0** - Structured logging
- **Serilog.Sinks.Seq 9.1.0** - Centralized log aggregation
- **Serilog.Sinks.Console** - Development logging

### Package Management
- **Central Package Management (CPM)** - .NET 10 feature for centralized NuGet version control

---

## 📁 Project Structure

```
GeymManagement/
│
├── GeymManagement/                          # Presentation Layer
│   ├── Controllers/
│   │   ├── HomeController.cs
│   │   └── PlansController.cs
│   ├── Views/
│   │   ├── Home/
│   │   ├── Plans/
│   │   └── Shared/
│   ├── ExceptionHandlers/
│   │   └── CustomExceptionHandler.cs
│   ├── wwwroot/                             # Static files
│   ├── Program.cs                           # Application entry point
│   ├── appsettings.json                     # Configuration
│   └── appsettings.Development.json
│
├── GeymInfrastructure/                      # Infrastructure Layer
│   ├── Data/
│   │   ├── DbContexts/
│   │   │   └── GymDbContext.cs              # EF Core DbContext
│   │   ├── Configurations/                  # Fluent API configurations
│   │   │   ├── PlanConfiguration.cs
│   │   │   ├── CategoryConfiguration.cs
│   │   │   ├── UserConfiguration.cs
│   │   │   ├── MemberConfiguration.cs
│   │   │   ├── TrainerConfiguration.cs
│   │   │   ├── SessionConfiguration.cs
│   │   │   ├── BokingConfiguration.cs
│   │   │   ├── MemberShipConfiguration.cs
│   │   │   └── HealthRecordConfiguration.cs
│   │   └── Interceptors/
│   │       └── AuditSaveChangesInterceptor.cs
│   ├── Models/                              # Domain entities
│   │   ├── BaseEntity.cs
│   │   ├── Plan.cs
│   │   ├── Category.cs
│   │   ├── User.cs
│   │   ├── Member.cs
│   │   ├── Trainer.cs
│   │   ├── Session.cs
│   │   ├── Boking.cs
│   │   ├── MemberShip.cs
│   │   └── HealthRecord.cs
│   ├── Enums/
│   │   ├── Gender.cs
│   │   ├── BloodType.cs
│   │   └── Speciality.cs
│   ├── ValueObjects/
│   │   └── Address.cs
│   ├── Repositories/
│   │   ├── IPlanRepository.cs
│   │   └── PlanRepository.cs
│   ├── BackgroundJobs/
│   │   └── DataCleanupJob.cs                # Periodic cleanup service
│   ├── IOC/
│   │   └── InfrastructureModule.cs          # Autofac module
│   ├── Migrations/                          # EF Core migrations
│   ├── Seed/
│   │   └── DatabaseSeeder.cs
│   └── DependencyInjection.cs
│
├── GeymDomain/                              # Domain Layer
│   ├── Repositories/                        # Repository interfaces
│   └── DependencyInjection.cs
│
├── Directory.Build.props                    # Shared MSBuild properties
├── Directory.Packages.props                 # Central Package Management
└── GeymManagement.slnx                      # Solution file
```

---

## 🔧 Core Implementation Details

### 1. Clean N-Tier Architecture

#### Layer Separation Strategy

**Presentation Layer** (`GymManagement.Presentation.csproj`)
```xml
<ItemGroup>
  <ProjectReference Include="..\GeymDomain\GymManagement.Domain.csproj" />
  <ProjectReference Include="..\GeymInfrastructure\GymManagement.Infrastructure.csproj" />
</ItemGroup>
```

**Infrastructure Layer** (`GymManagement.Infrastructure.csproj`)
```xml
<ItemGroup>
  <!-- No project references - contains implementations -->
</ItemGroup>
```

**Domain Layer** (`GymManagement.Domain.csproj`)
```xml
<ItemGroup>
  <ProjectReference Include="..\GeymInfrastructure\GymManagement.Infrastructure.csproj" />
</ItemGroup>
```

#### Dependency Flow
- Presentation depends on Domain and Infrastructure
- Infrastructure contains data access implementations
- Domain defines contracts and business entities

---

### 2. Autofac Dependency Injection

#### Configuration in Program.cs

```csharp
// Replace default DI container with Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Register Autofac modules
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new InfrastructureModule());
});
```

#### Infrastructure Module Implementation

```csharp
public class InfrastructureModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // Register repositories with scoped lifetime
        builder.RegisterType<PlanRepository>()
               .As<IPlanRepository>()
               .InstancePerLifetimeScope();

        // Register background services
        builder.RegisterType<DataCleanupJob>()
               .As<IHostedService>()
               .InstancePerDependency();
    }
}
```

#### Benefits
- **Modular Registration**: Each layer can define its own Autofac module
- **Lifetime Management**: Explicit control over service lifetimes
- **Advanced Features**: Property injection, decorators, interceptors
- **Testability**: Easy mocking and test container configuration

---

### 3. Dynamic Soft Delete Pattern

Our soft delete implementation uses a **three-tier approach**:

#### Tier 1: BaseEntity with Audit Fields

```csharp
public class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
}
```

#### Tier 2: Global Query Filters

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Apply to all entities inheriting from BaseEntity
    modelBuilder.Entity<User>()
        .HasQueryFilter(b => !b.IsDeleted);
    
    modelBuilder.Entity<Plan>()
        .HasQueryFilter(p => !p.IsDeleted);
    
    // ... applied to all entities
}
```

**Effect**: All queries automatically exclude soft-deleted records without explicit `Where()` clauses.

#### Tier 3: SaveChanges Override with Hard Delete Flag

```csharp
public bool AllowHardDelete { get; set; }

public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
{
    if (!AllowHardDelete)
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Deleted)
            {
                var isDeletedProp = entry.Entity.GetType().GetProperty("IsDeleted");
                if (isDeletedProp != null)
                {
                    entry.State = EntityState.Modified;
                    isDeletedProp.SetValue(entry.Entity, true);
                }
            }
        }
    }
    return base.SaveChangesAsync(cancellationToken);
}
```

**Behavior**:
- Normal delete operations → Soft delete (sets `IsDeleted = true`)
- When `AllowHardDelete = true` → Physical database deletion

#### Tier 4: EF Core Interceptor for Audit Timestamps

```csharp
public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private static void UpdateAuditProperties(DbContext? context)
    {
        var gymContext = context as GymDbContext;
        var allowHardDelete = gymContext?.AllowHardDelete ?? false;
        var now = DateTime.UtcNow;

        var entries = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Added || 
                       e.State == EntityState.Modified || 
                       e.State == EntityState.Deleted)
            .ToList();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.IsDeleted = false;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = now;
                    entry.Property(x => x.CreatedAt).IsModified = false;
                    break;

                case EntityState.Deleted:
                    if (allowHardDelete) break;
                    
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = now;
                    entry.Entity.UpdatedAt = now;
                    entry.Property(x => x.CreatedAt).IsModified = false;
                    break;
            }
        }
    }
}
```

**Registration**:
```csharp
services.AddDbContext<GymDbContext>((sp, options) =>
{
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    options.AddInterceptors(sp.GetRequiredService<AuditSaveChangesInterceptor>());
});
```

#### Benefits
- ✅ **Automatic**: No manual timestamp management
- ✅ **Consistent**: All entities follow same audit pattern
- ✅ **Recoverable**: Soft-deleted data can be restored
- ✅ **Compliant**: Audit trail for regulatory requirements
- ✅ **Flexible**: Hard delete available when needed

---

### 4. Secure Configuration Management

#### User Secrets for Development

```bash
# Initialize user secrets
dotnet user-secrets init --project GeymManagement

# Set connection string
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=...;Database=GymManagement;..."
```

#### Configuration Hierarchy

1. **appsettings.json** - Base configuration
2. **appsettings.Development.json** - Development overrides
3. **User Secrets** - Sensitive data (not committed to Git)
4. **Environment Variables** - Production deployment

#### Implementation

```csharp
// Program.cs automatically loads configuration in order:
var builder = WebApplication.CreateBuilder(args);

// Configuration sources (in order of precedence):
// 1. appsettings.json
// 2. appsettings.{Environment}.json
// 3. User Secrets (Development only)
// 4. Environment Variables
// 5. Command-line arguments
```

#### Project Configuration

```xml
<!-- GymManagement.Presentation.csproj -->
<PropertyGroup>
  <UserSecretsId>ca7206e1-d926-4e2c-a58f-39f90f170ca2</UserSecretsId>
</PropertyGroup>
```

---

### 5. Background Job Processing

#### DataCleanupJob Implementation

```csharp
public class DataCleanupJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<DataCleanupJob> _logger;
    private readonly TimeSpan _cleanupInterval = TimeSpan.FromDays(30);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Data Cleanup Job is starting...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<GymDbContext>();
                    
                    // Enable hard delete for cleanup
                    context.AllowHardDelete = true;

                    try
                    {
                        // Query soft-deleted records (bypass query filters)
                        var deletedPlans = context.Plans
                            .IgnoreQueryFilters()
                            .Where(p => p.IsDeleted)
                            .ToList();

                        if (deletedPlans.Any())
                        {
                            context.Plans.RemoveRange(deletedPlans);
                            _logger.LogInformation("Removed {Count} soft-deleted Plans.", 
                                deletedPlans.Count);
                        }

                        await context.SaveChangesAsync(stoppingToken);
                    }
                    finally
                    {
                        context.AllowHardDelete = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the data cleanup process.");
            }

            await Task.Delay(_cleanupInterval, stoppingToken);
        }
    }
}
```

#### Key Features
- **Scoped Service Resolution**: Creates new scope for each iteration
- **Graceful Shutdown**: Respects `CancellationToken`
- **Error Resilience**: Continues running after exceptions
- **Structured Logging**: Detailed operation logs
- **Configurable Interval**: 30-day cleanup cycle

#### Registration

```csharp
// Autofac Module
builder.RegisterType<DataCleanupJob>()
       .As<IHostedService>()
       .InstancePerDependency();
```

---

### 6. Structured Logging

#### Serilog Configuration (appsettings.json)

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.AspNetCore": "Warning",
        "Microsoft.Hosting.Lifetime": "Information"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "Seq",
        "Args": { "serverUrl": "http://localhost:5341" }
      }
    ],
    "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ],
    "Properties": {
      "Application": "GymManagementSystem"
    }
  }
}
```

#### Program.cs Setup

```csharp
// Build logger from configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

try
{
    Log.Information("Gym Management System is starting up... 🚀");
    
    // Replace default logger with Serilog
    builder.Host.UseSerilog();
    
    // ... application setup
    
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "The Application failed to start correctly! ❌");
}
finally
{
    Log.CloseAndFlush();
}
```

#### Structured Logging Example

```csharp
_logger.LogInformation("Removed {Count} soft-deleted Plans.", deletedPlans.Count);
// Output: Removed 5 soft-deleted Plans.
// Seq receives: { "Count": 5, "Message": "Removed {Count} soft-deleted Plans." }
```

#### Benefits
- **Centralized Logs**: All logs aggregated in Seq
- **Searchable**: Query logs by properties
- **Contextual**: Machine name, thread ID, application name
- **Performance**: Async logging doesn't block requests
- **Production-Ready**: Fatal error logging with graceful shutdown

---

## 🗄️ Database Schema

### Core Entities

#### **Plan**
- Membership plans with pricing and duration
- Soft delete enabled
- Check constraint: `DurationInDays BETWEEN 1 AND 365`

#### **Category**
- Session and trainer categorization
- Soft delete enabled

#### **User** (Abstract - TPH Inheritance)
- Base class for Member and Trainer
- Discriminator column: `UserType`
- Owned entity: `Address` (Value Object)
- Unique constraints: Email, Phone
- Check constraint: Phone validation (11 digits, Egyptian format)

#### **Member** : User
- Gym member profile
- One-to-One: `HealthRecord`
- One-to-Many: `MemberShips`, `Bokings`

#### **Trainer** : User
- Trainer profile with specialization
- One-to-Many: `Sessions`

#### **Session**
- Training sessions with capacity and pricing
- Many-to-One: `Trainer`, `Category`
- One-to-Many: `Bokings`

#### **Boking** (Booking)
- Session reservations
- Many-to-One: `Member`, `Session`
- Tracks attendance status

#### **MemberShip**
- Active memberships linking members to plans
- Many-to-One: `Member`, `Plan`
- Tracks start/end dates and payment status

#### **HealthRecord**
- Member health information
- One-to-One: `Member`
- Stores blood type, weight, height, medical conditions

---

## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB, Express, or Full)
- [Seq](https://datalust.co/seq) (Optional - for log aggregation)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd GeymManagement
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Configure User Secrets**
   ```bash
   cd GeymManagement
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\\mssqllocaldb;Database=GymManagement;Trusted_Connection=True;MultipleActiveResultSets=true"
   ```

4. **Apply database migrations**
   ```bash
   dotnet ef database update --project GeymInfrastructure --startup-project GeymManagement
   ```

5. **Run the application**
   ```bash
   dotnet run --project GeymManagement
   ```

6. **Access the application**
   - Web UI: `https://localhost:5001`
   - Seq Dashboard: `http://localhost:5341` (if running)

---

## ⚙️ Configuration

### Connection Strings

**Development** (User Secrets):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GymManagement;Trusted_Connection=True;"
  }
}
```

**Production** (Environment Variables):
```bash
export ConnectionStrings__DefaultConnection="Server=prod-server;Database=GymManagement;User Id=sa;Password=***;"
```

### Serilog Seq Setup

1. **Install Seq** (Windows):
   ```bash
   choco install seq
   ```

2. **Start Seq**:
   ```bash
   seq run
   ```

3. **Access Seq Dashboard**: `http://localhost:5341`

### Background Job Configuration

Modify cleanup interval in `DataCleanupJob.cs`:
```csharp
private readonly TimeSpan _cleanupInterval = TimeSpan.FromDays(30); // Change as needed
```

---

## 📚 Development Guidelines

### Adding a New Entity

1. **Create entity class** in `GeymInfrastructure/Models/`
   ```csharp
   public class Equipment : BaseEntity
   {
       public string Name { get; set; }
       public string Description { get; set; }
   }
   ```

2. **Create EF configuration** in `GeymInfrastructure/Data/Configurations/`
   ```csharp
   public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
   {
       public void Configure(EntityTypeBuilder<Equipment> builder)
       {
           builder.Property(e => e.Name).HasMaxLength(100);
           builder.HasQueryFilter(e => !e.IsDeleted);
       }
   }
   ```

3. **Add DbSet** to `GymDbContext`
   ```csharp
   public DbSet<Equipment> Equipment { get; set; }
   ```

4. **Create migration**
   ```bash
   dotnet ef migrations add AddEquipment --project GeymInfrastructure --startup-project GeymManagement
   ```

### Repository Pattern

1. **Define interface** in `GeymInfrastructure/Repositories/`
   ```csharp
   public interface IEquipmentRepository
   {
       Task<IEnumerable<Equipment>> GetAllAsync();
       Task<Equipment?> GetByIdAsync(int id);
       void Add(Equipment equipment);
       void Update(Equipment equipment);
       void Delete(Equipment equipment);
       Task<int> SaveChangesAsync();
   }
   ```

2. **Implement repository**
   ```csharp
   internal class EquipmentRepository : IEquipmentRepository
   {
       private readonly GymDbContext _context;
       
       public EquipmentRepository(GymDbContext context)
       {
           _context = context;
       }
       
       // Implementation...
   }
   ```

3. **Register in Autofac module**
   ```csharp
   builder.RegisterType<EquipmentRepository>()
          .As<IEquipmentRepository>()
          .InstancePerLifetimeScope();
   ```

---

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes using Conventional Commits
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Commit Message Format

```
<type>(<scope>): <subject>

<body>

<footer>
```

**Types**: `feat`, `fix`, `docs`, `style`, `refactor`, `test`, `chore`

**Example**:
```
feat(soft-delete): implement global query filters for all entities

- Add HasQueryFilter to all entity configurations
- Update DbContext SaveChangesAsync to handle soft deletes
- Add AllowHardDelete flag for administrative operations

Closes #123
```

---

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

---

## 👥 Authors

- **Development Team** - Initial work and architecture

---

## 🙏 Acknowledgments

- Clean Architecture principles by Robert C. Martin
- Entity Framework Core documentation and best practices
- Autofac community and documentation
- Serilog structured logging patterns

---

**Built with ❤️ using .NET 10 and Clean Architecture principles**
