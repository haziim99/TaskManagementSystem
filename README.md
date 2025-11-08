# 🧩 Task Management System v2.0 + Role-Based Access Control

A full-featured **Task Management System** built with **ASP.NET Core MVC 7.0**, **Entity Framework Core**, and **SQL Server**, supporting **Admin**, **Manager**, and **User** roles, dynamic notifications, analytics, task assignment, and a modern responsive UI.

---

## 🔥 Features Overview

### Task Management
- ✅ Create, Read, Update, Delete tasks
- 🔍 Search & filter tasks by title, priority, status
- 🎯 Task statuses: Todo / In Progress / Completed
- ⏰ Overdue detection & due tomorrow alerts
- 👥 Task assignment (Manager/Admin)
- 🔔 Real-time notifications on assignments and completions

### User & Profile
- 👤 User profile with personal info, bio, profile picture
- 🧮 Task statistics: Total, Completed, Pending tasks
- 🔒 Password change & optional 2FA support

### Analytics & Dashboard
- 📊 Performance trends & task completion charts
- ✅ Visual summary: Completed, Pending, Overdue
- 📈 Team analytics for Managers
- 🛡️ Admin panel for full system overview

### Modern UI
- 💡 Responsive design with Bootstrap 5 & icons
- 🗄️ Organized MVC structure for maintainable code
- 🔔 Navbar shows notifications & role badges
- ⚡ Dynamic modals & dropdowns

---

## 👥 Role Definitions

| Role    | Permissions |
|---------|-------------|
| **User** | View/complete assigned tasks only |
| **Manager** | Create tasks, assign to users, view created/assigned tasks, edit/delete own tasks |
| **Admin** | Full access: manage users, roles, all tasks, admin dashboard |

---

## 🗂️ Project Structure (Updated)

TaskManagementSystem/
├── Controllers/
│ ├── AccountController.cs ✅ Auth + Role assignment
│ ├── AnalyticsController.cs ✅ User-specific analytics
│ ├── AdminController.cs ✅ Admin panel
│ ├── HomeController.cs ⚪ No change
│ ├── NotificationsController.cs ✅ Dynamic notifications
│ └── TasksController.cs ✅ Role-based tasks
├── Models/
│ ├── ApplicationUser.cs ✅ Identity + custom fields
│ ├── TaskItem.cs ✅ Task assignment fields
│ ├── Notification.cs ✅ UserId field added
│ └── ErrorViewModel.cs
├── ViewModels/
│ ├── RegisterViewModel.cs ✅ Role selection
│ ├── LoginViewModel.cs
│ └── AdminViewModels.cs ✅ User management
├── Services/
│ ├── INotificationService.cs
│ ├── NotificationService.cs
│ └── NotificationBackgroundService.cs
├── Views/
│ ├── Account/
│ │ ├── Register.cshtml ✅ Role dropdown
│ │ └── Login.cshtml
│ ├── Admin/
│ │ ├── Index.cshtml ✅ User list
│ │ ├── UserDetails.cshtml
│ │ ├── EditUser.cshtml
│ │ └── AllTasks.cshtml
│ ├── Tasks/
│ │ ├── Index.cshtml ✅ Role-based display
│ │ ├── Create.cshtml ✅ Assignment dropdown
│ │ ├── Edit.cshtml
│ │ └── Dashboard.cshtml
│ ├── Profile/
│ │ ├── Index.cshtml
│ │ └── Edit.cshtml
│ ├── Notifications/
│ │ └── Index.cshtml
│ └── Shared/
│ └── _Navbar.cshtml ✅ Role badges + admin links
├── Data/
│ ├── ApplicationDbContext.cs ✅ Updated relationships
│ └── DbSeeder.cs ✅ Seed roles & default admin
├── Constants/
│ └── Roles.cs ✅ Admin / Manager / User
├── Migrations/ ✅ New
├── Program.cs ✅ Role seeding
├── wwwroot/
│ ├── css/
│ ├── js/
│ └── images/default-avatar.png
├── appsettings.json
└── README.md

yaml
Copy code

---

## 🛠️ Database Changes

### Tables & Columns

**AspNetRoles** – Admin / Manager / User  
**AspNetUserRoles** – Links users to roles  

**Tasks**  
- `CreatedByUserId` (FK) – Creator  
- `AssignedToUserId` (FK) – Assigned user  

**Notifications**  
- `UserId` (FK) – Receiver  

---

## ⚡ Role-Based Task Permissions

| Action | User | Manager | Admin |
|--------|------|---------|-------|
| View own tasks | ✅ | ✅ | ✅ |
| View team tasks | ❌ | ✅ | ✅ |
| View all tasks | ❌ | ❌ | ✅ |
| Create tasks | ❌ | ✅ | ✅ |
| Assign tasks | ❌ | ✅ | ✅ |
| Delete own tasks | ❌ | ✅ | ✅ |
| Delete any task | ❌ | ❌ | ✅ |
| Manage users | ❌ | ❌ | ✅ |
| Change roles | ❌ | ❌ | ✅ |
| Access admin panel | ❌ | ❌ | ✅ |

---

## 🚀 Setup Instructions

### Prerequisites
- .NET 7.0 SDK
- SQL Server / LocalDB
- Visual Studio / VS Code / Rider

### Installation
1. Install NuGet packages:
```bash
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.UI
Create folders & files as per project structure

Add Identity, Task, Notification, and ViewModel models

Update Program.cs to seed roles and default admin

Update ApplicationDbContext.cs for task assignments & relationships

Apply migrations:

bash
Copy code
dotnet ef migrations add AddRoleSystemAndTaskAssignment
dotnet ef database update
Run app:

bash
Copy code
dotnet run
Open browser: https://localhost:5001

🔐 Default Admin Account
pgsql
Copy code
Email: admin@taskmanagement.com
Password: Admin@123
Role: Admin
⚠️ Change after first login!

🧪 Testing Checklist (Combined)
User
 Register/Login as User

 Can only see assigned tasks

 Cannot create tasks or access admin panel

 Can complete assigned tasks

Manager
 Register/Login as Manager

 Can create and assign tasks

 Can see created & assigned tasks

 Cannot access admin panel

Admin
 Login with default admin

 Access admin panel

 View/edit/delete users

 Change roles

 See all tasks

 Assign tasks to anyone

Task Flow
 Manager assigns task → User sees & completes → Manager/Admin notified

 Role permissions enforced on Create/Edit/Delete tasks

Security
 Unauthorized access returns 403

 Users cannot access others' tasks

 Proper [Authorize(Roles="...")] attributes used

Notifications & Analytics
 Dynamic notifications work

 Badge counts update

 Analytics charts show performance correctly

 Background service refreshes hourly

✅ Success Criteria
✅ Role-based access enforced

✅ Tasks and notifications user-specific

✅ Admin panel functions correctly

✅ Managers can assign tasks

✅ Users receive notifications

✅ Profile & analytics data dynamic

✅ No console or runtime errors

📞 Troubleshooting
Migration fails: Drop DB & re-run migration

Login issues: Verify Identity tables & roles

Navbar missing info: Check _Navbar.cshtml

Notifications missing: Verify background service registration

🎉 Completion
System ready for production

Roles: Admin, Manager, User

Task assignment, notifications, analytics, admin panel fully functional

Secure & role-based access control applied