//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using NeoIsisJob.Models;
//using NeoIsisJob.Repositories;
//using NeoIsisJob.Repositories.Interfaces;

//namespace Tests.Repo.Mocks
//{
//    class WorkoutMock : IWorkoutRepository
//    {
//        private readonly List<WorkoutModel> workouts;

//        public WorkoutMock()
//        {
//            workouts = new List<WorkoutModel>
//            {
//                new WorkoutModel(1, "Workout A", 1),
//                new WorkoutModel(2, "Workout B", 2),
//                new WorkoutModel(3, "Workout C", 3)
//            };
//        }

//        public void DeleteWorkout(int workoutId)
//        {
//            var workout = workouts.FirstOrDefault(w => w.Id == workoutId);
//            if (workout != null)
//            {
//                workouts.Remove(workout);
//            }
//            else
//            {
//                throw new KeyNotFoundException("Workout not found.");
//            }
//        }

//        public IList<WorkoutModel> GetAllWorkouts()
//        {
//            return workouts;
//        }

//        public WorkoutModel GetWorkoutById(int workoutId)
//        {
//            return workouts.FirstOrDefault(w => w.Id == workoutId);
//        }

//        public WorkoutModel GetWorkoutByName(string workoutName)
//        {
//            return workouts.FirstOrDefault(w => w.Name.Equals(workoutName, StringComparison.OrdinalIgnoreCase));
//        }

//        public void InsertWorkout(string workoutName, int workoutTypeId)
//        {
//            if (workouts.Any(w => w.Name.Equals(workoutName, StringComparison.OrdinalIgnoreCase)))
//            {
//                throw new InvalidOperationException("Workout already exists.");
//            }
//            var newWorkout = new WorkoutModel
//            {
//                Id = workouts.Max(w => w.Id) + 1,
//                Name = workoutName,
//                WorkoutTypeId = workoutTypeId
//            };
//            workouts.Add(newWorkout);
//        }

//        public void UpdateWorkout(WorkoutModel workout)
//        {



//            var existingWorkout = workouts.FirstOrDefault(w => w.Id == workout.Id);

//            var duplicateWorkout = workouts.FirstOrDefault(w => w.Name.Equals(workout.Name, StringComparison.OrdinalIgnoreCase) && w.Id != workout.Id);

//                if (duplicateWorkout != null)
//                {
//                    throw new InvalidOperationException("Workout with the same name already exists.");
//                }
            

//            if (existingWorkout != null)
//            {
//                existingWorkout.Name = workout.Name;
//                existingWorkout.WorkoutTypeId = workout.WorkoutTypeId;
//            }
//            else
//            {
//                throw new KeyNotFoundException("Workout not found.");
//            }
//        }
//    }
//}
