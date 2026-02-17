Meeting Room Booking System 

A RESTful meeting room reservation backend built with ASP.NET Core (.NET 8).

This project models a real office booking system and enforces scheduling rules such as time validation and conflict prevention.
It is designed as a portfolio project to demonstrate backend engineering, API design, and clean architecture.

✨ Features

Create and manage meeting rooms

Make bookings for specific time ranges

Prevent double bookings

Query room schedules

Proper HTTP status handling (400 / 404 / 409)

🧠 Business Rules

The system enforces real calendar behaviour:

A booking cannot overlap another booking

Start time must be before end time

Inactive rooms cannot be reserved

Invalid requests → 400 Bad Request

Room not found → 404 Not Found

Booking conflict → 409 Conflict

🏗 Architecture

Layered architecture separating business logic from infrastructure:

Controllers → Application Services → Domain Entities → Repositories → Database

Business rules live in the Domain layer, not controllers.

🛠 Tech Stack

.NET 8 / ASP.NET Core Web API

Entity Framework Core

SQLite

Dependency Injection

Swagger / OpenAPI

▶️ How to Run
Prerequisite

Install .NET 8 SDK

Check installation:

dotnet --version

Start the API

cd src/MeetingRoomBooking.Api
dotnet run

On first run the database is created automatically.

Swagger

https://localhost:xxxx/swagger

🎯 What This Project Demonstrates

REST API design

Clean architecture

Business rule enforcement

Error handling with proper HTTP semantics