# RaceDay - Event Management System

## 1. Project Overview

RaceDay is a web-based event management system developed to support road running, walking and cycling events in South Africa.

The system allows event organisers to create and manage sporting events, categories, participant enrolments and race results.

Participants can browse upcoming events, enrol in events, update their profiles, upload profile pictures and view their race results.

## 2. Technologies Used

- ASP.NET Core Web API
- ASP.NET Core MVC
- C#
- .NET 8
- Entity Framework Core
- SQL Server
- HTML
- CSS
- Bootstrap
- JavaScript
- Azure Blob Storage
- Docker
- GitHub

## 3. System Features

### Organiser
- Register and log in.
- Create, edit and delete events.
- Upload event banner images.
- Create and manage event categories.
- View participant enrolments.
- Enter race results.

### Participant
- Register and log in.
- Browse sporting events.
- View event details and categories.
- Enrol in events.
- View personal enrolments.
- Update profile information.
- Upload a profile picture.
- View race results.

## 4. Project Structure

RaceDay contains two main projects:

- RaceDay.API – Handles API requests, authentication, database operations and Azure image storage.
- RaceDay.Web – Provides the MVC website used by organisers and participants.

## 5. Database

The application uses Microsoft SQL Server with Entity Framework Core.

The database stores users, organisers, participants, events, categories, enrolments and race results.

## 6. Azure Blob Storage

Azure Blob Storage is used to store participant profile pictures and event banner images.

Image uploads are handled through the API.

## 7. Running the Application

Open the RaceDay solution in Visual Studio 2022.

Set RaceDay.API and RaceDay.Web as startup projects.

Press F5 to run both projects.

The API Swagger interface is available at:

https://localhost:7237/

The MVC website is available at:

https://localhost:7172/

## 8. Docker

Dockerfiles have been added to both projects, together with a Docker Compose configuration.

Docker Desktop and a working Docker engine are required to build and run the containers.

The Docker configuration still requires environment-specific database and Azure Storage settings before deployment.

## 9. Conclusion

RaceDay provides a central platform for managing sporting events and participant activities. It demonstrates the use of ASP.NET Core, REST APIs, SQL Server, Entity Framework Core, Azure Blob Storage and MVC development.