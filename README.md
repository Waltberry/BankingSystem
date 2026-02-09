# 📘 BankingSystem

A modular, production-style banking system built with **ASP.NET Core**, **Entity Framework Core**, and **Clean Architecture principles**.
This project is being developed as a learning and portfolio project, focusing on backend design, API development, and full-stack integration.

> 🚧 **Status:** In active development

---

## 📌 Project Overview

**BankingSystem** is a multi-project .NET solution that simulates core banking operations such as:

* Account creation
* Deposits and withdrawals
* Transfers between accounts
* Transaction history
* RESTful API access
* Web-based UI

The project follows a layered architecture to separate business logic, infrastructure, and presentation concerns.

---

## 🏗️ Solution Architecture

```
BankingSystem
│
├── Banking.Domain         → Core business entities and rules
├── Banking.Application    → Use cases, services, DTOs, interfaces
├── Banking.Infrastructure → Database, EF Core, repositories
├── Banking.Api            → REST API (ASP.NET Core)
├── Banking.Web            → Razor Pages web interface
└── Banking.Tests          → Unit tests
```

### Layer Responsibilities

| Layer          | Purpose                              |
| -------------- | ------------------------------------ |
| Domain         | Business rules, entities, invariants |
| Application    | Application services and use cases   |
| Infrastructure | Database and external services       |
| API            | HTTP endpoints                       |
| Web            | User interface                       |
| Tests          | Automated testing                    |

---

## ⚙️ Technology Stack

* **.NET** (current target: .NET 10 preview / LTS candidate)
* **ASP.NET Core Web API**
* **Entity Framework Core**
* **SQLite** (local development)
* **Razor Pages**
* **Swagger / OpenAPI**
* **xUnit** (testing)

---

## ✨ Features (Current & Planned)

### Implemented / In Progress

* ✔️ Modular multi-project solution
* ✔️ RESTful API
* ✔️ Account management
* ✔️ Deposit / Withdraw
* ✔️ Transfers (transactional)
* ✔️ EF Core integration
* ✔️ Swagger documentation

### Planned

* 🔲 Authentication (JWT)
* 🔲 Authorization (roles)
* 🔲 Transaction statements
* 🔲 Pagination & filtering
* 🔲 Logging & monitoring
* 🔲 Global error handling
* 🔲 CI/CD pipeline
* 🔲 Docker support
* 🔲 Integration tests

---

## 🚀 Getting Started (Development)

### Prerequisites

* Visual Studio 2022+
* .NET SDK (installed via VS Installer)
* SQLite (bundled via EF Core)

---

### Clone Repository

```bash
git clone https://github.com/<your-username>/BankingSystem.git
cd BankingSystem
```

---

### Restore & Build

```bash
dotnet restore
dotnet build
```

---

### Run the Solution

In Visual Studio:

1. Set startup projects:

   * `Banking.Api`
   * `Banking.Web`

2. Press **F5**

Or via CLI:

```bash
dotnet run --project Banking.Api
dotnet run --project Banking.Web
```

---

### API Documentation (Swagger)

When the API is running:

```
https://localhost:<port>/swagger
```

Use Swagger to test endpoints.

---

## 🗄️ Database & Migrations

Entity Framework Core is used for persistence.

### Create Migration

```bash
dotnet ef migrations add InitialCreate \
  --project Banking.Infrastructure \
  --startup-project Banking.Api
```

### Update Database

```bash
dotnet ef database update \
  --project Banking.Infrastructure \
  --startup-project Banking.Api
```

Database file: `banking.db`

---

## 🔄 High-Level Program Flow

```
User (Web UI / Client)
        ↓
   Banking.Web
        ↓ HTTP
   Banking.Api
        ↓
Banking.Application
        ↓
Banking.Domain
        ↓
Banking.Infrastructure (EF / SQLite)
```

---

## 🧠 Design Principles

* Separation of Concerns
* Dependency Injection
* Clean Architecture
* Domain-Driven Design (lightweight)
* SOLID principles
* Transactional integrity
* Testability

---

## 🧪 Testing

Unit tests are implemented using xUnit.

Run tests:

```bash
dotnet test
```

---

## 📁 Project Structure

```
/src
  /Banking.Domain
  /Banking.Application
  /Banking.Infrastructure
  /Banking.Api
  /Banking.Web

/tests
  /Banking.Tests
```

---

## 📈 Learning Objectives

This project is used to practice:

* Backend system design
* .NET multi-project solutions
* API development
* Database modeling
* Transaction handling
* Layered architectures
* Production-style workflows

---

## 📝 Development Status

Current focus:

* Stabilizing build
* Finalizing migrations
* Improving Web/API integration
* Cleaning dependency references

Next milestones:

* Authentication
* Statement endpoints
* Production error handling

---

## 🤝 Contributions

This is currently a personal learning and portfolio project.

Contributions, feedback, and suggestions are welcome.

---

## 👤 Author

**Onyero Walter Ofuzim**
M.Sc. Electrical & Software Engineering
University of Calgary
---