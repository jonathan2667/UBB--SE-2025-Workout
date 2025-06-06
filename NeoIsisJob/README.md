# Workout / NeoIsisJob Solution

Welcome! This README will guide you step-by-step through **setting up**, **running**, and **understanding** the Workout application. No prior experience with EF Core, ASP.NET Core, or migrations is needed. Follow each section carefully.

---

## 📁 Solution Structure

```
/SolutionRoot
│
├── Workout.Core
│   ├── Data
│   │   └── WorkoutDbContext.cs        # EF Core DbContext and OnModelCreating
│   ├── Models                         # Entity classes (UserModel, WorkoutModel, CalendarDayModel, ...)
│   ├── IServices                      # Service interfaces (ICalendarService, IWorkoutService, ...)
│   └── Services                       # Implementations (CalendarService, WorkoutService, ...)
│
├── Workout.Server
│   ├── Controllers                    # Web API controllers (CalendarController, WorkoutController, ...)
│   ├── appsettings.json               # Connection strings and configuration
│   └── Program.cs                     # Host builder, DI, EF Core registration
│
└── NeoIsisJob
    ├── Views                          # XAML UI (CalendarPage.xaml, ...)
    ├── ViewModels                     # MVVM view models (CalendarViewModel.cs)
    ├── Proxy                          # HTTP client proxies (CalendarServiceProxy.cs)
    └── App.xaml                       # Application entry point
```

---

## 🔧 Prerequisites

1. **.NET 8.0 SDK**  
   Install from https://dotnet.microsoft.com/download

2. **SQL Server (LocalDB or full)**  
   LocalDB ships with Visual Studio. Or install SQL Server Express.

3. **Visual Studio 2022+**  
   - To run both server and client  
   - To open **Package Manager Console**

4. **Optionally VS Code**  
   - C# extension  
   - You can run `dotnet` CLI commands instead of PMC

---

## 🗄️ Database Setup & Migrations

> **All migrations are run from the Package Manager Console (PMC).**  
> In Visual Studio:  
> **Tools → NuGet Package Manager → Package Manager Console**

1. **Configure connection string**  
   In `Workout.Server/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=WorkoutDb;Trusted_Connection=True;MultipleActiveResultSets=true"
     }
   }
   ```

2. **Register the DbContext**  
   In `Program.cs`:
   ```csharp
   builder.Services.AddDbContext<WorkoutDbContext>(options =>
     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
   ```

3. **Add the initial migration**  
   In the PMC at the solution root (should be in tools/ but google it if you dont find it) (where the `.sln` is):
   ```
   PM> Add-Migration InitialCreate -Project Workout.Core -StartupProject Workout.Server
   ```
   - `-Project Workout.Core` points at the EF models / DbContext  
   - `-StartupProject Workout.Server` points at the Web API for config

4. **Apply the migration**  
   ```
   PM> Update-Database -Project Workout.Core -StartupProject Workout.Server
   ```
   This creates the database and tables.

5. **Seed lookup data**  
   - Then create a seed migration:
     ```
     PM> Add-Migration SeedInitialData -Project Workout.Core -StartupProject Workout.Server
     ```
   - And apply it again:
     ```
     PM> Update-Database -Project Workout.Core -StartupProject Workout.Server
     ```

> **Tip:** After any model change, repeat the Add-Migration & Update-Database steps with a new name.

---

## ▶️ Running the Solution

### A) In Visual Studio (Multiple Startup Projects)

1. **Select multiple startups**  
   - Right‑click the **Solution** → **Properties**  
   - Go to **Common Properties → Startup Project**  
   - Choose **Multiple startup projects**  

2. **Configure actions**  
   - For **Workout.Server** → **Action = Start**  
   - For **NeoIsisJob**    → **Action = Start**  

3. **Run (F5)**  
   - The Web API will start (e.g. `http://localhost:5261`)  
   - Then the WinUI client will launch and connect automatically  

### B) Using CLI (Two Terminals)

**Terminal 1: Server**  
```bash
cd /path/to/SolutionRoot
dotnet run --project Workout.Server
```
_Wait for "Now listening on http://localhost:5261"._

