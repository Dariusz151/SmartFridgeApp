# Purpose of the application

Application for managing kitchen products and recipes. 
Users can create fridges, join them and add list of food products. 
App has also list of recipes and based on selected food products user can find recipe to make.

I am automatically the first user of this app, because it was created to help me manage the preparing of everyday dinners. :)
I have a lot of ideas to new features and application growth - only limitation is my free time. 
Please do not hesitate to provide me any feedback/comments about the app.


#### Link to app: [SmartFridgeApp](https://smartfridgeapp.pl/ "Link to app")

# Front-end app

To be able to start using the application in everyday life, I had to create a prototype of the front-end interface (UI). 
I used React as I have some basics knowledge about this framework. 
I'm not front-end developer so I'm aware of its imperfections. I would be grateful if someone wanted to support the front end part of the application.

## Architecture

In this application I used several programming concepts such as:
- Domain-Driven Design
- Command Query Responsibility Separation
- Onion Architecture (split into domain layer, infrastructure layer and application (API) layer)

### Onion architecture

![Alt text](misc/img/onion-architecture.png?raw=true "Onion architecture")

### CQRS

I used CQRS pattern to communicate with database. 
- Read data is done with Dapper (raw SQL queries - SELECT data from views)
- Write data is done with Entity Framework (code first approach)

It's very simple solution without additional Event Source database or any projection - just made views in database for read-model.


![Alt text](misc/img/cqrs.png?raw=true "CQRS pattern")

### High-level view

### Domain



### Modules

### Stack

I used this tools while developing app:
- .NET Core 3.1
- SSMS (SQL Server)
- Entity Framework
- React,
- Swashbuckle (Swagger)
- Dapper (light ORM to read from database )
- Visual Studio 2019
- Visual Studio Code
- Postman
- NUnit (Unit testing)
- Autofac (Dependency Injection)
- MediatR (for mediator in CQRS)

## Infrastructure

## API

## How to run application

# Resources

- [Kamil Grzybek's Modular monolith repository](https://github.com/kgrzybek/modular-monolith-with-ddd "Github link")   
- and other's articles from Kamil Grzybek's
- Martin Fowler's articles [CQRS](https://martinfowler.com/bliki/CQRS.html "cqrs")


## CI/CD
 
## Todo list



