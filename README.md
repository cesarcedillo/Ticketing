# 🎟️ Ticketing System

## Project Overview

Ticketing System is a simple yet robust ticket management platform built using Clean Architecture with a .NET 8 backend and a modern React frontend (Vite).

The solution includes:

* **Backend RESTful API** (`Ticketing.API`, .NET 8)
* **Frontend SPA** (`Ticketing`, React + Vite)
* **Infrastructure** (Entity Framework Core, SQLite for local/dev)
* **Unit tests** (backend)
* **Container orchestration** via Docker Compose for easy setup

---

## Setup Instructions

### Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/)
* [Node.js](https://nodejs.org/) (for frontend development, not needed for Docker)
* [Docker & Docker Compose](https://docs.docker.com/get-docker/) (recommended)
* **(Optional):** GitHub/GitLab account for private repository

---

### 1. Clone the Repository

```bash
git clone <your-private-repo-url>
cd Ticketing
```

---

### 2. Configuration

#### Backend

* For local development, edit secret file.

  ```
  {
    "SerilogMinimumLevel": "Debug",
    "RepositoryConnection": "Data Source=Ticketing.db"
  }
  ```

#### Frontend

* For local development, edit or create `Frontend/.env`

  ```
  VITE_API_BASE_URL=https://localhost:7086
  ```

---

### 3. Running the App

#### Option A: Docker Compose (Recommended)

This will run **both backend (API) and frontend** containers and use SQLite for persistence.

```bash
docker compose up --build
```

* **Frontend**: [http://localhost:3000](http://localhost:3000)
* **Backend API**: [http://localhost:8080](http://localhost:8080)

#### Option B: Run Locally (Development Mode)

**Backend:**

```bash
cd Backend/src
dotnet run --project Ticketing.API
```

* Default API: [https://localhost:7086](https://localhost:7086)

**Frontend:**

```bash
cd Frontend
npm install
npm run dev
```

* Vite Dev Server: [http://localhost:5173](http://localhost:5173)

---

## Assumptions

* SQLite is used for local development and when running via Docker by default. No external DB configuration required unless specified.
* The frontend expects the backend API URL to be set via `VITE_API_BASE_URL` (in `.env` for local dev or as a build arg for Docker).
* No authentication is implemented (public API for demonstration).
* All migrations are automatically applied on startup when using Docker.

---

## How to Run the Tests

**Backend tests:**

```bash
cd Backend/tests/Ticketing.Tests
dotnet test
```

**Frontend tests:**

If not implemented.

---

## SQLite Setup Notes

* **Docker:** The API uses a local SQLite file located at `/app/Ticketing.db` inside the container.
* **Local development:** If no connection string is set, defaults to a `Ticketing.db` file in the project folder.
* To inspect the DB, you can open the `.db` file with [DB Browser for SQLite](https://sqlitebrowser.org/) or similar tools.

---

## Available Endpoints

Sample RESTful endpoints:

* `GET /api/ticket` – List all tickets
* `GET /api/ticket/{id}` – Get ticket by ID
* `POST /api/ticket` – Create a new ticket
* `PUT /api/ticket/{id}` – Update ticket
* `DELETE /api/ticket/{id}` – Delete ticket
* `GET /api/user/{username}` – Get user by UserName

---
