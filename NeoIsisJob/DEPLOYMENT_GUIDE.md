# Workout Application - Remote Desktop Self-Contained Deployment Guide

## Architecture Overview

This solution follows the **Neo-Isis Architecture** deployed to a remote desktop server:

1. **NeoIsisJob (WPF Desktop App)** - Local client connecting to remote API
2. **Workout.Web (MVC Web App)** - Self-contained deployment on remote desktop
3. **Workout.Server (API Layer)** - Self-contained deployment on remote desktop
4. **Workout.Core (Service Layer)** - Shared library
5. **SQL Server Express** - Database on remote desktop

```
Desktop App (Local) ──HTTP──> Remote Desktop API ──> SQL Server Express
External Web Access ───────> Remote Desktop Web App ──> SQL Server Express
```

## Prerequisites

- **Development Machine**: Visual Studio 2022, .NET 8.0 SDK
- **Remote Desktop**: Windows Server/10/11, SQL Server Express, IIS
- **Network**: External IP access to remote desktop

## Project Configuration
- **.NET Version**: 8.0
- **Deployment Type**: Self-contained
- **Database**: SQL Server Express
- **Remote Desktop IP**: `172.30.248.145`

## Step 1: Prepare Remote Desktop Environment

### 1.1 Install Required Software (on Remote Desktop)
```powershell
# Enable IIS with ASP.NET Core support
Enable-WindowsOptionalFeature -Online -FeatureName IIS-WebServerRole, IIS-WebServer, IIS-CommonHttpFeatures, IIS-HttpErrors, IIS-HttpRedirect, IIS-ApplicationDevelopment, IIS-NetFxExtensibility45, IIS-HealthAndDiagnostics, IIS-HttpLogging, IIS-Security, IIS-RequestFiltering, IIS-Performance, IIS-WebServerManagementTools, IIS-ManagementConsole, IIS-IIS6ManagementCompatibility, IIS-Metabase, IIS-ASPNET45

# Download and install ASP.NET Core 8.0 Hosting Bundle
# https://dotnet.microsoft.com/download/dotnet/8.0/runtime
```

### 1.2 Configure SQL Server Express
1. Ensure SQL Server Express is installed and running
2. Create `Workout` database
3. Enable remote connections (SQL Server Configuration Manager)
4. Configure Windows Authentication for the application pool identity

### 1.3 Create Application Directories
```powershell
New-Item -Path "C:\WorkoutApp" -ItemType Directory
New-Item -Path "C:\WorkoutApp\WebApp" -ItemType Directory  
New-Item -Path "C:\WorkoutApp\ApiServer" -ItemType Directory
```

## Step 2: Build and Publish Applications

### 2.1 Deploy Using PowerShell Script
Run this on your development machine:

```powershell
.\deploy-to-rmd.ps1
```

Or follow manual steps below:

### 2.2 Publish Web Application (Manual)
```powershell
dotnet publish Workout.Web --configuration Release --output "publish\WebApp" --self-contained true --runtime win-x64
```

### 2.3 Publish API Server (Manual)
```powershell
dotnet publish Workout.Server --configuration Release --output "publish\ApiServer" --self-contained true --runtime win-x64
```

### 2.4 Publish Desktop Application
```powershell
dotnet publish NeoIsisJob --configuration Release --output "publish\DesktopApp" --self-contained true --runtime win-x64
```

## Step 3: Deploy to Remote Desktop

### 3.1 Copy Files to Remote Desktop
Transfer these folders to the remote desktop:
- `publish\WebApp` → `C:\WorkoutApp\WebApp`
- `publish\ApiServer` → `C:\WorkoutApp\ApiServer`

### 3.2 Configure IIS for Web Application

#### Create IIS Site
1. Open IIS Manager on remote desktop
2. Right-click "Sites" → "Add Website"
3. **Site name**: `WorkoutWeb`
4. **Physical path**: `C:\WorkoutApp\WebApp`
5. **Binding**: 
   - **Type**: HTTP
   - **IP**: All Unassigned (0.0.0.0)
   - **Port**: 8080
   - **Host name**: (leave empty)
6. Click "OK"

#### Configure Application Pool
1. Go to "Application Pools"
2. Select "WorkoutWeb" pool
3. **Advanced Settings**:
   - **.NET CLR Version**: No Managed Code
   - **Identity**: ApplicationPoolIdentity
   - **Start Mode**: AlwaysRunning

### 3.3 Configure API Server as Windows Service

#### Install API Service
```powershell
# Run as Administrator on Remote Desktop
.\install-api-service.ps1 -ExePath "C:\WorkoutApp\ApiServer\Workout.Server.exe"
```

Or manually:
```powershell
sc.exe create WorkoutApiServer binPath= "C:\WorkoutApp\ApiServer\Workout.Server.exe" DisplayName= "Workout API Server" start= auto
Start-Service -Name WorkoutApiServer
```

