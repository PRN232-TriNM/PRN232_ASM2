# EVCS Project

This project is an Electric Vehicle Charging Station (EVCS) management system built with modern .NET technologies.

## Tech Stack

### Backend
- **Framework**: .NET 8 (ASP.NET Core Web API)
- **API**: GraphQL (using HotChocolate 15)
- **Database ORM**: Entity Framework Core 8.0.5
- **Database**: SQL Server
- **Real-time Communication**: SignalR
- **Documentation**: Swagger/OpenAPI

### Frontend
- **Framework**: Blazor WebAssembly (.NET 8)
- **API Client**: GraphQL.Client
- **State/Storage**: Blazored.LocalStorage
- **Authentication**: JWT (JSON Web Tokens)

### Architecture & Patterns
- **Repository Pattern**: Abstraction layer for data access.
- **Unit of Work**: Manages transaction management and database context.
- **Clean Architecture Principles**: Separation of concerns between API, Services, and Repositories.
