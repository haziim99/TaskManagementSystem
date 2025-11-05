# 📋 Task Management System

A comprehensive task management system built using ASP.NET Core MVC and Entity Framework Core.

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-7.0-blue)
![C#](https://img.shields.io/badge/C%23-11.0-purple)
![License](https://img.shields.io/badge/license-MIT-green)

## ✨ Features

- ✅ **Full Task Management** - Create, Read, Update, Delete (CRUD)
- 🔍 **Advanced Search & Filtering** - Search by title, status, and priority
- 🎯 **Multiple Priorities** - Low, Medium, High, Urgent
- 📊 **Task Statuses** - Todo, In Progress, Completed
- 📅 **Due Dates** - Track deadlines
- 🎨 **Modern UI** - Responsive design using Bootstrap 5
- 🗄️ **SQL Server Database** - Secure data storage
- 📱 **Fully Responsive** - Works on all devices
- 📊 **Dashboard View** - Quick statistics for tasks (Total, Completed, In Progress, Overdue)

## 🛠️ Technologies Used

- **Backend**: ASP.NET Core MVC 7.0
- **Language**: C# 11.0
- **Database**: SQL Server / LocalDB
- **ORM**: Entity Framework Core
- **Frontend**: HTML5, CSS3, Bootstrap 5
- **Icons**: Bootstrap Icons
- **Version Control**: Git & GitHub

## 📋 Project Structure

TaskManagementSystem/
├── Controllers/
│ └── TasksController.cs # CRUD + Dashboard method
├── Data/
│ └── ApplicationDbContext.cs # Database context
├── Models/
│ └── TaskItem.cs # Task data model (Title, Description, Status, Priority, DueDate, etc.)
├── Views/
│ ├── Tasks/
│ │ ├── Index.cshtml # Task list view (search, filter, CRUD links)
│ │ ├── Create.cshtml # Add new task
│ │ ├── Edit.cshtml # Edit task
│ │ ├── Details.cshtml # Task details
│ │ ├── Delete.cshtml # Delete task
│ │ └── Dashboard.cshtml # Dashboard view with statistics and recent tasks
│ └── Shared/
│ └── _Layout.cshtml # Main layout with navbar
├── wwwroot/
│ └── css/
│ └── site.css # Updated styles
├── appsettings.json # App settings
└── Program.cs # Application entry point

markdown
Copy code

## 📋 Requirements

- [.NET SDK 7.0](https://dotnet.microsoft.com/download) or later
- [SQL Server LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) or SQL Server
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

## 🚀 Installation & Running

### 1. Clone the Project

```bash
git clone https://github.com/haziim99/TaskManagementSystem.git
cd TaskManagementSystem
2. Restore Dependencies
bash
Copy code
dotnet restore
3. Apply Migrations & Update Database
bash
Copy code
dotnet ef database update
4. Run the Application
bash
Copy code
dotnet run
Then navigate to https://localhost:5001 (or the port shown in console).

📌 Notes
The Dashboard page provides a quick overview of task statistics and recent tasks.

All views are built using Bootstrap 5 with responsive design.

Enums are used for TaskStatus and TaskPriority for clarity and maintainability.

The project supports basic CRUD operations with validation on required fields.

pgsql
Copy code
