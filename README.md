# StaySavvy: Hotel Room Management System

StaySavvy is a comprehensive software application designed to optimize operations within small to medium-sized hotels. It leverages fundamental data structures to deliver a robust, efficient, and user-friendly experience.



## Introduction

StaySavvy is developed as a Windows Forms application using C#. It provides an intuitive graphical user interface (GUI) that enables hotel staff to efficiently manage:

* Room reservations
* Guest information
* Room availability

The main objective is to streamline hotel operations, improving both efficiency and user experience.



## Technical Implementation

This project integrates key data structures to handle system logic effectively:

* Linked Lists
  Used for dynamic room management (add, update, delete operations).

* Binary Search Trees (BST)
  Enable fast searching of rooms by Room Number or Guest ID.

* Stacks
  Provide Undo/Redo functionality for booking operations.


## Key Features

* Room Management
  Full CRUD operations for managing hotel rooms.

* Booking System
  Seamless reservations and cancellations with real-time updates.

* Customer Management
  Store and search guest data using Name or NIC.

* Reporting System
  Generate reports for:

  * Revenue
  * Occupancy
  * Customer statistics

* Undo/Redo Functionality
  Stack-based system to revert booking changes.



## System Architecture

The application follows a 3-Tier Architecture:

### Presentation Layer (UI)

* Built using Windows Forms
* Provides a user-friendly interface

### Business Logic Layer (BLL)

* Implements core logic
* Uses Linked Lists, BST, and Stacks

### Data Access Layer (DAL)

* Handles communication with SQL Server
* Manages data storage and retrieval


## Setup and Installation

### Step 1: Database Setup

* Navigate to the `/Database` folder
* Open `SQLQuery1.sql` in SQL Server Management Studio (SSMS)
* Execute the script



### Step 2: Open Project

* Open Visual Studio
* Load `hotel.sln` from the `/Source` folder



### Step 3: Configuration

* Update the connection string in the code
* Match it with your local SQL Server instance


### Step 4: Run Application

* Press F5 or click Start


## Developer Information

* Name: Zainab Rafique
* Institute: Karachi Institute of Economics and Technology (KIET)


## Conclusion

StaySavvy provides a reliable and efficient solution for hotel management. By combining strong data structure concepts with a practical GUI-based system, it ensures high performance and usability in real-world scenarios.


## Future Enhancements

* Online booking system
* Mobile application integration
* Cloud database support
* Role-based access control

---
