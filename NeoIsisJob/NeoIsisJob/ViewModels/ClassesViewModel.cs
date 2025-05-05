using System;
using System.Threading.Tasks;
using Workout.Core.IServices;


// using NeoIsisJob.Services.Interfaces;

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