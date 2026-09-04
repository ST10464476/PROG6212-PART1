CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    PhoneNumber VARCHAR(20),
    Role VARCHAR(20) NOT NULL,
    DateRegistered DATETIME DEFAULT GETDATE()
);
GO
CREATE TABLE Events (
    EventID INT IDENTITY(1,1) PRIMARY KEY,
    OrganiserID INT NOT NULL,
    EventName VARCHAR(100) NOT NULL,
    Description VARCHAR(500),
    EventDate DATE NOT NULL,
    Location VARCHAR(150) NOT NULL,
    Distance DECIMAL(6,2) NOT NULL,
    EntryFee DECIMAL(10,2) NOT NULL,
    Status VARCHAR(30) NOT NULL,

    CONSTRAINT FK_Events_Users
        FOREIGN KEY (OrganiserID) REFERENCES Users(UserID)
);
GO
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    EventID INT NOT NULL,
    CategoryName VARCHAR(100) NOT NULL,
    Distance DECIMAL(6,2) NOT NULL,
    AgeMin INT,
    AgeMax INT,
    EntryFee DECIMAL(10,2) NOT NULL,

    CONSTRAINT FK_Categories_Events
        FOREIGN KEY (EventID) REFERENCES Events(EventID)
);
GO
CREATE TABLE Enrolments (
    EnrolmentID INT IDENTITY(1,1) PRIMARY KEY,
    EventID INT NOT NULL,
    ParticipantID INT NOT NULL,
    EnrolmentDate DATETIME DEFAULT GETDATE(),
    RaceNumber INT,
    PaymentStatus VARCHAR(30) NOT NULL,

    CONSTRAINT FK_Enrolments_Events
        FOREIGN KEY (EventID) REFERENCES Events(EventID),

    CONSTRAINT FK_Enrolments_Users
        FOREIGN KEY (ParticipantID) REFERENCES Users(UserID)
);
GO
CREATE TABLE Results (
    ResultID INT IDENTITY(1,1) PRIMARY KEY,
    EnrolmentID INT NOT NULL,
    FinishTime TIME,
    Position INT,
    AveragePace DECIMAL(6,2),
    ResultStatus VARCHAR(30) NOT NULL,

    CONSTRAINT FK_Results_Enrolments
        FOREIGN KEY (EnrolmentID) REFERENCES Enrolments(EnrolmentID)
);
GO
CREATE TABLE Routes (
    RouteID INT IDENTITY(1,1) PRIMARY KEY,
    EventID INT NOT NULL,
    RouteName VARCHAR(100) NOT NULL,
    Distance DECIMAL(6,2) NOT NULL,
    Description VARCHAR(500),
    MapUrl VARCHAR(255),

    CONSTRAINT FK_Routes_Events
        FOREIGN KEY (EventID) REFERENCES Events(EventID)
);
GO
CREATE TABLE Weather (
    WeatherID INT IDENTITY(1,1) PRIMARY KEY,
    EventID INT NOT NULL,
    WeatherDate DATE NOT NULL,
    Temperature DECIMAL(5,2),
    Condition VARCHAR(100),
    WindSpeed DECIMAL(6,2),
    RainChance DECIMAL(5,2),

    CONSTRAINT FK_Weather_Events
        FOREIGN KEY (EventID) REFERENCES Events(EventID)
);
GO



-- 2 Organisers
INSERT INTO Users (FirstName, LastName, Email, PasswordHash, PhoneNumber, Role)
VALUES
('Thabo', 'Mokoena', 'thabo@raceday.co.za', 'Password123', '0711111111', 'Organiser'),
('Lerato', 'Nkosi', 'lerato@raceday.co.za', 'Password123', '0722222222', 'Organiser');

-- 2 Participants
INSERT INTO Users (FirstName, LastName, Email, PasswordHash, PhoneNumber, Role)
VALUES
('Sipho', 'Dlamini', 'sipho@gmail.com', 'Password123', '0733333333', 'Participant'),
('Naledi', 'Molefe', 'naledi@gmail.com', 'Password123', '0744444444', 'Participant');
GO



-- 3 Events
INSERT INTO Events (OrganiserID, EventName, Description, EventDate, Location, Distance, EntryFee, Status)
VALUES
(1, 'Johannesburg City Run', 'Annual road running event.', '2026-10-10', 'Johannesburg', 10.00, 150.00, 'Open'),
(1, 'Pretoria Charity Walk', 'Community charity walking event.', '2026-11-07', 'Pretoria', 5.00, 100.00, 'Open'),
(2, 'Cape Town Cycle Challenge', 'Road cycling event.', '2026-12-05', 'Cape Town', 50.00, 300.00, 'Open');
GO



-- Categories for all 3 Events
INSERT INTO Categories (EventID, CategoryName, Distance, AgeMin, AgeMax, EntryFee)
VALUES
(1, '10km Open', 10.00, 18, 99, 150.00),
(1, '10km Junior', 10.00, 13, 17, 100.00),
(2, '5km Open', 5.00, 18, 99, 100.00),
(2, '5km Junior', 5.00, 13, 17, 75.00),
(3, '50km Open', 50.00, 18, 99, 300.00),
(3, '50km Junior', 50.00, 16, 17, 250.00);
GO



-- Sample Enrolments
INSERT INTO Enrolments (EventID, ParticipantID, RaceNumber, PaymentStatus)
VALUES
(1, 3, 101, 'Paid'),
(2, 4, 202, 'Paid'),
(3, 3, 303, 'Pending');
GO



-- Check the inserted data
SELECT * FROM Users;
SELECT * FROM Events;
SELECT * FROM Categories;
SELECT * FROM Enrolments;