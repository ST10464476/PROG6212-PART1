# 🔐 RaceDay Database Constraints

## Overview

Database constraints are used in the RaceDay system to help maintain accurate, consistent and reliable data.

## Primary Keys

Each main table has a primary key that uniquely identifies each record.

Examples include:

- UserID
- EventID
- CategoryID
- EnrolmentID
- ResultID
- RouteID
- WeatherID

## Foreign Keys

Foreign keys are used to connect related tables.

For example:

- Events are linked to Users through OrganiserID.
- Categories are linked to Events through EventID.
- Enrolments are linked to Events and Users.
- Results are linked to Enrolments.
- Routes are linked to Events.
- Weather records are linked to Events.

## Required Fields

Important fields use `NOT NULL` constraints to prevent required information from being left empty.

## Unique Values

The user email address uses a `UNIQUE` constraint to help prevent duplicate accounts.

## Default Values

Fields such as registration and enrolment dates use default values so that dates can be recorded automatically.

These constraints help protect the integrity of the RaceDay database.
