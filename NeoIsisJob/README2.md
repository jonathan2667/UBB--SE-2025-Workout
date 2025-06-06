# NeoIsis Workout Application - Authentication Implementation

## Login Information
**IMPORTANT:** For connections, there are already two users made:
- Username: **user1**, Password: **password1**
- Username: **user2**, Password: **password2**

## Setting Up Database Connection
To run the project, you must configure the connection strings:

1. Modify the connection string in **appsettings.json** in both Server and Web projects
2. Also modify the connection string in **Workout.Core/Data/WorkoutDb.cs**

Make sure to use your default SQL Server instance name in the connection strings.

## Database Migrations
Migrations have been added to the project. You must update your database on both CORE and WEB to incorporate all new migrations.

If updating does not work through normal methods, please use these commands in Package Manager Console:

```
# First drop the existing database
Drop-Database -Context WorkoutDbContext -Project Workout.Core -StartupProject Workout.Server

# Then update the database for Core
Update-Database -Context WorkoutDbContext -Project Workout.Core -StartupProject Workout.Server

# Finally update the database for Server
Update-Database -Context WorkoutDbContext -Project Workout.Server -StartupProject Workout.Server
```

**Note:** The PM commands should be run as a single line each (not broken across multiple lines).

## Authentication Implementation Details

### Major Changes Made

#### 1. Session-Based Authentication
- Implemented complete login and registration functionality
- Created AuthorizeUser filter to protect routes from unauthorized access
- Stored user information in session variables for persistent authentication
- Added conditional UI elements that display based on login state

#### 2. Removal of Hardcoded User ID
- Replaced all instances of hardcoded `userId=1` across the codebase
- Updated controllers to get the current user ID from session:
  ```csharp
  var userId = HttpContext.Session.GetString("UserId");
  ```
- Modified repositories to return all data rather than filtering to `userId=1`
- Controllers now filter data based on the current user's ID

#### 3. Controller Updates
- CartController: Now shows only current user's cart items
- WishlistController: Displays only items added by the logged-in user
- ClassController: Enrollments filtered by current user
- CalendarController: Events shown based on user's schedule
- ProfileController: Shows and updates only the logged-in user's profile

#### 4. User Experience Improvements
- Login/logout buttons displayed appropriately based on authentication state
- Navbar shows the actual username instead of "Hello User!"
- Added confirmation messages for actions like adding items to wishlist/cart
- Unauthorized users redirected to login page with return URL

#### 5. Code Implementation Details
- Used `HttpContext.Session` for maintaining authentication state
- Implemented cookie authentication with proper session timeout
- Added proper CSRF protection for forms
- Stored minimal user information in session for security
- Added validation for ownership before allowing modifications

Each component of the application now properly handles user-specific data, ensuring users only see their own information while preserving all functionality. 