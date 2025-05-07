using Workout.Core.Data;
using Workout.Core.IRepositories;
using Workout.Core.Models;

namespace Workout.Core.Repositories
{
    class ProductRepository : IRepository<ProductModel>
    {
        private readonly WorkoutDbContext context;

        public ProductRepository(WorkoutDbContext context)
        {
            this.context = context;
        }

        public async Task<ProductModel> CreateAsync(ProductModel entity)
        {
            /*string query = @"
                SELECT
                    Product.ID AS ProductID,
                    Product.Name AS ProductName,
                    Product.Price,
                    Product.Stock,
                    Product.Size,
                    Product.Color,
                    Product.Description,
                    Product.PhotoURL,
                    Category.ID AS CategoryID,
                    Category.Name AS CategoryName
                FROM Product
                JOIN Category
                    ON Product.CategoryID = Category.ID;
            ";
            DataTable result = await this.dbService.ExecuteSelectAsync(query, []);

            List<Product> products = [];
            foreach (DataRow row in result.Rows)
            {
                products.Add(MapRowToProduct(row));
            }

            return products;*/
            return await context.Products
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProductModel?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductModel> UpdateAsync(ProductModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
