# Meeting Room Booking System

A .NET Web API project implementing meeting room scheduling with conflict detection and clean layered architecture.

---

## Overview

The **Meeting Room Booking System** is a web application that allows users to reserve meeting rooms, manage availability, and prevent scheduling conflicts. It models a realistic office booking workflow: rooms have capacities and time slots, and bookings must pass validation rules before being confirmed.

Core focus of the project:

* Scheduling logic & conflict detection
* Layered backend architecture
* REST API design
* Database persistence
* Prepared for a React admin/user interface

---

## Key Features

* View available meeting rooms
* Create and cancel bookings
* Prevent double‑booking (time overlap validation)
* Room capacity management
* Booking history records
* Server‑side validation rules

---

## Tech Stack

**Backend**

* C# / .NET (ASP.NET Core Web API)
* Entity Framework Core
* SQLite (development database)

**Frontend (planned / optional)**

* React + Vite

**Other**

* RESTful API
* JSON DTOs

---

## Architecture

The project follows a layered architecture:

```
API Controller → Application/Service → Domain → Infrastructure (EF Core) → Database
```

Responsibilities:

* **Controllers**: HTTP handling only
* **Services**: booking rules & business logic
* **Domain**: entities and constraints
* **Infrastructure**: database persistence

Key design decision: booking validation occurs in the **service layer** to guarantee no overlapping reservations regardless of client behavior.

---

## Core Domain

Entities:

* **Room** – capacity, name, availability
* **Booking** – time range reservation
* **User/Organizer** – booking owner

Business Rules:

* A room cannot be booked if the time slot overlaps with an existing booking
* End time must be after start time
* Capacity must support meeting size

---

## Run Locally

### Prerequisites

* .NET SDK 7+ (or compatible)

### Steps

```bash
git clone https://github.com/YOUR_USERNAME/MeetingRoomBooking.git
cd MeetingRoomBooking
```

Restore packages:

```bash
dotnet restore
```

Run the API:

```bash
dotnet run
```

Server:

```
http://localhost:5000
```

(or the port shown in the console)

---

## Example API Endpoints

**Get rooms**

```
GET /api/rooms
```

**Create booking**

```
POST /api/bookings
```

**Cancel booking**

```
DELETE /api/bookings/{id}
```

---

## What This Project Demonstrates

* Designing REST APIs using ASP.NET Core
* Separation of concerns in backend systems
* Implementing scheduling conflict detection
* Database integration with EF Core
* Preparing backend for frontend integration

---

## API Preview

The API is documented using Swagger UI.  
After running the application, the interactive documentation is available at:

http://localhost:5076/swagger


Below is a preview of the running system:

<img src="images/swagger.png" width="850">
