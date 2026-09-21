/* 
   RaceDay - Database Creation and Seed Script

   Module: PROG6212 - Programming 
   Assessment: POE - Part 1
   Section: C - Database Implementation

   Student Name: Kieran Pillay
   Student Number: ST10445189
   Group: 2
   Campus: Emeris Durban North

   Database: RaceDayDB
   DBMS: Microsoft SQL Server
   Tool: SQL Server Management Studio (SSMS)

   This script creates the RaceDay database, creates the
   required tables and relationships, and adds sample data
   for testing.
   */

IF DB_ID('RaceDayDB') IS NULL
BEGIN
    CREATE DATABASE RaceDayDB;
END
GO

USE RaceDayDB;
GO

IF OBJECT_ID('dbo.Result', 'U') IS NOT NULL
    DROP TABLE dbo.Result;

IF OBJECT_ID('dbo.Enrolment', 'U') IS NOT NULL
    DROP TABLE dbo.Enrolment;

IF OBJECT_ID('dbo.Route', 'U') IS NOT NULL
    DROP TABLE dbo.Route;

IF OBJECT_ID('dbo.Category', 'U') IS NOT NULL
    DROP TABLE dbo.Category;

IF OBJECT_ID('dbo.Event', 'U') IS NOT NULL
    DROP TABLE dbo.Event;

IF OBJECT_ID('dbo.[User]', 'U') IS NOT NULL
    DROP TABLE dbo.[User];
GO

CREATE TABLE dbo.[User] (
    UserId INT IDENTITY(1,1) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) NOT NULL,
    PhoneNumber NVARCHAR(20) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),

    CONSTRAINT PK_User PRIMARY KEY (UserId),
    CONSTRAINT UQ_User_Email UNIQUE (Email),
    CONSTRAINT CK_User_Role
        CHECK (Role IN ('Organiser', 'Participant'))
);
GO

CREATE TABLE dbo.Event (
    EventId INT IDENTITY(1,1) NOT NULL,
    OrganiserId INT NOT NULL,
    EventName NVARCHAR(150) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    EventType NVARCHAR(20) NOT NULL,
    EventDate DATE NOT NULL,
    StartTime TIME NULL,
    Location NVARCHAR(150) NOT NULL,
    Province NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),

    CONSTRAINT PK_Event PRIMARY KEY (EventId),
    CONSTRAINT FK_Event_Organiser
        FOREIGN KEY (OrganiserId)
        REFERENCES dbo.[User](UserId),
    CONSTRAINT CK_Event_Type
        CHECK (EventType IN ('Running', 'Walking', 'Cycling'))
);
GO

CREATE TABLE dbo.Category (
    CategoryId INT IDENTITY(1,1) NOT NULL,
    EventId INT NOT NULL,
    CategoryName NVARCHAR(50) NOT NULL,
    DistanceKm DECIMAL(5,2) NOT NULL,
    MaxParticipants INT NOT NULL DEFAULT 500,
    EntryFee DECIMAL(8,2) NOT NULL DEFAULT 0,

    CONSTRAINT PK_Category PRIMARY KEY (CategoryId),
    CONSTRAINT FK_Category_Event
        FOREIGN KEY (EventId)
        REFERENCES dbo.Event(EventId)
        ON DELETE CASCADE
);
GO

CREATE TABLE dbo.Route (
    RouteId INT IDENTITY(1,1) NOT NULL,
    EventId INT NOT NULL,
    RouteName NVARCHAR(100) NOT NULL,
    DistanceKm DECIMAL(5,2) NOT NULL,
    ElevationGainM INT NULL,
    StartPoint NVARCHAR(150) NOT NULL,
    EndPoint NVARCHAR(150) NOT NULL,
    GpxFileUrl NVARCHAR(255) NULL,

    CONSTRAINT PK_Route PRIMARY KEY (RouteId),
    CONSTRAINT FK_Route_Event
        FOREIGN KEY (EventId)
        REFERENCES dbo.Event(EventId)
        ON DELETE CASCADE
);
GO

CREATE TABLE dbo.Enrolment (
    EnrolmentId INT IDENTITY(1,1) NOT NULL,
    ParticipantId INT NOT NULL,
    CategoryId INT NOT NULL,
    EnrolmentDate DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending',

    CONSTRAINT PK_Enrolment PRIMARY KEY (EnrolmentId),
    CONSTRAINT FK_Enrolment_Participant
        FOREIGN KEY (ParticipantId)
        REFERENCES dbo.[User](UserId),
    CONSTRAINT FK_Enrolment_Category
        FOREIGN KEY (CategoryId)
        REFERENCES dbo.Category(CategoryId),
    CONSTRAINT UQ_Enrolment_ParticipantCategory
        UNIQUE (ParticipantId, CategoryId),
    CONSTRAINT CK_Enrolment_Status
        CHECK (Status IN ('Pending', 'Confirmed', 'Cancelled'))
);
GO

