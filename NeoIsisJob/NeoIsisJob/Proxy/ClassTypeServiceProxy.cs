using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Workout.Core.Models;

namespace NeoIsisJob.Proxy
{
    public class ClassTypeServiceProxy : BaseServiceProxy
    {
        private const string EndpointName = "classtype";

        public ClassTypeServiceProxy(IConfiguration configuration = null) 
            : base(configuration)
        {
        }

        public async Task<List<ClassTypeModel>> GetAllClassTypesAsync()
        {
            try
            {
                var results = await GetAsync<List<ClassTypeModel>>($"{EndpointName}");
                return results ?? new List<ClassTypeModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all class types: {ex.Message}");
                return new List<ClassTypeModel>();
            }
        }

        public async Task<ClassTypeModel> GetClassTypeByIdAsync(int classTypeId)
        {
            try
            {
                var result = await GetAsync<ClassTypeModel>($"{EndpointName}/{classTypeId}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching class type: {ex.Message}");
                throw;
            }
        }

        public async Task AddClassTypeAsync(ClassTypeModel classTypeModel)
        {
            try
            {
                await PostAsync($"{EndpointName}", classTypeModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding class type: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteClassTypeAsync(int classTypeId)
        {
            try
            {
                await DeleteAsync($"{EndpointName}/{classTypeId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting class type: {ex.Message}");
                throw;
            }
        }
    }
} 