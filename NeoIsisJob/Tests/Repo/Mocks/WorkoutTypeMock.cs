//using NeoIsisJob.Models;
//using NeoIsisJob.Repositories.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Tests.Repo.Mocks
//{
//    public class WorkoutTypeMock : IWorkoutTypeRepository
//    {
//        private readonly List<WorkoutTypeModel> workoutTypes;

//        public WorkoutTypeMock()
//        {
//            workoutTypes = new List<WorkoutTypeModel>
//            {
//                new WorkoutTypeModel(1, "Cardio"),
//                new WorkoutTypeModel(2, "Strength"),
//                new WorkoutTypeModel(3, "Flexibility")
//            };
//        }

//        public void DeleteWorkoutType(int workoutTypeId)
//        {
//            var workoutType = workoutTypes.FirstOrDefault(wt => wt.Id == workoutTypeId);
//            if (workoutType != null)
//            {
//                workoutTypes.Remove(workoutType);
//            }
//            else
//            {
//                throw new KeyNotFoundException("Workout type not found.");
//            }
//        }

//        public IList<WorkoutTypeModel> GetAllWorkoutTypes()
//        {
//            return workoutTypes;
//        }

//        public WorkoutTypeModel GetWorkoutTypeById(int workoutTypeId)
//        {
//            return workoutTypes.FirstOrDefault(wt => wt.Id == workoutTypeId);
//        }

//        public void InsertWorkoutType(string workoutTypeName)
//        {

//            var duplicateWorkout = workoutTypes.FirstOrDefault(wt => wt.Name.Equals(workoutTypeName, StringComparison.OrdinalIgnoreCase));

//            if(duplicateWorkout != null)
//            {
//                throw new Exception();
//            }

//            var newWorkoutType = new WorkoutTypeModel
//            {
//                Id = workoutTypes.Max(wt => wt.Id) + 1,
//                Name = workoutTypeName
//            };
//            workoutTypes.Add(newWorkoutType);
//        }
//    }
//}
