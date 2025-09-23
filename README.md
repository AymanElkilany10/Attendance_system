```markdown
# 🕒 Company Attendance System

A web-based system to manage employee attendance in a company.  
Built with **ASP.NET Core** and follows a layered architecture for maintainability and scalability.

---

## 📌 Features
- 👤 **Employee Management** – Add, update, and manage employees.  
- ⏰ **Attendance Tracking** – Record daily check-in and check-out times.  
- 📊 **Reports** – Generate summaries of attendance and working hours.  
- 🔒 **Role-Based Access** – Admins, managers, and employees have different permissions.  
- 🗂️ **Layered Architecture** – Separation of concerns with **Business Logic**, **Data Access**, and **Presentation** layers.  

---

## 🏗️ Tech Stack
- **C#**  
- **ASP.NET Core Web API**  
- **Entity Framework Core**  
- **SQL Server**  

---

## 📂 Project Structure
```

Attendance\_system/
│── Attendance\_system/        # Presentation Layer (Web API - Controllers)
│── BusinessLogic/            # Business services and DTOs
│── DataAccess/               # Database context and repositories
│── Models/                   # Entity models
│── Migrations/               # EF Core migrations

````

---

## 🚀 Getting Started

### 1️⃣ Clone the Repository
```bash
git clone https://github.com/AymanElkilany10/Attendance_system.git
cd Attendance_system
````

### 2️⃣ Configure Database

* Update the connection string in `appsettings.json`.
* Run migrations:

```bash
dotnet ef database update
```

### 3️⃣ Run the Project

```bash
dotnet run
```

The API will be available at: `https://localhost:5001`

---

## 🔗 API Endpoints

Here are some of the key API endpoints (from `Controllers`):

### 👤 Employee Management

| Method   | Endpoint              | Description         |
| -------- | --------------------- | ------------------- |
| `GET`    | `/api/employees`      | Get all employees   |
| `GET`    | `/api/employees/{id}` | Get employee by ID  |
| `POST`   | `/api/employees`      | Create new employee |
| `PUT`    | `/api/employees/{id}` | Update employee     |
| `DELETE` | `/api/employees/{id}` | Delete employee     |

### 🕒 Attendance

| Method | Endpoint                              | Description                        |
| ------ | ------------------------------------- | ---------------------------------- |
| `POST` | `/api/attendance/checkin`             | Mark employee check-in             |
| `POST` | `/api/attendance/checkout`            | Mark employee check-out            |
| `GET`  | `/api/attendance/{employeeId}/report` | Get attendance report for employee |

### 🔑 Authentication (if enabled)

| Method | Endpoint             | Description         |
| ------ | -------------------- | ------------------- |
| `POST` | `/api/auth/login`    | User login          |
| `POST` | `/api/auth/register` | Register a new user |

> ⚠️ Endpoints may differ based on final implementation. Check controller attributes for exact routes.

---

## 👥 Contributors

* [@AymanElkilany10](https://github.com/AymanElkilany10) – Project Owner
* [@ahmed-khalid2004](https://github.com/ahmed-khalid2004) – Contributor

---

## 📜 License

This project is licensed under the MIT License.

```

---


