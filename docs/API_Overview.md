# 🌐 RaceDay API Overview

## Overview

The RaceDay API will provide the backend services required by the event management system.

The API will allow the web application to communicate with the database and perform operations such as managing users, events, enrolments and results.

## Main API Areas

### 🔐 Authentication
Handles user registration and login.

### 👤 Users
Allows authenticated users to view and update their profile information.

### 🏃 Events
Allows events to be viewed, created, updated and deleted by authorised organisers.

### 🏷️ Categories
Manages the different categories available for each event.

### 📝 Enrolments
Allows participants to enrol for events and allows organisers to view event enrolments.

### 🏆 Results
Allows race results to be recorded and viewed.

### 🗺️ Routes
Provides information about event routes.

### 🌤️ Weather
Provides weather information associated with events.

## Access Control

The API will use role-based access to ensure that different users can only perform actions appropriate to their role.

The two main roles are:

- **Organiser**
- **Participant**

The complete list of planned endpoints is available in:

`RaceDay_API_Endpoint_Plan.pdf`
