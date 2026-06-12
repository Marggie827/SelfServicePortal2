# Self-Service Portal

A web-based application built with Blazor Server, 
MudBlazor, SQL Server and Entity Framework Core.

## Technology Stack
- C# / .NET 10
- Blazor Server
- MudBlazor
- SQL Server
- Entity Framework Core

## Features
- User registration and login
- Submit and track service requests
- Admin panel for managing requests
- In-app notifications
- Audit logging

## Setup Instructions

### Prerequisites
- Visual Studio 2022
- SQL Server Express
- .NET 10 SDK

### Steps
1. Clone the repository
2. Update connection string in appsettings.json
3. Run migrations: `Update-Database`
4. Run the project

## Database
Seven tables: Users, Requests, Categories, 
Statuses, Comments, Notifications, AuditLogs

## Default Admin Setup
After registering, run this SQL to make admin:
UPDATE Users SET Role = 'Admin' 
WHERE Email = 'your-email@here.com';
