StaySavvy: Hotel Room Management System
StaySavvy is a comprehensive software application developed to optimize operations within small to medium-sized hotels. This project successfully leverages key data structures to provide a robust and efficient user experience.

Introduction and Background
The system is implemented as a Windows Forms application using C#. It provides an intuitive graphical user interface (GUI) that enables hotel staff to effectively manage room reservations, guest information, and availability. The primary objective is to streamline hotel management processes, enhancing both operational efficiency and the overall user experience.

Technical Implementation (Data Structures)
This project utilizes fundamental data structures to handle complex logic:

Linked Lists: Implemented for efficient room management, allowing for dynamic adding, updating, and deleting of room details.

Binary Search Trees (BST): Utilized to provide accelerated search functionalities, enabling staff to quickly identify rooms by Room Number or Guest ID.

Stacks: Integrated to provide robust Undo/Redo capabilities specifically for booking modifications.

Key Features
Room Management: Full CRUD (Create, Read, Update, Delete) operations for the hotel's room inventory.

Booking System: Facilitates seamless room reservations and cancellations with real-time availability updates.

Customer Management: Stores guest profiles and reservation history, searchable by Name or NIC Number.

Comprehensive Reporting: Generates detailed insights regarding revenue, occupancy, and customer statistics.

Undo/Redo Actions: A stack-based system to revert or reinstate booking changes, preventing data entry errors.

System Architecture
The application follows a multi-layered (3-Tier) architecture to ensure modularity and maintainability:

Presentation Layer (UI): Built with Windows Forms for a user-friendly desktop experience.

Business Logic Layer (BLL): Orchestrates data flow and implements core algorithms using Linked Lists, BSTs, and Stacks.

Data Access Layer (DAL): Manages all interactions with the SQL Server database for data persistence.

Setup and Installation
Database: Navigate to the /Database folder and run the SQLQuery1.sql script in SQL Server Management Studio (SSMS).

Open Project: Launch Visual Studio and open the hotel.sln file located in the /Source folder.

Configuration: Update the connection string in the code to match your local SQL Server instance.

Run: Press F5 or click "Start" to launch the application.

Developer Information
Name: Zainab Rafique

Institute: Karachi Institute of Economics and Technology (KIET)

Conclusion
StaySavvy stands as a robust solution for the hospitality industry, successfully meeting its goals of delivering functional, reliable, and high-performance room management through advanced computer science principles
