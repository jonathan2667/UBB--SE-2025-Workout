using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.Repositories.Interfaces
{
    internal class ICalendarRepository
    {
        List<CalendarDay> GetCalendarDaysForMonth(int userId, DateTime month);
        UserWorkoutModel GetUserWorkout(int userId, DateTime date);
        List<WorkoutModel> GetWorkouts();
        string GetUserClass(int userId, DateTime date);
    }
}
