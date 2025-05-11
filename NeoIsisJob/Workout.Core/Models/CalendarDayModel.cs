using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Workout.Core.Models
{
    [Table("CalendarDays")]
    public class CalendarDayModel
    {
        [Key]
        public int Id { get; set; }
        public int DayNumber { get; set; }
        public bool IsCurrentDay { get; set; }
        public bool IsEnabled { get; set; } = true;
        [NotMapped]
        public bool IsNotCurrentDay => !IsCurrentDay;
        public int GridRow { get; set; }
        public int GridColumn { get; set; }
        public bool HasClass { get; set; }
        public bool HasWorkout { get; set; }
        public bool IsWorkoutCompleted { get; set; }
        public DateTime Date { get; set; }
        [NotMapped]
        public ICommand ClickCommand { get; set; }
        [NotMapped]
        public ICommand RemoveWorkoutCommand { get; set; }
        [NotMapped]
        public ICommand ChangeWorkoutCommand { get; set; }
    }
}
