namespace NeoIsisJob.Proxy
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Workout.Core.IServices;
    using Workout.Core.Models;

    public class OrderServiceProxy : BaseServiceProxy, IService<OrderModel>
    {
        private const string BaseRoute = "order";

        public OrderServiceProxy(IConfiguration configuration = null)
            : base(configuration)
        {
        }

        public async Task<OrderModel> CreateAsync(OrderModel entity)
        {
            try
            {
                OrderModel result = await PostAsync<OrderModel>($"{BaseRoute}", entity);
                return result;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error creating order: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await DeleteAsync($"{BaseRoute}/{id}");
                return true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error deleting order: {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<OrderModel>> GetAllAsync()
        {
            try
            {
                return await GetAsync<IEnumerable<OrderModel>>($"{BaseRoute}");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error fetching all orders: {ex.Message}");
                return await Task.FromResult<IEnumerable<OrderModel>>(new List<OrderModel>());
            }
        }

        public async Task<OrderModel> GetByIdAsync(int id)
        {
            try
            {
                return await GetAsync<OrderModel>($"{BaseRoute}/{id}");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error fetching order by ID: {ex.Message}");
                return await Task.FromResult<OrderModel>(null);
            }
        }

        public async Task<OrderModel> UpdateAsync(OrderModel entity)
        {
            try
            {
                return await PutAsync<OrderModel>($"{BaseRoute}/{entity.ID}", entity);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error updating order: {ex.Message}");
                return await Task.FromResult<OrderModel>(null);
            }
        }

        public async Task CreateOrderFromCartAsync()
        {
            /*
            try
            {
                var result = await PostAsync<string>($"{BaseRoute}/from-cart", null);
                return result;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error creating order from cart: {ex.Message}");
                return string.Empty;
            }*/
        }
    }
