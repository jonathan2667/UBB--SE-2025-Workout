using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;
using Workout.Core.Repositories;
using Workout.Core.IServices;
using Workout.Core.IRepositories;

namespace Workout.Core.Services
{
    public class WorkoutTypeService : IWorkoutTypeService
    {
        private readonly IWorkoutTypeRepository workoutTypeRepository;

        public WorkoutTypeService(IWorkoutTypeRepository workoutTypeRepository = null)
        {
            this.workoutTypeRepository = workoutTypeRepository
                ?? throw new ArgumentNullException(nameof(workoutTypeRepository));
        }

        public async Task InsertWorkoutTypeAsync(string workoutTypeName)
        {
            if (string.IsNullOrWhiteSpace(workoutTypeName))
            {
                throw new ArgumentException("Workout type name cannot be empty or null.", nameof(workoutTypeName));
            }

            try
            {
                await workoutTypeRepository
                      .InsertWorkoutTypeAsync(workoutTypeName);
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                throw new Exception("A workout type with this name already exists.");
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while inserting workout type.", ex);
            }
        }

        public async Task DeleteWorkoutTypeAsync(int workoutTypeId)
        {
            await workoutTypeRepository
                  .DeleteWorkoutTypeAsync(workoutTypeId);
        }

        public async Task<WorkoutTypeModel> GetWorkoutTypeByIdAsync(int workoutTypeId)
        {
            return await workoutTypeRepository
                         .GetWorkoutTypeByIdAsync(workoutTypeId);
        }

        public async Task<IList<WorkoutTypeModel>> GetAllWorkoutTypesAsync()
        {
            return await workoutTypeRepository
                         .GetAllWorkoutTypesAsync();
        }
    }
}
