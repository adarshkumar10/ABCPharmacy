## Project changes, setup and run instructions

This section documents the recent changes made to the repository and how to run the solution locally.

### Summary of changes
- Created ASP.NET Core Web API project: `src/ABCPharmacy.Api`
  - Key files: `Program.cs`, `Controllers/MedicinesController.cs`, `Controllers/SalesController.cs`, `Repositories/*`, `Services/*`, `Middleware/ExceptionMiddleware.cs`, `Data/medicines.json`, `Data/sales.json`.
- Replaced UI with an Angular 21 single-page app: `src/ABCPharmacy.UI`
  - Key files: `package.json`, `proxy.conf.json`, `src/main.ts`, `src/app/*` (components, services, models), `angular.json`, `tsconfig.json`.
- JSON persistence: medicine and sale records are stored in `src/ABCPharmacy.Api/Data/medicines.json` and `sales.json`.
- Implemented: DI, repository/service pattern, Swagger, CORS, global exception handling, sale recording and stock reduction.

### Local run (development)
1. Ensure .NET 8 SDK and Node.js installed. Install Angular CLI if needed (`npm i -g @angular/cli@21` or use `npx`).

2. From repository root (`C:\AdarshWork\ABCPharmacy`):
   - Restore and run API:
     ```powershell
     cd src\ABCPharmacy.Api
     dotnet restore
     dotnet run
     ```
     Default API URLs (configured): `http://localhost:5001` and `https://localhost:7201` (see `Properties/launchSettings.json`).

   - Run Angular UI (separate terminal):
     ```powershell
     cd src\ABCPharmacy.UI
     npm install
     npm start
     ```
     Angular dev server: `http://localhost:5002` (proxy configured to forward `/api` to the API HTTPS address).

3. API Swagger (development): `https://localhost:7201/swagger`.

### Ports and proxy
- The Angular dev server uses `proxy.conf.json` to proxy `/api` to `https://localhost:7201` so you can call the API without CORS problems. If you change API ports, update `src\ABCPharmacy.UI\proxy.conf.json` and the `applicationUrl` in `src\ABCPharmacy.Api\Properties\launchSettings.json`.

### Troubleshooting
- Address already in use:
  - Find process using port (PowerShell):
    ```powershell
    netstat -ano | findstr ":5001"
    taskkill /PID <PID> /F
    ```
  - Or change the port in `launchSettings.json` and update `proxy.conf.json`.

- CodeDom provider error (missing `Microsoft.VisualC.CppCodeProvider`):
  - Search for the entry in project configs or system `machine.config` and remove/comment the `<compiler ... type="Microsoft.VisualC.CppCodeProvider,..."/>` line. Backup `machine.config` before editing.

### Committing changes
- Recommended commit after adding project files:
