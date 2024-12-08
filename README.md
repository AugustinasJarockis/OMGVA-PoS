# OMGVA Point of Sales System

As part of the project for the Software Design module in our 5th semester of Software Engineering studies, we, a team of five members, created a PoS system for beauty and food sector.

## Techologies used:
- .NET for backend
- React for frontend
- Docker for local MSSQL database


## Run Instructions

### Setup local MSSQL database
**Prerequisites:** docker installed

1. Create MSSQL database container  
(this will create database 'OmgvaPos' with user 'omgva' and password 'admin')
```
cd "OMGVA PoS/docker/mssql-db"
docker compose up -d
```

2. Change LocalDatabase connectionString in _appsettings.json_  
   "LocalDatabase": "Data Source=localhost;Database=OmgvaPos;Integrated Security=false;User ID=omgva;Password=admin;TrustServerCertificate=True;"


3. Update database (run migrations)

Make sure to install dotnet-ef
```
dotnet tool install --global dotnet-ef --version 8.0.10
dotnet-ef --version
```

Update database
```
dotnet ef database update
```


