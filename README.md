# Cheapest Route API

A RESTful API built with ASP.NET Core and MediatR that allows users to manage travel routes and calculate the cheapest route between two points, regardless of the number of connections. The service uses graph-based pathfinding to return the most cost-effective travel path.

## Project Description

This project is a technical challenge that involves:

- A CRUD for managing routes (origin, destination, and cost).
- A search algorithm that returns the cheapest route between two cities, considering multiple connections.
- Implementation using the Mediator pattern via MediatR.
- Clean architecture with separation of concerns.
- Swagger UI for API documentation.
- Unit tests for handlers and business logic.

## Tech Stack

- .NET 8
- ASP.NET Core Web API
- MediatR
- Entity Framework Core or File-based Storage
- Swagger / Swashbuckle
- xUnit and Moq

## Endpoints
