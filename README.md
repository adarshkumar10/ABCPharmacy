Here's the improved `README.md` file, incorporating the new content while maintaining the existing structure and coherence:

# ABC Pharmacy Project

## Overview
This project is designed to manage pharmacy operations, including medicine inventory and sales tracking. It consists of an ASP.NET Core Web API for backend services and an Angular single-page application for the frontend.

## Project changes, setup and run instructions

This section documents the recent changes made to the repository and how to run the solution locally.

### Summary of changes
- Created ASP.NET Core Web API project: `src/ABCPharmacy.Api`
  - Key files: `Program.cs`, `Controllers/MedicinesController.cs`, `Controllers/SalesController.cs`, `Repositories/*`, `Services/*`, `Middleware/ExceptionMiddleware.cs`, `Data/medicines.json`, `Data/sales.json`.
- Replaced UI with an Angular 21 single-page app: `src/ABCPharmacy.UI`
  - Key files: `package.json`, `proxy.conf.json`, `src/main.ts`, `src/app/*` (components, services, models), `angular.json`, `tsconfig.json`.
- JSON persistence: medicine and sale records are stored in `src/ABCPharmacy.Api/Data/medicines.json` and `sales.json`.
- Implemented: Dependency Injection (DI), repository/service pattern, Swagger documentation, Cross-Origin Resource Sharing (CORS), global exception handling, sale recording, and stock reduction.

### Local run (development)
1. Ensure .NET 8 SDK and Node.js are installed. Install Angular CLI if needed (`npm i -g @angular/cli@21` or use `npx`).

2. From the repository root (`C:\AdarshWork\ABCPharmacy`):
   - Restore and run the API:
     ```powershell
     cd src\ABCPharmacy.Api
     dotnet restore
     dotnet run
     ```
     Default API URLs (configured): `http://localhost:5001` and `https://localhost:7201` (see `Properties/launchSettings.json`).

   - Run the Angular UI (in a separate terminal):
     ```powershell
     cd src\ABCPharmacy.UI
     npm install
     npm start
     ```
     Angular dev server: `http://localhost:5002` (proxy configured to forward `/api` to the API HTTPS address).

3. Access the API Swagger documentation (development): `https://localhost:7201/swagger`.

### Ports and proxy
- The Angular dev server uses `proxy.conf.json` to proxy `/api` to `https://localhost:7201`, allowing you to call the API without CORS issues. If you change API ports, update `src\ABCPharmacy.UI\proxy.conf.json` and the `applicationUrl` in `src\ABCPharmacy.Api\Properties\launchSettings.json`.

### Troubleshooting
- **Address already in use**:
  - Find the process using the port (PowerShell):
    ```powershell
    netstat -ano | findstr ":5001"
    taskkill /PID <PID> /F
    ```
  - Alternatively, change the port in `launchSettings.json` and update `proxy.conf.json`.

- **CodeDom provider error (missing `Microsoft.VisualC.CppCodeProvider`)**:
  - Search for the entry in project configs or system `machine.config` and remove/comment the `<compiler ... type="Microsoft.VisualC.CppCodeProvider,..."/>` line. Backup `machine.config` before editing.

### Committing changes
- Recommended commit after adding project files:
  git add -A
  git commit -m "Add API and Angular UI (Angular 21) with JSON persistence, DI, services, Swagger and proxy"
  git push origin master

### Next steps
- Optionally configure the API to serve built Angular assets (`dist`) for a single-deployable artifact. I can provide the build-and-copy steps and API static file configuration.

---

(End of README)

This revised README maintains the original structure while integrating the new content seamlessly, ensuring clarity and coherence throughout the document.