using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Workout.Core.Models;

namespace NeoIsisJob.Proxy
{
    public class ClassServiceProxy : BaseServiceProxy
    {
        private const string EndpointName = "class";

        public ClassServiceProxy(IConfiguration configuration = null)
            : base(configuration)
        {
        }

        public async Task<List<ClassModel>> GetAllClassesAsync()
        {
            try
            {
                var results = await GetAsync<List<ClassModel>>($"{EndpointName}");
                return results ?? new List<ClassModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all classes: {ex.Message}");
                return new List<ClassModel>();
            }
        }

        public async Task<ClassModel> GetClassByIdAsync(int classId)
        {
            try
            {
                var result = await GetAsync<ClassModel>($"{EndpointName}/{classId}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching class: {ex.Message}");
                throw;
            }
        }

        public async Task AddClassAsync(ClassModel classModel)
        {
            try
            {
                await PostAsync($"{EndpointName}", classModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding class: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteClassAsync(int classId)
        {
            try
            {
                await DeleteAsync($"{EndpointName}/{classId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting class: {ex.Message}");
                throw;
            }
        }

        public async Task<string> ConfirmRegistrationAsync(int userId, int classId, DateTime date)
        {
            try
            {
                var request = new ConfirmRegistrationRequest
                {
                    UserId = userId,
                    ClassId = classId,
                    Date = date
                };

                var result = await PostAsync<string>($"{EndpointName}/confirm", request);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error confirming registration: {ex.Message}");
                throw;
            }
        }
    }

    public class ConfirmRegistrationRequest
    {
        public int UserId { get; set; }
        public int ClassId { get; set; }
        public DateTime Date { get; set; }
    }
}