# LOGIN-SERVICE

A .NET 10 Web API providing endpoints for user registration, login, and retrieving user information, backed by PostgreSQL and Entity Framework Core.
The entire application is fully containerized using Docker and Docker Compose.

### Features

- User registration & authentication
- Retrieve user details
- PostgreSQL database with EF Core migrations
- Fully Dockerized (API + PostgreSQL)

### Technologies Used

* .NET 10
* C#
* Entity Framework Core 10
* PostgreSQL 16
* Docker & Docker Compose

### Prerequisites
Make sure you have the following installed:

* .NET 10 SDK
* Docker
* Docker Compose

### Setup & Run Guide
1. Install EF Core CLI (if not already installed)
   - dotnet tool install --global dotnet-ef
   - export PATH="$HOME/.dotnet/tools:$PATH"
   - dotnet ef --version
2. Restore Packages
   - dotnet restore
3. Add Initial Migration (one-time)
   - dotnet ef migrations add InitialCreate
4. Build & Start Docker Containers
   - docker compose up --build
   - Docker will:
   - Start PostgreSQL first
   - Start Login-Service and automatically apply migrations (db.Database.Migrate())
   - Host the API on: http://localhost:8080
5. Verify Database
   - docker compose exec db psql -U postgres -d users
   - \dt
   - users

### API Endpoints
1. POST /register
   - Registers a new user.
3. POST /login
   - Authenticates an existing user.
5. GET /users
   - Returns all registered users.

