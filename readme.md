# Dynamic generic CRUD API via ASP.NET (alpha version)

> OMG - One More Generic

## Introduction

This repo is an example of generic CRUD API.

You create types derived from `IEntity` in `Domain`,

you add it to `DataContext` in `Infrastructure`,

you got dynamically  generated API for them.

## Instruction

To run project you need working Postgres database.

Go to `WebApi` folder, open `appsettings.json` and configure your connection string. Then run 

    dotnet ef migrations add <migration-name> -p ../Infrastructure/Infrastructure.csproj

and after that

    dotnet ef database update

and finally

    dotnet run

here you go.

## Constraints

As an alpha version, currently the project has many constraints, such as:

- many-to-many links are not supported
- owned types are not supported
- only one level nesting 
- only value types and strings (references working for navigation) are supported

## Validation

Use `System.ComponentModel.DataAnnotations` attributes on domain entities to make validation.

## ToDos

- generic mediator pipeline
- expand functionality of mapper
- get rid of constraints
- beautify swagger

## Contributor

[Matveyushka](https://github.com/Matveyushka)