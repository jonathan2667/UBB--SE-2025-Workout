using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;
using Workout.Core.IServices;
using Workout.Core.IRepositories;
using Workout.Core.Repositories;

namespace Workout.Core.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository classRepo;
        private readonly IUserClassService userClassService;

        public ClassService(IClassRepository classRepository, IUserClassService userClassService)
        {
            classRepo = classRepository ?? throw new ArgumentNullException(nameof(classRepository));
            this.userClassService = userClassService ?? throw new ArgumentNullException(nameof(userClassService));
        }

        public async Task<List<ClassModel>> GetAllClassesAsync()
        {
            return await classRepo
                .GetAllClassModelAsync();
        }
        public async Task<ClassModel> GetClassByIdAsync(int classId)
        {
            return await classRepo
                .GetClassModelByIdAsync(classId);
                // .ConfigureAwait(false);
        }

        public async Task AddClassAsync(ClassModel classModel)
        {
            await classRepo
                .AddClassModelAsync(classModel);
        }

        public async Task DeleteClassAsync(int classId)
        {
            await classRepo
                .DeleteClassModelAsync(classId);
                // .ConfigureAwait(false);
        }

        public async Task<string> ConfirmRegistrationAsync(int userId, int classId, DateTime date)
        {
            try
            {
                var userClass = new UserClassModel
                {
                    UID = userId,
                    CID = classId,
                    Date = date.Date
                };

                await userClassService
                    .AddUserClassAsync(userClass);

                var cls = await GetClassByIdAsync(classId);

                Debug.WriteLine($"Successfully registered for class {cls.Name}");
                return string.Empty;
            }
            catch (Exception ex)
            {
                var msg = $"Registration failed: {ex.Message}";
                Debug.WriteLine(msg);
                return msg;
            }
        }
    }
}
