# About

This is a sample solution that consists of:
* GraphQL based Web API (powered by [HotChocolate](https://chillicream.com/docs/hotchocolate))
* Frontend (powered by Blazor WebAssembly)

## WebApi (Backend)

Is a set of four projects:
1. BananaChips.API - it's WebAPI itself, used mainly for configuring the app.
2. BananaChips.Application - it's a project, that contains handlers for specific actions (powered by [MediatR](https://github.com/jbogard/MediatR))
3. BananaChips.Domain - contains domain models that are stored in the database and interacted with.
4. BananaChips.Infrastructure - contains all infrastructure related implementations and definitions. Most important part are the Database & Database Migrations.

## Frontend

Is a Blazor WebAssembly application that allows to:
* login (JWT based session)
* CRUD for companies

All data is being taken from the Backend via GraphQL (using [GraphQL.NET](https://github.com/graphql-dotnet/graphql-dotnet))

Great look of the app is done by using [MudBlazor](https://mudblazor.com/) design system.

# Launching the solution

In order to launch solution in debugging mode do the following:
1. Setup MSSQL database instance.
2. Go to `Backend/BananaChips.API/appsettings.Development.json` file and provide required settings values.
3. Debug Backend - at first start, app is going to create database, apply migrations and create default user automatically.
4. Go to `Frontend/BananaChips.Frontend` in shell, and run the command: `dotnet run`
5. After starting backend, it will be possible to interact with it by using the Frontend app.