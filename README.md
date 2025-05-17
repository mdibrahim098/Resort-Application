# 🏝️ White Lagoon Resort Booking System - ASP.NET Core 8 MVC Project

A full-stack resort booking system built with **ASP.NET Core 8 MVC**, **Clean Architecture**, **Stripe Payments**, **Entity Framework Core**, and advanced admin features like dashboard charts and document exports. Designed for beginners to intermediate developers who want real-world project experience with .NET Core.

---

## 🚀 What You'll Learn

- ✅ Structure of a Clean Architecture ASP.NET Core 8 MVC Project
- ✅ Building Web Applications with ASP.NET Core MVC
- ✅ Custom ASP.NET Identity (not Razor Class Library)
- ✅ Entity Framework Core with Code-First Migrations
- ✅ Repository Pattern + Dependency Injection
- ✅ Admin Dashboard with Summary, Charts & User Management
- ✅ Stripe Payment Gateway Integration
- ✅ Dynamic Exporting to PDF, PPT, and Word
- ✅ Charts using Chart.js or other libraries
- ✅ Deployment to MyWindowsHosting
- ✅ Seeding the Database Automatically

---

## 🛠️ Tech Stack

- **ASP.NET Core 8 MVC**
- **Entity Framework Core**
- **SQL Server**
- **Stripe API (Payments)**
- **ASP.NET Identity**
- **Bootstrap 5 / AdminLTE / SB Admin**
- **Chart.js or similar for Analytics**
- **SelectPDF / Syncfusion / EPPlus for Exports**
- **MyWindowsHosting (Deployment)**

---

## 📁 Project Structure

```text
/WhiteLagoonBooking
│
├── WhiteLagoon.Web               # ASP.NET Core MVC UI
├── WhiteLagoon.Application       # Application layer (Services, DTOs)
├── WhiteLagoon.Infrastructure    # DB Access, EF Core, Repositories
├── WhiteLagoon.Domain            # Core Domain Models
├── WhiteLagoon.Utility           # Helpers, Constants, Export Services
├── WhiteLagoon.DataAccess        # EF DbContext, Identity setup
│
└── README.md
🧪 Features
Feature	Description
Room Booking	View rooms, check availability, and book
Stripe Payment Integration	Real-time card processing
Admin Panel	View bookings, check-in/out, manage villas
Role-based Identity	Admin & Customer roles
Dashboard & Charts	Booking analytics, revenue, availability
Dynamic Reports	Export PDF, PPT, and Word
Clean Architecture	Proper separation of concerns
Seeding and Migration	Automated on startup
Deployed to Hosting	On MyWindowsHosting

🖥️ Prerequisites
Visual Studio 2022 or later

SQL Server Management Studio

.NET 8 SDK

Stripe Account (for keys)

MyWindowsHosting account (for deployment)

⚙️ Getting Started
Clone the Repository
bash
Copy
Edit
git clone https://github.com/yourusername/WhiteLagoonBooking.git
cd WhiteLagoonBooking
Setup Database & Stripe
Update your appsettings.json with:

SQL Server Connection String

Stripe API Keys

Run Migrations & Seed Database
In the Package Manager Console:

powershell
Copy
Edit
Update-Database
Or enable automatic migration/seeding on app start.

🚨 Deployment
Deploy to MyWindowsHosting
Publish the project using Visual Studio → Publish → Folder Profile

Upload via FTP or File Manager to MyWindowsHosting

Configure SQL Server and connection strings via the hosting panel

Ensure ASP.NET Core 8 runtime is enabled in your hosting plan

