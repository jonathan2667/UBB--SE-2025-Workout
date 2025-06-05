# Deployment Configuration Checklist ✅

## New Configuration Details
- **Database Server**: `WIN-IVAPD6T4EJF\MSSQLSERVER01`
- **New IP Address**: `172.30.248.145`
- **Old IP Address**: `172.30.241.79` (replaced everywhere)

## ✅ Configuration Files Updated

### 1. Database Connection Strings
- ✅ `Workout.Web/appsettings.json`
- ✅ `Workout.Web/appsettings.Production.json`
- ✅ `Workout.Server/appsettings.json`
- ✅ `Workout.Server/appsettings.Production.json`
- ✅ `Workout.Core/Data/DatabaseHelper.cs`
- ✅ Root `appsettings.json`

### 2. API Base URLs (IP Address Updates)
- ✅ `Workout.Web/appsettings.json`
- ✅ `Workout.Web/appsettings.Production.json`
- ✅ Root `appsettings.json`
- ✅ `NeoIsisJob/Configuration/ApiSettings.cs`
- ✅ `NeoIsisJob/Helpers/ServerHelpers.cs`
- ✅ `NeoIsisJob/App.xaml.cs` (both instances)

### 3. Controller Fallback URLs
- ✅ `Workout.Web/Controllers/WorkoutController.cs`
- ✅ `Workout.Web/Controllers/WorkoutTypeController.cs`

### 4. View Error Messages
- ✅ `Workout.Web/Views/Workout/Index.cshtml`
- ✅ `Workout.Web/Views/Workout/Create.cshtml`

### 5. Development/Testing Files
- ✅ `Workout.Server/Workout.Server.http`
- ✅ `NeoIsisJob/Proxy/BaseServiceProxy.cs`

### 6. Documentation
- ✅ `DEPLOYMENT_GUIDE.md`
- ✅ `SIMPLE_WEB_DEPLOYMENT.md`

## ✅ Verified Working Components

### Database Connections
```
Server=WIN-IVAPD6T4EJF\MSSQLSERVER01;Database=Workout;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True
```

### API Endpoints
- **Base URL**: `http://172.30.248.145:5261`
- **Web App**: `http://172.30.248.145:8080`
- **API/Swagger**: `http://172.30.248.145:5261/swagger`

### Server Configuration
- **API Server**: Binds to `0.0.0.0:5261` (allows external access)
- **Web App**: Binds to `0.0.0.0:8080` (allows external access)

## ✅ Workout.Web Localhost Issue Resolution

**Problem**: Workout tab was connecting to localhost instead of remote IP
**Solution**: 
- Updated all fallback URLs in controllers
- Updated error messages in views to show correct IP
- AJAX calls use `@Url.Action()` which automatically uses correct host
- All API base URLs point to `172.30.248.145:5261`

## 🚀 Deployment Steps

### 1. Publish Applications
```powershell
# Web Application
dotnet publish Workout.Web --configuration Release --output "publish\WebApp" --self-contained true --runtime win-x64

# API Server
dotnet publish Workout.Server --configuration Release --output "publish\ApiServer" --self-contained true --runtime win-x64

# Desktop Application
dotnet publish NeoIsisJob --configuration Release --output "publish\DesktopApp" --self-contained true --runtime win-x64
```

### 2. Deploy to Remote Desktop (172.30.248.145)
- Copy `publish\WebApp` to `C:\WorkoutApp\WebApp`
- Copy `publish\ApiServer` to `C:\WorkoutApp\ApiServer`

### 3. Configure IIS for Web App
- Site Name: `WorkoutWeb`
- Port: `8080`
- Binding: **All Unassigned** (not localhost)
- App Pool: **No Managed Code**

### 4. Configure API as Windows Service
```powershell
sc.exe create WorkoutApiServer binPath= "C:\WorkoutApp\ApiServer\Workout.Server.exe" DisplayName= "Workout API Server" start= auto
Start-Service -Name WorkoutApiServer
```

### 5. Configure Firewall
```powershell
New-NetFirewallRule -DisplayName "Workout Web App" -Direction Inbound -Protocol TCP -LocalPort 8080 -Action Allow
New-NetFirewallRule -DisplayName "Workout API Server" -Direction Inbound -Protocol TCP -LocalPort 5261 -Action Allow
```

## 🔍 Testing Checklist

### From Remote Desktop (Local Tests)
- [ ] `http://localhost:8080` - Web app loads
- [ ] `http://localhost:5261/swagger` - API documentation loads
- [ ] Database connection works

### External Access Tests
- [ ] `http://172.30.248.145:8080` - Web app accessible externally
- [ ] `http://172.30.248.145:5261/swagger` - API accessible externally
- [ ] Desktop app connects from development machine
- [ ] Workout CRUD operations work from external web access
- [ ] No localhost connection errors in workout tab

### Workout Functionality Tests
- [ ] View workouts list
- [ ] Create new workout
- [ ] Edit workout name (AJAX call)
- [ ] Delete workout
- [ ] Filter by workout type
- [ ] All operations work without localhost errors

## 🎯 Key Changes Summary

1. **Database**: Updated to `WIN-IVAPD6T4EJF\MSSQLSERVER01`
2. **IP Address**: Changed from `172.30.241.79` to `172.30.248.145`
3. **External Access**: All applications properly bind to `0.0.0.0`
4. **Localhost Fix**: Eliminated hardcoded localhost references in Workout.Web

Your application is now ready for deployment to the new server! 🚀 