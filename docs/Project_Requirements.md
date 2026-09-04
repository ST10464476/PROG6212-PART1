# 🎯 RaceDay Project Requirements

## Functional Requirements

The RaceDay system should allow users to perform actions based on their role.

### Organiser

An organiser should be able to:

- Create and manage events
- Create event categories
- View participant enrolments
- Manage event routes
- Record race results
- Manage event information

### Participant

A participant should be able to:

- Register an account
- Log into the system
- View available events
- View event categories
- Enrol for an event
- View enrolment information
- View race results

## Database Requirements

The system requires a relational SQL Server database that stores:

- User information
- Event information
- Event categories
- Participant enrolments
- Race results
- Event routes
- Weather information

Primary and foreign keys will be used to maintain relationships and data integrity.

## Security Requirements

The system should ensure that users are authenticated before accessing protected functionality.

Role-based access should be used to separate organiser and participant functionality.

## Future Development

These requirements will guide the development of the REST API in Part 2 and the MVC web application in Part 3.
