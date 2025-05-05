using NeoIsisJob.Helpers;
using NeoIsisJob.Proxy;
using System;
using System.Net.Http;
using System.Threading.Tasks;
// using NeoIsisJob.Services.Interfaces;
namespace NeoIsisJob.ViewModels
{
    public class ClassesViewModel
    {
        private readonly ClassServiceProxy classService;

        public ClassesViewModel()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(ServerHelpers.SERVER_BASE_URL)
            };

            this.classService = new ClassServiceProxy();
        }

        public async Task<string> ConfirmRegistration(int userId, int classId, DateTime date)
        {
            return await classService.ConfirmRegistrationAsync(userId, classId, date);
        }
    }
}