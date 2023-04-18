CREATE DATABASE evv;

USE evv;

CREATE TABLE Categories (
    Vehicle_ID int IDENTITY(1,1) PRIMARY KEY,
    Vehicle_Name nvarchar(255) NOT NULL,
    Emission int NOT NULL
);

CREATE TABLE Journeys (
    Journey_ID int IDENTITY(1,1) PRIMARY KEY,
    User_ID int NOT NULL,
    Total_Distance int NULL,
    Total_Emission int NULL,
    [Start] varchar(255) NULL,
    [End] varchar(255) NULL,
    Date date NOT NULL,
	Score int,
);

CREATE TABLE Stopovers (
    Stopover_ID int IDENTITY(1,1) NOT NULL,
    Vehicle_ID int NOT NULL,
    Journey_ID int NOT NULL,
    Distance int NOT NULL,
    [Start] varchar(255) NOT NULL,
    [End] varchar(255) NOT NULL,
    Emission int NOT NULL,
    PRIMARY KEY (Stopover_ID),
    FOREIGN KEY (Journey_ID) REFERENCES Journeys(Journey_ID) ON DELETE CASCADE,
    FOREIGN KEY (Vehicle_ID) REFERENCES Categories(Vehicle_ID) ON DELETE CASCADE
);

CREATE TABLE Users(
User_ID int IDENTITY(1,1) NOT NULL,
UUID nvarchar(50) NOT NULL,
FullName nvarchar(50) NOT NULL,
Score int NOT NULL,
CONSTRAINT PK_Users PRIMARY KEY CLUSTERED (User_ID)
);
