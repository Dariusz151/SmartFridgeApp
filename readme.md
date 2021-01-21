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

# High-level view



# Architecture

In this application I used several programming concepts such as:
- Onion Architecture (split into domain layer, infrastructure layer and application (API) layer)
- Command Query Responsibility Separation
- Domain-Driven Design

### Onion architecture

![Alt text](misc/img/onion-architecture.png?raw=true "Onion architecture")

### CQRS

I used CQRS pattern to communicate with database. 
- Read data is done with Dapper (raw SQL queries - SELECT data from views)
- Write data is done with Entity Framework (code first approach)

It's very simple solution without additional Event Source database or any projection - just made views in database for read-model.

![Alt text](misc/img/cqrs.PNG?raw=true "CQRS pattern")

### Domain-driven design

I tried to apply some Domain-Driven Design concepts to my app. 
I'm aware that in this case its over-engineering but I created this app to learn as much as possible (mainly on own mistakes).
So while looking into the code you can see such a objects like aggregates, entities, value objects, domain events (building blocks) etc. 


# Domain

There are 3 main group of contexts in this app: 
1. Fridge 
2. Recipe
3. Food product

**Fridges** contain users (**FridgeUser**) who have a list of **FridgeItems**. 

Each FridgeItem consists of **FoodProduct** (connected by id), **AmountValue** and other additional properties like note or expiration date.

**FoodProducts** are independent - they have only identificators, name and category.

**Recipes** have a list of **FoodProduct** and description how to prepare this recipe from this items.

In this case Fridge, FoodProduct and Recipe are *aggregates*. 

FridgeUsers and FridgeItems are *entities*.

AmountValue is *value object*.

### Quartz and Outbox pattern

I used Quartz to handle outbox messages. 
I implemented outbox pattern by simple table in database - I save some of the events and then can handle them in hosted service.
Quartz is triggering the job which checks if there is new records in db.


### UML diagram.

# Database structure

# Stack

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
- Quartz
- MediatR (for mediator in CQRS)





# Resources

- [Kamil Grzybek's Modular monolith repository](https://github.com/kgrzybek/modular-monolith-with-ddd "Github link")   
- and other's articles from Kamil Grzybek's
- Martin Fowler's articles [CQRS](https://martinfowler.com/bliki/CQRS.html "cqrs")
- Quartz config https://andrewlock.net/using-scoped-services-inside-a-quartz-net-hosted-service-with-asp-net-core/




