Prerequisites
- Visual Studio (Windows/Linux/Mac)
    - .net core 3.1.2 installed
- Docker (Linux container)
- SQL Server Management Studio
    - To access docker contained SQL-Server


Structure
- WebAPI 
    - Controllers
    - Configurations files
        - C# project configurations
        - Docker configurations
    - SwaggerStartup
    - Startup

- Data
    - Contains
        - Models
        - DBContext
- Service
    - Providers
    - Handlers

- Tests
    - Automation tests with specflow

- Core
    - Generic Provider
    - Generic Handlers
        - Error handlers
    - Generic Models
        - Base Entity
        - Validation Entity
    - Generic classes
        - Exceptions

Migration
=============================================================
Install dot net core ef tools
- dotnet tool install --global dotnet-ef --version 3.1.2

Show Package Manager Console 
- View > Other Windows > Package Manager Console 

Migrate database Steps (From Package Manager Console)
1. cd Kantin //Make sure that you are in the project folder
2. dotnet ef migrations add "Name of migration" //Create migration

Roadmaps
==============================================================
API
- Menu items
- Add on items
    - Link to menu item
- Menu
    - Containing menu items
- Unit tests
- Account
    - Login
    - Register
    - JWT token handlers
    - Authentication type
        - Anonymous
        - Registered
- Order
    - Link with Menu item and Add on item
- Web socket nofications


Flutter App
- Create a robust data management 
- Create login & register screen
- Create menu screen
