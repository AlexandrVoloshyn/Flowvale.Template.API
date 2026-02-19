# Flowvale.Template.API — Run & Test

Quick instructions to build, run and manually test the API.

Prerequisites
- .NET 10 SDK installed (verify with `dotnet --info`).
- Git (optional, for cloning).
- Network access if the app calls external services (the `WolneLektury` client).

Build & run (CLI)
1. From repository root:
   - Restore and build:
     ```
     dotnet restore
     dotnet build
     ```
   - Run the API project:
     ```
     dotnet run --project src\Flowvale.Template.API --urls "http://localhost:5000"
     ```
   - The app will listen on the URL(s) you specify (default shown above).

Run in Visual Studio
1. Open the solution in Visual Studio.
2. Set `src\Flowvale.Template.API` as the startup project.
3. Press __F5__ to run with debugger or __Ctrl+F5__ to run without debugging.

Swagger / OpenAPI
- If Swagger is enabled, open:
  - `http://localhost:5000/swagger` (adjust port if different)
- Use Swagger UI to explore and execute endpoints.