CREATE TABLE dbo.Result (
    ResultId INT IDENTITY(1,1) NOT NULL,
    EnrolmentId INT NOT NULL,
    CapturedByOrganiserId INT NOT NULL,
    FinishTime TIME NULL,
    Position INT NULL,
    ResultStatus NVARCHAR(20) NOT NULL DEFAULT 'Pending',
    CapturedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),

    CONSTRAINT PK_Result PRIMARY KEY (ResultId),
    CONSTRAINT FK_Result_Enrolment
        FOREIGN KEY (EnrolmentId)
        REFERENCES dbo.Enrolment(EnrolmentId),
    CONSTRAINT UQ_Result_Enrolment
        UNIQUE (EnrolmentId),
    CONSTRAINT FK_Result_Organiser
        FOREIGN KEY (CapturedByOrganiserId)
        REFERENCES dbo.[User](UserId),
    CONSTRAINT CK_Result_Status
        CHECK (ResultStatus IN ('Pending', 'Finished', 'DNF', 'DQ'))
);
GO

INSERT INTO dbo.[User]
    (FullName, Email, PasswordHash, Role, PhoneNumber)
VALUES
    ('Thandiwe Nkosi', 'thandiwe.nkosi@raceday.co.za',
     'HASHED_PW_1', 'Organiser', '0821234567'),

    ('Johan van der Merwe', 'johan.vdm@raceday.co.za',
     'HASHED_PW_2', 'Organiser', '0837654321'),

    ('Lindiwe Dlamini', 'lindiwe.dlamini@gmail.com',
     'HASHED_PW_3', 'Participant', '0741122334'),

    ('Sipho Khumalo', 'sipho.khumalo@gmail.com',
     'HASHED_PW_4', 'Participant', '0765544332');
GO

INSERT INTO dbo.Event
    (OrganiserId, EventName, Description, EventType,
     EventDate, StartTime, Location, Province)
VALUES
    (1, 'Cape Town Cycle Tour',
     'Iconic road cycling tour around the Cape Peninsula.',
     'Cycling', '2027-03-08', '06:00:00',
     'Cape Town', 'Western Cape'),

    (1, 'Soweto Marathon',
     'Annual marathon through the streets of Soweto.',
     'Running', '2027-11-07', '06:00:00',
     'Soweto', 'Gauteng'),

    (2, 'Durban Beachfront Park Run',
     'Community 5km park run along the beachfront.',
     'Walking', '2027-05-15', '07:00:00',
     'Durban', 'KwaZulu-Natal');
GO

INSERT INTO dbo.Category
    (EventId, CategoryName, DistanceKm, MaxParticipants, EntryFee)
VALUES
    (1, '109km Cycle Race', 109.00, 15000, 550.00),
    (1, '35km Fun Ride', 35.00, 5000, 250.00),
    (2, '42.2km Marathon', 42.20, 8000, 400.00),
    (2, '21.1km Half Marathon', 21.10, 6000, 300.00),
    (3, '5km Park Run', 5.00, 1000, 0.00);
GO

INSERT INTO dbo.Route
    (EventId, RouteName, DistanceKm, ElevationGainM,
     StartPoint, EndPoint, GpxFileUrl)
VALUES
    (1, 'Peninsula Loop', 109.00, 1200,
     'Green Point', 'Green Point', NULL),

    (2, 'Soweto Marathon Route', 42.20, 350,
     'FNB Stadium', 'FNB Stadium', NULL),

    (3, 'Beachfront Promenade', 5.00, 20,
     'uShaka Marine', 'Blue Lagoon', NULL);
GO

INSERT INTO dbo.Enrolment
    (ParticipantId, CategoryId, Status)
VALUES
    (3, 1, 'Confirmed'),
    (3, 3, 'Confirmed'),
    (4, 4, 'Pending'),
    (4, 5, 'Confirmed');
GO

INSERT INTO dbo.Result
    (EnrolmentId, CapturedByOrganiserId,
     FinishTime, Position, ResultStatus)
VALUES
    (1, 1, '04:15:32', 342, 'Finished');
GO

SELECT * FROM dbo.[User];
SELECT * FROM dbo.Event;
SELECT * FROM dbo.Category;
SELECT * FROM dbo.Route;
SELECT * FROM dbo.Enrolment;
SELECT * FROM dbo.Result;
