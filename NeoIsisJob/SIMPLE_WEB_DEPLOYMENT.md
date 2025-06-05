# Simple Web Application Deployment Guide - Remote Desktop IIS

## Overview
Deploy **Workout.Web** (MVC application) to IIS on remote desktop `172.30.248.145`. The web application includes all Core services and will connect directly to the database.

## Current Configuration ✅
- **Database**: `WIN-IVAPD6T4EJF\MSSQLSERVER01` 
- **External Access**: Configured for `0.0.0.0:8080`
- **Project**: .NET 8.0, Self-contained ready

## Step 1: Publish Web Application

### 1.1 Publish from Visual Studio
1. Right-click **Workout.Web** project → **Publish**
2. Choose **Folder**
3. **Target Location**: `publish\WebApp`
4. Click **Show all settings**:
   - **Configuration**: Release
   - **Target Framework**: net8.0
   - **Deployment Mode**: Self-contained
   - **Target Runtime**: win-x64
5. Click **Publish**

### 1.2 Or Publish via Command Line
```powershell
dotnet publish Workout.Web --configuration Release --output "publish\WebApp" --self-contained true --runtime win-x64
```

## Step 2: Deploy to Remote Desktop

### 2.1 Copy Published Files
1. Copy entire `publish\WebApp` folder to remote desktop
2. Place it in: `C:\inetpub\wwwroot\WorkoutApp`

### 2.2 Configure IIS Website

#### Create Website in IIS Manager
1. Open **IIS Manager** on remote desktop
2. Right-click **Sites** → **Add Website**
3. **Site name**: `WorkoutWeb`
4. **Physical path**: `C:\inetpub\wwwroot\WorkoutApp`
5. **Binding**:
   - **Type**: HTTP
   - **IP**: All Unassigned ⭐ **(IMPORTANT: Not localhost!)**
   - **Port**: `8080`
   - **Host name**: (leave empty)
6. Click **OK**

#### Configure Application Pool
1. Go to **Application Pools**
2. Select **WorkoutWeb** (auto-created)
3. **Advanced Settings**:
   - **.NET CLR Version**: **No Managed Code** ⭐ **(Critical for .NET 8)**
   - **Identity**: ApplicationPoolIdentity
   - **Start Mode**: AlwaysRunning

## Step 3: Configure Network Access

### 3.1 Windows Firewall (Run as Administrator)
```powershell
New-NetFirewallRule -DisplayName "Workout Web App" -Direction Inbound -Protocol TCP -LocalPort 8080 -Action Allow
```

### 3.2 Verify Firewall Rule
```powershell
Get-NetFirewallRule -DisplayName "Workout Web App"
```

## Step 4: Configure Database Access

### 4.1 SQL Server Configuration
1. **SQL Server Configuration Manager**:
   - Enable **TCP/IP** protocol
   - Restart SQL Server service

2. **Grant Database Access**:
   - Give **IIS AppPool\WorkoutWeb** access to Workout database
   - Or use SQL Server authentication

3. **Test Database Connection**:
```powershell
# From remote desktop
sqlcmd -S WIN-IVAPD6T4EJF\MSSQLSERVER01 -d Workout -E
```

## Step 5: Test Deployment

### 5.1 Local Test (on Remote Desktop)
```powershell
# Test local access
Start-Process "http://localhost:8080"
```

### 5.2 External Test (from your development machine)
Open browser and navigate to:
```
http://172.30.248.145:8080
```

### 5.3 Test All Features
- [ ] Home page loads
- [ ] Workouts page displays data
- [ ] Can create new workouts
- [ ] Can view workout details
- [ ] All CRUD operations work

## Step 6: Troubleshooting

### 6.1 "Cannot access externally"
✅ **Check IIS Binding**: Must be "All Unassigned" not "127.0.0.1"
✅ **Check Firewall**: Port 8080 must be open
✅ **Check Router**: Port forwarding may be needed

### 6.2 "Database connection error"
✅ **Connection String**: Verify `WIN-IVAPD6T4EJF\MSSQLSERVER01`
✅ **SQL Server**: TCP/IP enabled and service running
✅ **Permissions**: App pool identity has database access

### 6.3 "500 Internal Server Error"
✅ **Application Pool**: Must be "No Managed Code"
✅ **Dependencies**: Self-contained should include everything
✅ **Logs**: Check Windows Event Viewer

### 6.4 Detailed Error Logging
Add to `appsettings.json` temporarily:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Debug"
    }
  }
}
```

## Your Application URLs

- **Web Application**: `http://172.30.248.145:8080`
- **From Remote Desktop**: `http://localhost:8080`

## Architecture Benefits

✅ **Single Deployment**: Web app includes all Core services  
✅ **Direct Database Access**: No need for separate API deployment  
✅ **External Access**: Available from any internet connection  
✅ **Neo-Isis Compliant**: MVC Controllers → Core Services → Database  

## Quick Commands Summary

```powershell
# 1. Publish
dotnet publish Workout.Web --configuration Release --output "publish\WebApp" --self-contained true --runtime win-x64

# 2. Configure Firewall (on remote desktop)
New-NetFirewallRule -DisplayName "Workout Web App" -Direction Inbound -Protocol TCP -LocalPort 8080 -Action Allow

# 3. Test External Access
# Browse to: http://172.30.248.145:8080
```

Your web application is now deployed and accessible externally! 🚀 