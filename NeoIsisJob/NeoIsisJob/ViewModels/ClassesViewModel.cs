using System;
using System.Threading.Tasks;

// using NeoIsisJob.Services.Interfaces;
using Workout.Core.Services.Interfaces;

namespace NeoIsisJob.ViewModels
{
    public class ClassesViewModel
    {
        private readonly IClassService classService;

        public ClassesViewModel(IClassService classService)
        {
            this.classService = classService;
        }

        public async Task<string> ConfirmRegistration(int userId, int classId, DateTime date)
        {
            return await classService.ConfirmRegistrationAsync(userId, classId, date);
        }
    }
}