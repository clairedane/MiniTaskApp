# Mini Task Management App

## Overview

This is a mini Task Management application built using:

* **ASP.NET MVC 5** – UI layer
* **.NET Core Web API** – RESTful backend
* **Entity Framework (Code-First + Migrations)**
* **MySQL** – Database
* **jQuery / AJAX** – Client-side API calls

The application allows managing **Tasks**, **Task Items**, and **Employees**, with dynamic computation of overall task status.

---

## Architecture

MVC 5 (UI) communicates with the .NET Core Web API using AJAX.
The API uses Entity Framework Code-First with migrations to manage a MySQL database.

```
MVC 5 (UI)
    ↓
AJAX
    ↓
.NET Core Web API
    ↓
Entity Framework (Code-First)
    ↓
MySQL
```

---

## Business Rules

* A **Task does NOT contain a Status column**.
* Overall Task status is **computed dynamically** from its TaskItems.
* Task status is **never stored in the database**.
* TaskItem assignment to Employee is optional.
* APIs use **DTOs** (EF entities are not exposed directly).

### Task Status Logic

| Condition                                  | Status     |
| ------------------------------------------ | ---------- |
| No TaskItems                               | Empty      |
| All items New                              | New        |
| At least one InProgress (and not all Done) | InProgress |
| All items Done (with at least one item)    | Done       |

---

## API Endpoints

### Tasks

* `GET /api/tasks`
* `GET /api/tasks/{id}`
* `POST /api/tasks`
* `PUT /api/tasks/{id}`
* `DELETE /api/tasks/{id}`

### Task Items

* `POST /api/tasks/{taskId}/items`
* `PUT /api/taskitems/{id}`
* `DELETE /api/taskitems/{id}`

### Employees

* `GET /api/employees`
* `GET /api/employees/{id}`
* `POST /api/employees`
* `PUT /api/employees/{id}`
* `DELETE /api/employees/{id}`

---

## How to Run the Project

### 1. Configure Database

Update the connection string in:

`appsettings.json`

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;database=TaskDb;user=root;password=yourpassword;"
}
```

---

### 2. Apply Migrations

From Package Manager Console:

```
Add-Migration InitialCreate
Update-Database
```

The database is created using **Update-Database** as required.

---

### 3. Run the Projects

* Start the **API project**
* Set **MVC project** as startup project
* Ensure API base URL is correctly configured in MVC
