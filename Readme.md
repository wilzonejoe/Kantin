Prerequisites
- Visual Studio (Windows/Linux/Mac)
    - .net core 3 installed
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
        - Unit tests

    - Core
        - Generic Provider
        - Generic Handlers
            - Error handlers
        - Generic Models
            - Base Entity
            - Validation Entity
        - Generic classes
            - Exceptions

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
            - Anynomous
            - Registered
    - Order
        - Link with Menu item and Add on item
    - Web socket nofications


Flutter App
    - Create a robust data management 
    - Create login & register screen
    - Create menu screen
