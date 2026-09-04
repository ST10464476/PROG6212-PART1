# 🔗 RaceDay Database Relationships

## Overview

The RaceDay database uses relationships between the main entities to keep related information connected and organised.

## Main Relationships

### Users → Events

One organiser can manage multiple events.

### Users → Enrolments

One participant can have multiple event enrolments.

### Events → Categories

One event can have multiple categories.

### Events → Enrolments

One event can have multiple participant enrolments.

### Enrolments → Results

An enrolment can have a related race result.

### Events → Routes

An event can have route information associated with it.

### Events → Weather

An event can have weather information associated with it.

## Data Integrity

Primary keys uniquely identify records, while foreign keys connect related records between tables.

These relationships help maintain the structure and integrity of the RaceDay database.
