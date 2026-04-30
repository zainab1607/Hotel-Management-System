
CREATE DATABASE HotelManagementDB;

USE HotelManagementDB;


CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL,
    Password NVARCHAR(50) NOT NULL
);
INSERT INTO Users (Username, Password)
VALUES ('admin', 'admin123');

select * from Bookings
CREATE TABLE Rooms (
    RoomNumber INT PRIMARY KEY,
    RoomType NVARCHAR(50),
    Price FLOAT,
    Status NVARCHAR(20)
);

 CREATE TABLE Guests (
    GuestID INT PRIMARY KEY IDENTITY(1,1),
    GuestName NVARCHAR(100) NOT NULL
);
 CREATE TABLE Bookings (
    BookingID INT IDENTITY(1,1) PRIMARY KEY,
    GuestID INT NOT NULL,
    GuestName NVARCHAR(100) NOT NULL,
    RoomNumber INT NOT NULL,
    CheckIn DATE NOT NULL,
    CheckOut DATE NOT NULL,
    PaymentStatus NVARCHAR(10) NOT NULL CHECK (PaymentStatus IN ('Paid', 'Unpaid')),
    
    FOREIGN KEY (GuestID) REFERENCES Guests(GuestID),
    FOREIGN KEY (RoomNumber) REFERENCES Rooms(RoomNumber)
);



ALTER TABLE Guests
ADD 
    Phone NVARCHAR(20),
    CNIC NVARCHAR(20),
    Email NVARCHAR(100);

	ALTER TABLE Bookings
DROP COLUMN GuestName;

ALTER TABLE Rooms
ALTER COLUMN RoomType NVARCHAR(50) NOT NULL;

ALTER TABLE Rooms
ADD CONSTRAINT DF_Rooms_Status DEFAULT 'Available' FOR Status;

