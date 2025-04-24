# CoinKeeper Technical Context Analysis

## Technologies to Analyze

### Core Framework
- **.NET 9**: Analyze usage of latest preview version features (as confirmed in global.json)
- **ASP.NET Core**: Evaluate implementation of web API framework patterns

### Data Access
- **Entity Framework Core**: Assess ORM usage patterns and best practices
- **PostgreSQL**: Evaluate database schema design and PostgreSQL-specific features
- **EF Core Migrations**: Analyze migration strategy and versioning approach

### API Development
- **ASP.NET Core Web API**: Evaluate RESTful API implementation patterns
- **NSwag**: Assess Swagger/OpenAPI documentation completeness and accuracy
- **FluentValidation**: Analyze validation rules and implementation patterns

### Object Mapping
- **AutoMapper**: Evaluate mapping configurations and transformation patterns

### Logging & Monitoring
- **Serilog**: Assess structured logging implementation (configured but not fully implemented)
- **Prometheus/Grafana**: Analyze planned monitoring approach (not yet implemented)

### Containerization
- **Docker**: Evaluate container configuration and best practices
- **Docker Compose**: Assess multi-container orchestration setup

### Authentication (Planned)
- **JWT**: Analyze planned JSON Web Tokens authentication approach
- **ASP.NET Core Identity**: Evaluate planned user management implementation

## Development Environment Analysis

### Required Tools Analysis
- **.NET 9 SDK**: Assess compatibility with preview version
- **PostgreSQL**: Evaluate database setup and configuration
- **Docker**: Analyze containerization approach
- **IDE**: Assess project configuration for Visual Studio or VS Code

### Project Structure Analysis
```
CoinKeeper.Backend/
├── src/
│   ├── CoinKeeper.Backend.Api/        # API host project
│   ├── CoinKeeper.Operations/         # Operations module
│   └── Common/
│       ├── CoinKeeper.Common/         # Shared utilities
│       └── CoinKeeper.Infrastructure/ # Data access layer
├── global.json                        # .NET SDK version
└── CoinKeeper.Backend.sln             # Solution file
```

Evaluate the organization of projects, namespaces, and code files for adherence to separation of concerns and modularity principles.

### Configuration Analysis
- **appsettings.json**: Evaluate application configuration structure and completeness
- **appsettings.Development.json**: Assess development-specific settings approach
- **Dockerfile**: Analyze container configuration best practices
- **.dockerignore**: Evaluate exclusion patterns for Docker context

## Technical Constraint Analysis

### Database
- Evaluate PostgreSQL-only approach implications
- Assess impact of empty connection string in configuration
- Analyze Entity Framework Core Code-First approach implementation

### API Design
- Evaluate adherence to RESTful API conventions (currently not fully implemented)
- Assess JSON format usage for API payloads
- Analyze Swagger/OpenAPI documentation completeness

### Authentication
- Evaluate configured but not implemented middleware
- Assess planned JWT-based authentication approach

### Deployment
- Analyze Docker-based deployment configuration
- Evaluate implications of missing CI/CD pipeline

## Dependency Analysis

### NuGet Package Usage
- Microsoft.EntityFrameworkCore: Evaluate version and usage patterns
- Microsoft.EntityFrameworkCore.Design: Assess design-time tools usage
- Npgsql.EntityFrameworkCore.PostgreSQL: Analyze PostgreSQL provider implementation
- FluentValidation: Evaluate validation implementation patterns
- FluentValidation.DependencyInjectionExtensions: Assess DI integration
- AutoMapper: Analyze mapping configurations
- AutoMapper.Extensions.Microsoft.DependencyInjection: Evaluate DI integration
- NSwag.AspNetCore: Assess API documentation implementation
- Serilog: Evaluate logging configuration and usage

## Tool Usage Pattern Analysis

### Entity Framework Core
- Evaluate Code-First approach with migrations implementation
- Assess entity type configurations for schema definition
- Analyze DbContext usage patterns and best practices

### Dependency Injection
- Evaluate service registration in Startup.ConfigureServices
- Assess extension methods for module registration
- Analyze service lifetime scoping (mostly scoped lifetime)

### Validation
- Evaluate FluentValidation implementation for request validation
- Assess validator registration with dependency injection
- Analyze validation execution in handlers

### Error Handling
- Evaluate centralized error middleware implementation
- Assess CommonErrorException usage for business logic errors
- Analyze structured error response patterns

### API Documentation
- Evaluate Swagger UI implementation for interactive documentation
- Assess OpenAPI specification generation completeness
