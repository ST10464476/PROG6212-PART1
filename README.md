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