**Terminal 2: Client**  
```bash
cd /path/to/SolutionRoot
dotnet run --project NeoIsisJob
```
_The desktop app will open and fetch data from the API._

---

## 🔄 How It All Talks Together

1. **WinUI Client**  
   - `CalendarViewModel` calls methods on `CalendarServiceProxy`.

2. **CalendarServiceProxy**  
   - Issues HTTP requests (`HttpClient`) to `http://localhost:5261/api/...`.

3. **Web API**  
   - Routes requests to controllers (e.g. `CalendarController`).  
   - Calls into `ICalendarService` implementations.

4. **EF Core (Workout.Core)**  
   - Queries the `WorkoutDbContext` and returns entity data.

5. **SQL Server**  
   - Stores tables for Users, Workouts, CalendarDays, etc.

---

## 🐞 Troubleshooting

- **`405 Method Not Allowed` on DELETE**  
  Verify you're calling the correct route:  
  `/api/calendar/userworkout/{userId}/{workoutId}/{year}/{month}/{day}`  
  (not `/api/calendar/workout/...`)

- **`400 Bad Request` on POST**  
  Only send scalar fields for `UserWorkoutModel` (UID, WID, Date, Completed).  
  Do **not** include the full `User` or `Workout` navigation objects.

- **Migrations out of sync**  
  Always re-run in PMC:
  ```
  PM> Add-Migration YourChangeName -Project Workout.Core -StartupProject Workout.Server
  PM> Update-Database    -Project Workout.Core -StartupProject Workout.Server
  ```

---

You're all set—enjoy exploring the Workout calendar UI! 🎉

---

# README GENERATED by cursor

## 😃 Super Simple Guide to NeoIsisJob

### What Is This App?
This is a workout tracking app with a calendar view! You can:
- See your workouts on a calendar
- Add new workouts to specific days
- Track your fitness classes
- View your progress

### Architecture in Plain English

The app is split into 3 main parts:

1. **The Pretty Part (NeoIsisJob)** - What you see and click on
   - XAML files: These are the actual screens you see
   - ViewModels: They connect what you see with the data
   - Proxy classes: They talk to the server to get/save data

2. **The Brain Part (Workout.Server)** - The server that handles requests
   - It listens for requests from the app (like "give me all workouts")
   - It has controllers that handle these requests
   - It talks to the database to save or retrieve information

3. **The Memory Part (Workout.Core)** - Rules and data structures
   - Models: Define what a "Workout" or "User" looks like
   - Services: Handle business logic
   - Database context: Knows how to save to the database

### How Data Flows Through the App
1. You click "Add Workout" in the calendar
2. The Calendar View tells the CalendarViewModel
3. CalendarViewModel calls CalendarServiceProxy
4. The proxy sends an HTTP request to the server
5. The server controller receives the request
6. The controller asks the service to handle it
7. The service saves to the database
8. The response travels all the way back to update your screen

### Getting Started for Absolute Beginners

#### Step 1: Open the Project
1. Make sure you have Visual Studio 2022 installed
2. Open the solution file (NeoIsisJob.sln)
3. Wait for all packages to restore

#### Step 2: Set Up the Database
1. Open Package Manager Console (Tools → NuGet Package Manager → Package Manager Console)
2. Run this command: `Update-Database -Project Workout.Core -StartupProject Workout.Server`
3. Wait for it to finish (it creates all the necessary tables)

#### Step 3: Run Both Projects
1. Right-click on the Solution in Solution Explorer
2. Click "Set Startup Projects..."
3. Select "Multiple startup projects"
4. Set both "Workout.Server" and "NeoIsisJob" to "Start"
5. Click "OK"
6. Press F5 or click the green play button

#### Step 4: Use the App
1. The server will start first (you'll see a terminal window)
2. Then the app will open
3. You'll see the calendar view
4. Click on a day to add a workout
5. Use the navigation bar at the top to switch between views

### Common Problems and Solutions
- If the app shows blank pages: Check if the server is running
- If data doesn't save: Make sure your database connection string is correct
- If you see weird errors: Restart both the client and server

That's it! You're now ready to use and understand the NeoIsisJob workout app! 💪 