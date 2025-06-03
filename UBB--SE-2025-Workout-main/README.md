# NeoIsisJob - Workout Management System

Overview
--------
NeoIsisJob is a workout management system designed to help users manage workouts, exercises, and workout types. The system allows users to view, edit, and update workout details, including their associated exercises. This project is built using C# and follows the MVVM (Model-View-ViewModel) design pattern.

Features
--------
- Workout Management:
  - View a list of workouts.
  - Edit workout names using an in-page popup.
  - View Workout details.

- Exercise Integration:
  - Fetch exercises associated with a workout using the ExerciseService.

- Data Persistence:
  - Workouts are stored and managed using a repository pattern (WorkoutRepo).
  - SQL Server is used as the database backend.

Project Structure
-----------------
Key Components:

1. ViewModels:
   - SelectedWorkoutViewModel: Handles the logic for managing a single selected workout, including updating its name and fetching associated exercises.
   - WorkoutViewModel: Manages the list of workouts and their filtering based on workout types.

2. Services:
   - WorkoutService: Provides methods to interact with the WorkoutRepo for CRUD operations on workouts.
   - ExerciseService: Fetches exercise details by ID.
   - CompleteWorkoutService: Manages complete workouts and their associations.

3. Repositories:
   - WorkoutRepo: Handles direct database interactions for the Workouts table.

4. UI:
   - WorkoutPage.xaml: Displays the list of workouts and provides an in-page popup for editing workout details.

Code Highlights
---------------
SelectedWorkoutViewModel
------------------------
This ViewModel is responsible for managing the currently selected workout. Key methods and properties include:

- SelectedWorkout:
  - Represents the currently selected workout.
  - Automatically updates the associated exercises when set.

- UpdateWorkoutName(string newName):
  - Updates the name of the selected workout in the database.
  - Refreshes the UI to reflect the changes immediately.

- FilledCompleteWorkoutsWithExercies(IList<CompleteWorkoutModel> complWorkouts):
  - Populates the CompleteWorkouts collection with exercises fetched from the ExerciseService.

Example Code Snippets
----------------------
Updating a Workout Name
-----------------------
The UpdateWorkoutName method in SelectedWorkoutViewModel handles updating the workout name and refreshing the UI:
```csharp
public void UpdateWorkoutName(string newName)
{
    try
    {
        if (_selectedWorkout == null || string.IsNullOrWhiteSpace(newName))
        {
            throw new InvalidOperationException("Workout cannot be null and name cannot be empty or null.");
        }

        _selectedWorkout.Name = newName;
        this._workoutService.UpdateWorkout(_selectedWorkout);

        // Notify the UI about the change
        OnPropertyChanged(nameof(SelectedWorkout));

        // Reload the CompleteWorkouts collection if necessary
        IList<CompleteWorkoutModel> complWorkouts = FilledCompleteWorkoutsWithExercies(this._completeWorkoutService.GetCompleteWorkoutsByWorkoutId(this._selectedWorkout.Id));
        CompleteWorkouts = new ObservableCollection<CompleteWorkoutModel>(complWorkouts);

    }
    catch (Exception ex)
    {
        throw new Exception($"An error occurred while updating the workout: {ex.Message}", ex);
    }
}
```
Setting the Selected Workout
----------------------------
The SelectedWorkout property automatically updates the associated exercises when a new workout is selected:
```csharp
public WorkoutModel SelectedWorkout
{
    get => _selectedWorkout;
    set 
    { 
        _selectedWorkout = value;
        Debug.WriteLine($"SelectedWorkout set to: {_selectedWorkout?.Name}");
        OnPropertyChanged();

        // Update the collection
        IList<CompleteWorkoutModel> complWorkouts = FilledCompleteWorkoutsWithExercies(this._completeWorkoutService.GetCompleteWorkoutsByWorkoutId(this._selectedWorkout.Id));
        CompleteWorkouts = new ObservableCollection<CompleteWorkoutModel>(complWorkouts);
    }
}
```
Database Schema
---------------
Workouts Table
--------------
The Workouts table is used to store workout details. Below is the schema:

``` sql
CREATE TABLE Workouts (
    WID INT PRIMARY KEY IDENTITY(1, 1),
    [Name] VARCHAR(256) UNIQUE,
    WTID INT REFERENCES WorkoutTypes(WTID)
);
```

Key Points:
- WID: Primary key for the workout.
- Name: Unique name for the workout.
- WTID: Foreign key referencing the WorkoutTypes table.

More info about the database in the */database* directory.

How to Run the Project
----------------------
1. Set Up the Database:
   - Ensure you have a SQL Server instance running.

2. Run the Application:
   - Open the project in Visual Studio.
   - Build and run the application.
   - Navigate to the WorkoutPage to view and edit workouts.
