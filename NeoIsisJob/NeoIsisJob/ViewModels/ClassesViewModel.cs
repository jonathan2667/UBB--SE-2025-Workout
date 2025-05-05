using NeoIsisJob.Helpers;
using NeoIsisJob.Proxy;
using Refit;
using System;
using System.Net.Http;
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

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(ServerHelpers.SERVER_BASE_URL)
            };

            this.classService = RestService.For<IClassServiceProxy>(httpClient);
        }

        public async Task<string> ConfirmRegistration(int userId, int classId, DateTime date)
        {
            return await classService.ConfirmRegistrationAsync(userId, classId, date);
        }
    }
}