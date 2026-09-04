# 🏃‍♂️ RaceDay Event Management System

### A Web-Based Event Management System for Running, Walking and Cycling Events

**Student Number:** ST10464476  
**Module:** PROG6212  

---

## 🎯 Project Overview

**RaceDay** is a web-based event management system designed to support the planning and management of running, walking and cycling events in South Africa.

The system is designed to make it easier for **Organisers** to manage events, categories, participant enrolments, routes, weather information and race results. **Participants** will be able to view available events, enrol for events and access their enrolment and result information.

The project is being developed in stages, starting with system planning and database design before moving into API and web application development.

---

## 👥 User Roles

### Organiser

Organisers are responsible for managing events and their related information. They can:

- Create and update events
- Manage event categories
- View participant enrolments
- Manage event routes
- Record race results
- Manage event weather information

### Participant

Participants interact with the system to take part in available events. They can:

- Create an account and log in
- View available events
- View event categories
- Enrol for events
- View their enrolments
- View their results

---

## 📋 Part 1 – System Planning & Database

Part 1 establishes the foundation of the RaceDay system before API development begins.

The following components have been planned and developed:

- Entity Relationship Diagram (ERD)
- REST API Endpoint Plan
- SQL Server database design
- Primary and foreign key relationships
- Database constraints
- Sample database records
- Project documentation

---

## 🗄️ Database

The RaceDay database contains seven main entities:

**Users, Events, Categories, Enrolments, Results, Routes and Weather**

The database design defines the relationships between these entities and includes the required primary keys, foreign keys, constraints and sample data.

**Database Script:**  
`docs/RaceDay_Database.sql`

---

## 🌐 API Planning

The API Endpoint Plan defines the REST endpoints that will be required for the RaceDay system.

The planned endpoints cover:

- Authentication
- User profiles
- Events
- Categories
- Enrolments
- Results
- Routes
- Weather information

**API Endpoint Plan:**  
`docs/RaceDay_API_Endpoint_Plan.pdf`

---

## 📊 System Design

The Entity Relationship Diagram provides a visual representation of the RaceDay database and shows how the main entities are connected.

**ERD:**  
`docs/RaceDay_ERD.png`

---

## 🛠️ Technologies & Tools

- C# / .NET
- SQL Server
- SQL Server Management Studio (SSMS)
- REST API
- Git & GitHub
- GitHub Actions
- MVC
- Azure
- Docker

---

## 📁 Repository Structure

```text
PROG6212
│
├── docs
│   ├── RaceDay_ERD.png
│   ├── RaceDay_API_Endpoint_Plan.pdf
│   ├── RaceDay_Database.sql
│   └── ERD_Description.md
│
├── .github
│   └── workflows
│
└── README.md
