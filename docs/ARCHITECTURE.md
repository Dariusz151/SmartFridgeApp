# Architecture Documentation

## Technology stack

- .NET 10
- EF Core, Dapper
- PostgreSQL
- Redis
- MongoDB
- Docker and Docker compose for local development
- React for UI

## Architectural Patterns

### 1. Clean/Onion Architecture

The application is organized into concentric layers with strict dependency rules:

```
┌─────────────────────────────────────┐
│         API Layer                   │  ← Presentation/Endpoints
├─────────────────────────────────────┤
│    Infrastructure Layer             │  ← Database, External Services
├─────────────────────────────────────┤
│    Application Layer                │  ← Use Cases, application logic across domains
├─────────────────────────────────────┤
│    Domain Layer                     │  ← Business Logic, Entities, Value Objects
└─────────────────────────────────────┘
```

## Core Principles

**Dependency Rules:**

- Domain has NO dependencies on other layers
- Keep Clean Architecture/Onion pattern
- Infrastructure depends on Application and Domain
- API depends on Application and Infrastructure

**Never:**

- Import Infrastructure in Domain
- Import API in Domain or Application
- Have circular dependencies

** Tips: **

- use native solutions from .NET and Microsoft instead of external packages
- refactor existing code if there is alternative with native packages (like Newtonsoft -> Text.Json, Autofac -> DependecyInjection package)
- after each change check if its not over-engineering

### 3. Domain-Driven Design (DDD)

** Use DDD patterns and rules but do not overhead and over-engineer **
** If something doesnt make sense to do with DDD, ask me what to do **

**Building Blocks:**

#### Aggregates

#### Entities

#### Value Objects

#### Domain Events

## Deployment

### Docker Support

- Dockerfile included when applicable
- Docker Compose for infrastructure
- Docker Compose for local dev (all services and infra)
- keep updated .tasks.json for vscode startup
- Multi-stage build for optimization

### Tasks Available

- `infrastructure up` - Start infrastructure services
- `smartfridgeapp up` - Start full application stack

## Key Design Decisions

- easy to modify and adding new components/modules
