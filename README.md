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

### Local run (development)

     ```powershell
     cd src\ABCPharmacy.Api
     dotnet restore
     dotnet run
     ```
     Default API URLs (configured): `http://localhost:5001` and `https://localhost:7201` (see `Properties/launchSettings.json`).

     ```powershell
     cd src\ABCPharmacy.UI
     npm install
     npm start
     ```
     Angular dev server: `http://localhost:5002` (proxy configured to forward `/api` to the API HTTPS address).


### Ports and proxy

### Troubleshooting
    ```powershell
    netstat -ano | findstr ":5001"
    taskkill /PID <PID> /F
    ```

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