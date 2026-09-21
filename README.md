# RaceDay - PROG6212 POE Part 1

## Student Information

**Student Name:** Kieran Pillay  
**Student Number:** ST10445189  
**Module:** PROG6212  
**Project:** RaceDay  
**Assessment:** POE Part 1  

---

## 1. Project Overview

RaceDay is a race and event management system designed to help organisers manage running, walking and cycling events.

The system allows organisers to create and manage events, categories and race routes. Participants can register for event categories and view their enrolments and race results.

Part 1 of the project focuses on the initial system planning and database design. This includes the API endpoint plan, Entity Relationship Diagram (ERD), SQL Server database and sample data.

---

## 2. Part 1 Deliverables

The following documents and files were created for Part 1:

- `API_Endpoint_Plan.md`
- `RaceDay_Database_Script.sql`
- `RaceDay_ERD.drawio.png`

These files contain the main planning and database work completed for the RaceDay system.

---

## 3. API Endpoint Plan

The `API_Endpoint_Plan.md` file contains the planned API endpoints for the RaceDay system.

The endpoint plan covers:

- Authentication
- User profiles
- Events
- Event categories
- Participant enrolments
- Race results
- Race routes
- Weather information

The API plan includes the HTTP method, route, description, required role, request body and expected response for each endpoint.

Examples of planned endpoints include:

```text
POST /api/auth/register
POST /api/auth/login
GET /api/events
GET /api/events/{id}
POST /api/events
PUT /api/events/{id}
DELETE /api/events/{id}
POST /api/enrolments
GET /api/enrolments/me
POST /api/results
GET /api/results/me
GET /api/events/{eventId}/routes
GET /api/events/{eventId}/weather
```

---

## 4. Database

The RaceDay database was created using Microsoft SQL Server.

The database is called:

**RaceDayDB**

The database contains the following tables:

- User
- Event
- Category
- Route
- Enrolment
- Result

### User

Stores both organisers and participants.

Important fields include:

- UserId
- FullName
- Email
- PasswordHash
- Role
- PhoneNumber
- CreatedAt

The Role field identifies whether the user is an Organiser or Participant.

### Event

Stores race and event information.

Important fields include:

- EventId
- OrganiserId
- EventName
- Description
- EventType
- EventDate
- StartTime
- Location
- Province
- CreatedAt

### Category

Stores the different entry categories available for an event.

Examples include:

- 109km Cycle Race
- 35km Fun Ride
- 42.2km Marathon
- 21.1km Half Marathon
- 5km Park Run

### Route

Stores information about the route used for an event.

The table includes:

- RouteId
- EventId
- RouteName
- DistanceKm
- ElevationGainM
- StartPoint
- EndPoint
- GpxFileUrl

### Enrolment

Stores participants entering specific event categories.

The table links participants to categories and stores the enrolment status.

### Result

Stores race results for participants.

The table includes information such as:

- Finish time
- Position
- Result status
- Organiser who captured the result
- Date and time the result was captured

---

## 5. Database Relationships

The database uses primary keys and foreign keys to connect the different tables.

The main relationships are:

- User → Event
- User → Enrolment
- Event → Category
- Event → Route
- Category → Enrolment
- Enrolment → Result
- User → Result

An organiser can create events, while participants can enrol in event categories.

Each event can have multiple categories and routes.

An enrolment belongs to a participant and a category. A result is linked to an enrolment.

---

## 6. Database Constraints

The SQL database uses several constraints to help maintain valid data.

These include:

- Primary keys
- Foreign keys
- Unique constraints
- Check constraints
- Default values

For example, user email addresses must be unique and the user role must be either:

- Organiser
- Participant

The database also prevents a participant from enrolling in the same category more than once.

---

## 7. Sample Data

The database includes sample data for testing.

The seed data contains:

- 2 organisers
- 2 participants
- 3 events
- 5 event categories
- 3 routes
- 4 enrolments
- 1 sample race result

The sample data is included in the SQL script so that the database can be tested after creation.

---

## 8. ERD

The RaceDay Entity Relationship Diagram was created to show the database structure and relationships between the tables.

The ERD is saved as:

`RaceDay_ERD.drawio.png`

The diagram shows the primary keys, foreign keys, important attributes and relationships between the RaceDay entities.

---

## 9. Technologies Used

The following technologies and tools were used for Part 1:

- Microsoft SQL Server
- SQL Server Management Studio (SSMS)
- Visual Studio
- Draw.io
- Git
- GitHub
- Markdown

---

## 10. Running the Database Script

To create the RaceDay database:

1. Open SQL Server Management Studio.
2. Connect to the SQL Server instance.
3. Open `RaceDay_Database_Script.sql`.
4. Execute the complete script from the beginning.
5. The script creates the `RaceDayDB` database.
6. The required tables are created.
7. Sample data is inserted.
8. Verification queries at the end of the script display the inserted data.

The database can then be viewed under:

```text
Databases
└── RaceDayDB
    └── Tables
```

---

## 11. Part 1 Status

The following Part 1 components have been completed:

- [x] API Endpoint Plan
- [x] Database design
- [x] SQL Server database
- [x] Database tables
- [x] Primary and foreign keys
- [x] Database constraints
- [x] Sample/seed data
- [x] Entity Relationship Diagram
- [x] Project documentation
- [x] Git repository files committed

---

## 12. Future Development

The API and application functionality will be developed in later parts of the RaceDay project.

Future development will include:

- ASP.NET Core Web API
- Authentication and JWT tokens
- Role-based authorisation
- MVC application
- Azure services
- Azure Blob Storage
- Route and GPX file management
- Weather information
- Participant and organiser functionality

---

## 13. Demonstration Video

A short video walkthrough demonstrating the RaceDay Part 1 deliverables (database design, ERD and API endpoint plan) is available at the link below:

**YouTube Link:** 

The video covers:

- An overview of the RaceDay project
- A walkthrough of the API Endpoint Plan
- The database design and ERD
- A demonstration of the SQL Server database script and sample data

---

## 14. Conclusion

Part 1 establishes the initial design and database foundation for the RaceDay system. The API Endpoint Plan defines the planned system functionality, while the ERD and SQL Server database provide the structure needed for the application to be developed in later parts.

The database has been populated with sample data and tested using SQL Server Management Studio.
