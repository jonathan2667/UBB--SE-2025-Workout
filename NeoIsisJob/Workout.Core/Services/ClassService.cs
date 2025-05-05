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
        private readonly IClassRepository _classRepo;
        private readonly IUserClassService _userClassService;

        public ClassService(IClassRepository classRepository, IUserClassService userClassService)
        {
            _classRepo        = classRepository ?? throw new ArgumentNullException(nameof(classRepository));
            _userClassService = userClassService ?? throw new ArgumentNullException(nameof(userClassService));
        }

        public async Task<List<ClassModel>> GetAllClassesAsync()
        {
            return await _classRepo
                .GetAllClassModelAsync();
                //.ConfigureAwait(false);
        }

        public async Task<ClassModel> GetClassByIdAsync(int classId)
        {
            //if (classId <= 0)
            //    throw new ArgumentOutOfRangeException(nameof(classId), "classId must be positive.");

            return await _classRepo
                .GetClassModelByIdAsync(classId);
                //.ConfigureAwait(false);
        }

        public async Task AddClassAsync(ClassModel classModel)
        {
            //if (classModel == null)
            //    throw new ArgumentNullException(nameof(classModel));
            //if (string.IsNullOrWhiteSpace(classModel.Name))
            //    throw new ArgumentException("Name is required.", nameof(classModel));

            await _classRepo
                .AddClassModelAsync(classModel);
                //.ConfigureAwait(false);
        }

        public async Task DeleteClassAsync(int classId)
        {
            //if (classId <= 0)
            //    throw new ArgumentOutOfRangeException(nameof(classId), "classId must be positive.");

            await _classRepo
                .DeleteClassModelAsync(classId);
                //.ConfigureAwait(false);
        }

        public async Task<string> ConfirmRegistrationAsync(int userId, int classId, DateTime date)
        {
            //if (userId  <= 0)
            //    throw new ArgumentOutOfRangeException(nameof(userId), "userId must be positive.");
            //if (classId <= 0)
            //    throw new ArgumentOutOfRangeException(nameof(classId), "classId must be positive.");
            //if (date.Date < DateTime.Today)
            //    return "Please choose a valid date (today or future).";

            try
            {
                var userClass = new UserClassModel
                {
                    UID         = userId,
                    CID        = classId,
                    Date = date.Date
                };

                await _userClassService
                    .AddUserClassAsync(userClass);
                //.ConfigureAwait(false);

                var cls = await GetClassByIdAsync(classId);
                              //.ConfigureAwait(false);

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
