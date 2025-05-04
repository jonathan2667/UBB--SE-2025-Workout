using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace Workout.Core.Services.Interfaces
{
    public interface IClassService
    {
        Task<List<ClassModel>> GetAllClassesAsync();
        Task<ClassModel> GetClassByIdAsync(int classId);
        Task AddClassAsync(ClassModel classModel);
        Task DeleteClassAsync(int classId);

        /// <summary>
        /// Attempts to register a user for a class on the given date.
        /// Returns empty string on success, or an error message.
        /// </summary>
        Task<string> ConfirmRegistrationAsync(int userId, int classId, DateTime date);
    }
}