## Step 4: Configure Network Access

### 4.1 Configure Windows Firewall
```powershell
# Run as Administrator on Remote Desktop
New-NetFirewallRule -DisplayName "Workout Web App" -Direction Inbound -Protocol TCP -LocalPort 8080 -Action Allow
New-NetFirewallRule -DisplayName "Workout API Server" -Direction Inbound -Protocol TCP -LocalPort 5261 -Action Allow
```

### 4.2 Configure SQL Server for Remote Access
1. **SQL Server Configuration Manager**:
   - Enable TCP/IP protocol
   - Set TCP Port to 1433 (if not default)
   - Restart SQL Server service

2. **Windows Firewall**:
   ```powershell
   New-NetFirewallRule -DisplayName "SQL Server" -Direction Inbound -Protocol TCP -LocalPort 1433 -Action Allow
   ```

3. **SQL Server Authentication**:
   - Enable Mixed Mode Authentication (if needed)
   - Configure service account permissions

## Step 5: Test External Access

### 5.1 Test from Remote Desktop (Local)
```powershell
# Test API Server
Invoke-RestMethod -Uri "http://localhost:5261/api/product"

# Test Web App
Start-Process "http://localhost:8080"
```

### 5.2 Test External Access
From any external machine:
- **Web Application**: `http://172.30.248.145:8080`
- **API + Swagger**: `http://172.30.248.145:5261/swagger`

### 5.3 Test Desktop Application
1. Build desktop app on development machine
2. Run `NeoIsisJob.exe` 
3. Verify it connects to `http://172.30.248.145:5261`
4. Test real-time data synchronization

## Step 6: Troubleshooting External Access

### 6.1 Web App Only Works on Localhost
✅ **Check IIS Bindings**: 
- Ensure binding is "All Unassigned" not "127.0.0.1"
- Port should be 8080

✅ **Check Application Configuration**:
- `appsettings.json` should have `"Urls": "http://0.0.0.0:8080"`

✅ **Verify Firewall Rules**:
```powershell
Get-NetFirewallRule -DisplayName "Workout*"
```

### 6.2 API Server Not Accessible Externally
✅ **Check Service Status**:
```powershell
Get-Service -Name "WorkoutApiServer"
```

✅ **Check Port Binding**:
- API should bind to `0.0.0.0:5261` not `localhost:5261`
- Check `appsettings.json`: `"Urls": "http://0.0.0.0:5261"`

✅ **Test Local API**:
```powershell
Invoke-RestMethod -Uri "http://localhost:5261/swagger"
```

### 6.3 Database Connection Issues
✅ **SQL Server Configuration**:
- SQL Server Browser service running
- TCP/IP protocol enabled
- Remote connections allowed

✅ **Connection String**:
```
Server=WIN-IVAPD6T4EJF\MSSQLSERVER01;Database=Workout;Integrated Security=True;TrustServerCertificate=True
```

✅ **Authentication**:
- Application pool identity has database access
- Or use SQL Server authentication

### 6.4 Network/Router Issues
✅ **Port Forwarding** (if behind router):
- Forward ports 8080 and 5261 to remote desktop
- Check ISP doesn't block these ports

✅ **Remote Desktop Network**:
- Ensure remote desktop has static IP or DDNS
- Check if hosting provider allows external access

## Step 7: Architecture Validation

Your deployment correctly implements Neo-Isis architecture:

✅ **Application Project (NeoIsisJob)**: WPF with ViewModels, ServiceProxy connecting to remote API  
✅ **Server API Project (Workout.Server)**: Self-contained API with controllers and DTOs  
✅ **Server Library Project (Workout.Core)**: Services, repositories, domain models  
✅ **Web Application (Workout.Web)**: MVC application with controllers consuming API services  
✅ **Database**: SQL Server Express with Entity Framework

## Step 8: Final Verification Checklist

- [ ] Remote desktop has IIS configured and running
- [ ] Web app accessible at `http://172.30.248.145:8080`
- [ ] API server running as Windows service
- [ ] API accessible at `http://172.30.248.145:5261/swagger`
- [ ] SQL Server Express accessible remotely
- [ ] Desktop app connects from external machine
- [ ] Real-time data sync between web and desktop
- [ ] All CRUD operations working

## Application URLs

- **Web Application**: `http://172.30.248.145:8080`
- **API + Swagger**: `http://172.30.248.145:5261/swagger`
- **Desktop App**: Runs locally, connects to remote API

## Benefits of This Architecture

1. **Centralized Data**: Single database on remote desktop
2. **Multi-Client Access**: Web app and desktop app use same API
3. **Real-time Sync**: Changes in web app immediately visible in desktop app
4. **External Access**: Applications accessible from anywhere
5. **Professional Deployment**: Self-contained, production-ready setup

Your solution demonstrates a complete Neo-Isis architecture with external accessibility and real-time synchronization! 