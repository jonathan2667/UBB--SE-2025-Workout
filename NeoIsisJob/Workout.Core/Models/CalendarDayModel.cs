using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Workout.Core.Models
{
    public class CalendarDay
    {
        public int DayNumber { get; set; }
        public bool IsCurrentDay { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsNotCurrentDay => !IsCurrentDay;
        public int GridRow { get; set; }
        public int GridColumn { get; set; }
        public bool HasClass { get; set; }
        public bool HasWorkout { get; set; }
        public bool IsWorkoutCompleted { get; set; }
        public DateTime Date { get; set; }
        public ICommand ClickCommand { get; set; }
        public ICommand RemoveWorkoutCommand { get; set; }

        public ICommand ChangeWorkoutCommand { get; set; }
    }
}
