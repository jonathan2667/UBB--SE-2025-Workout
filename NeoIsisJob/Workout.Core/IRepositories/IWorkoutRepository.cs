﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.IRepositories
{
    public interface IWorkoutRepository
    {
        Task<WorkoutModel> GetWorkoutByIdAsync(int workoutId);
        Task<WorkoutModel> GetWorkoutByNameAsync(string workoutName);
        Task InsertWorkoutAsync(string workoutName, int workoutTypeId);
        Task InsertWorkoutAsync(string workoutName, int workoutTypeId, string description);
        Task DeleteWorkoutAsync(int workoutId);
        Task UpdateWorkoutAsync(WorkoutModel workout);
        Task<IList<WorkoutModel>> GetAllWorkoutsAsync();
    }
}
