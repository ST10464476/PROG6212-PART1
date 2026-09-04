# 🗄️ RaceDay Database Description

## Overview

The RaceDay database is designed to store and manage the information required by the event management system.

The database uses a relational structure where the main entities are connected through primary and foreign keys.

## Main Entities

### Users
Stores information about organisers and participants.

### Events
Stores information about running, walking and cycling events, including the organiser responsible for each event.

### Categories
Stores the different participation categories available for each event.

### Enrolments
Stores participant enrolments for events and their payment status.

### Results
Stores race results linked to participant enrolments.

### Routes
Stores route information associated with events.

### Weather
Stores weather information associated with events.

## Relationships

The database relationships allow information to be connected logically between users, events, categories, enrolments, results, routes and weather records.

Primary and foreign keys are used to maintain referential integrity between related tables.

## Database Implementation

The database was created using **SQL Server Management Studio (SSMS)**.

The complete database creation and sample data script is available in:

`RaceDay_Database.sql`
