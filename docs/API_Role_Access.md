# 🔐 RaceDay API Role Access

## Overview

The RaceDay API will use role-based access to control which actions can be performed by different users.

## Organiser Access

Organisers will have access to functions related to event management.

They can:

- Create events
- Update events
- Delete events
- Create event categories
- View event enrolments
- Add event routes
- Record race results

## Participant Access

Participants will have access to functions related to taking part in events.

They can:

- View available events
- View event categories
- Enrol for events
- View their enrolments
- View their results

## Public Access

Some information can be viewed without logging in, such as:

- Available events
- Event details
- Event categories
- Event results
- Route information
- Weather information

## Security

Protected API operations will require authentication and appropriate role permissions.

This structure will be implemented during the REST API development stage.
