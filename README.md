# RaceDay Event Management System

## Student Details

Name and Surname: Katlego Moshaba  
Student Number: ST10464476  
Module: PROG6212  

## Project Description

RaceDay is a web-based event management system designed for running, walking and cycling events in South Africa.

The system will allow organisers to create and manage events, event categories, participant enrolments, routes, weather information and race results. Participants will be able to register, log in, view available events, enrol for events and view their enrolment and result information.

The project is being developed in different stages. Part 1 focuses on the planning of the system and the design of the database before the REST API and web application are developed.

## User Roles

### Organiser

An organiser will be able to:

- Create and manage events
- Manage event categories
- View participant enrolments
- Manage event routes
- Record race results
- Manage event weather information

### Participant

A participant will be able to:

- Register and log in
- View available events
- View event categories
- Enrol for events
- View their enrolments
- View their results

## Part 1: System Planning and Database

Part 1 includes the planning and database design for the RaceDay system.

The main deliverables include:

- Entity Relationship Diagram (ERD)
- API Endpoint Plan
- SQL Server database
- Database tables and relationships
- Primary and foreign keys
- Database constraints
- Sample database data
- Project documentation

## Database

The RaceDay database contains the following entities:

- Users
- Events
- Categories
- Enrolments
- Results
- Routes
- Weather

The database was created using SQL Server Management Studio (SSMS).

The SQL database script is available in:

`docs/RaceDay_Database.sql`

## API Planning

The API Endpoint Plan outlines the REST API routes that will be required when the API is developed.

The planned endpoints cover:

- Authentication
- User profiles
- Events
- Categories
- Enrolments
- Results
- Routes
- Weather

The API Endpoint Plan is available in:

`docs/RaceDay_API_Endpoint_Plan.pdf`

## ERD

The ERD shows the main entities in the RaceDay database and the relationships between them.

The ERD is available in:

`docs/RaceDay_ERD.png`

## Technologies and Tools

- C#
- .NET
- SQL Server
- SQL Server Management Studio
- REST API
- GitHub
- GitHub Actions
- MVC
- Azure
- Docker

## Repository Structure

```text
PROG6212
│
├── docs
│   ├── RaceDay_ERD.png
│   ├── RaceDay_API_Endpoint_Plan.pdf
│   ├── RaceDay_Database.sql
│   └── Project Documentation
│
├── .github
│   └── workflows
│
├── .gitignore
└── README.md



---

# Part 2: RaceDay REST API

## Project Description

RaceDay is a system designed to manage events, organisers, participants, event categories, enrolments, and results. This part of the project focuses on developing a REST API to support the management of the RaceDay system.

## Technologies Used

- C#
- ASP.NET Core Web API
- SQL Server
- Entity Framework Core
- Swagger / OpenAPI
- GitHub

## Project Features

The RaceDay API is designed to support the following functionality:

- User registration and login
- User profile management
- Event management
- Event category management
- Event enrolments
- Event results

Only features implemented in the project should be considered complete.

## Project Structure

The project contains the following main files and folders:

- **Controllers:** Handles API requests and responses.
- **Data:** Contains database-related code.
- **Models:** Contains the application's data models.
- **Properties:** Contains project launch settings.
- **Program.cs:** Configures and starts the application.
- **appsettings.json:** Contains application configuration.
- **REST API.csproj:** Contains project settings and dependencies.
- **REST API.sln:** Visual Studio solution file.
- **API.http:** Contains HTTP requests for testing the API.
- **Database Script:** Contains the SQL database setup.
- **API Endpoint Plan:** Documents the planned API endpoints.

## Database

The RaceDay system uses SQL Server to store and manage application data. The database script provides the table definitions and related database setup required for the project.

The database must be configured correctly before running API operations that depend on it.

## Running the API

1. Open the solution file in Visual Studio.
2. Restore the required NuGet packages if prompted.
3. Check the database connection settings in `appsettings.json`.
4. Ensure that the SQL Server database is configured.
5. Run the application.
6. Open the Swagger page to view and test the available API endpoints.

## API Testing

Swagger / OpenAPI can be used to inspect and test the available endpoints. The `API.http` file can also be used to send HTTP requests where applicable.

Successful API responses depend on the endpoint, request data, database configuration, and application state.

## Documentation

The repository includes the API endpoint plan, database script, and supporting project documentation.

## Version Control

GitHub is used to store the project files and track changes through commits.

## Author

RaceDay System Project — PROG6212
