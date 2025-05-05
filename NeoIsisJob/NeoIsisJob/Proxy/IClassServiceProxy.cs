using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using Workout.Core.Models;
using Workout.Core.Services.Interfaces;

namespace NeoIsisJob.Proxy
{
    public interface IClassServiceProxy : IClassService
    {
        [Get("/api/class")]
        Task<List<ClassModel>> GetAllClassesAsync();

        [Get("/api/class/{classId}")]
        Task<ClassModel> GetClassByIdAsync(int classId);

        [Post("/api/class")]
        Task AddClassAsync([Body] ClassModel classModel);

        [Delete("/api/class/{classId}")]
        Task DeleteClassAsync(int classId);

        [Post("/api/class/confirm")]
        Task<string> ConfirmRegistrationAsync([Body] ConfirmRegistrationRequest request);
    }

    public class ConfirmRegistrationRequest
    {
        public int UserId { get; set; }
        public int ClassId { get; set; }
        public DateTime Date { get; set; }
    }
}
