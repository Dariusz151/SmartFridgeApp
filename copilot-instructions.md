# AI Agent Instructions for SmartFridgeApp

## Project Overview

SmartFridgeApp is an application to help manage kitchen products and find recipes for current "kitchen" state.
It tracks expiration dates and can notify about expiring products. It can also prepare some user profile based on his history to suggest future purchases, better shopping habits, and recipes.
It gives user ability to generate some summaries (reports) based on history of usage, so he can track his progress in reducing food waste.

## Purpose of the application

The main goal of the application is to help users reduce food waste by providing insights into their consumption patterns, suggesting recipes based on available ingredients, and notifying them about expiring products. By doing so, it aims to promote sustainable living and help users save money.
I want this app to be open source and free for everyone, so I want to make it as good as possible, so it can be used by many people and also other developers can contribute to it and learn from it.

# Hosting: Google Cloud Platform (GCP) - not defined specific tools yet, will adjust based on needs and budget

## When Adding New Features

1. **Understand the domain** - Read `ARCHITECTURE.md` and relevant context in `docs/`
2. **Use newest version of packages if possible and compatible (keep .dependencies up to date)**
3. **Follow existing patterns** - Check similar features for reference
4. **Respect layer boundaries** - Domain → Application → Infrastructure → API
5. **Write unit tests only for parts with custom logic** - Especially for domain logic and critical paths
6. **Use async/await for all database operations**
7. **Do not over-engineer** - Keep it simple, YAGNI applies
8. **Do not add comments at all **unless\* the code is doing something non-obvious\*\* - Strive for self-explanatory code

**Important:**

- Business logic goes in Domain (Aggregate/Entity methods)

**Important:**

- Use Dapper + raw SQL
- Use Repository pattern
- Use Domain Services for complex logic that doesn't fit in entities, but Application Services for orchestration logic
- Use extension method to transform instead of AutoMapper
- Query views (not tables directly)
- Return DTOs
- NO UnitOfWork for queries

### Code Style

- primary constructors when possible (C# 12)
- use struct, record and so on when possible and makes sense (but stick to DDD rules)

### Error Handling

- Domain exceptions inherit from `DomainException`
- Other exception for example infrastructure use native exceptions or custom ones inheriting from `Exception`
- Caught by `ErrorHandlerMiddleware`
- Mapped to appropriate HTTP status codes

## Testing Guidelines

### Unit Tests

- Test domain logic (entities, aggregates, value objects)
- Use xUnit
- Follow Arrange-Act-Assert pattern

## Key Files to Check

Before making changes, review:

1. `docs/ARCHITECTURE.md` - Architecture overview
2. `docs/CODING_CONVENTIONS.md` - Detailed coding standards
3. `readme.md` - Project overview (but keep in mind this readme is not up to date and may contain outdated information about architecture and stack)

## Tips for AI Agents

1. **Always respect layer boundaries** - Check dependencies before importing
2. **Use existing patterns** - Don't introduce new patterns without discussion
3. **Follow naming conventions** - Consistency is key
4. **Write tests** - Especially for domain logic
5. **Validate appropriately** - Structure in validators, business rules in domain
6. ** Follow the SOLID principles** - Keep code maintainable and extensible
7. ** After changes, review your code like you are a senior developer and expert in the domain - so try to ask yourself critical questions about design, performance, and maintainability **

## Documentation

When adding features:

1. Do not add any comments unless I explicitly ask you to do so - strive for self-explanatory code
2. Do not Update README unless I explicitly ask you to do so
