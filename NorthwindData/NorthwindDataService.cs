using Dapper;
using NorthwindData.Models;

namespace NorthwindData
{
    public interface INorthwindDataService
    {
        #region Categories
        Task<Category?>         GetCategoryAsync(int id);
        Task<List<Category>>    GetCategoriesAsync();
        Task                    AddCategoryAsync(Category category);
        Task<bool>              UpdateCategoryAsync(Category category);
        Task<bool>              DeleteCategoryAsync(int id);
        #endregion
    }

    public class NorthwindDataService : INorthwindDataService
    {
        private readonly INorthwindConnection _ConnectionManager;

        public NorthwindDataService(INorthwindConnection connection)
            => _ConnectionManager = connection;

        #region Categories
        public async Task<Category?> GetCategoryAsync(int id)
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();

                var category = await connection.QuerySingleOrDefaultAsync<Category>(
                    "SELECT * FROM Categories WHERE CategoryID = @id",
                    new { id }
                    );

                return category;
            }
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();

                var categories = await connection.QueryAsync<Category>("SELECT * FROM Categories");

                return categories.ToList();
            }
        }

        public async Task AddCategoryAsync(Category category)
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();

                var sql = @"INSERT INTO Categories(CategoryName, Description, Picture)
                            OUTPUT INSERTED.CategoryID
                            VALUES (@CategoryName, @Description, @Picture)";

                var newID = await connection.QuerySingleAsync<int>(
                    sql,
                    new
                    {
                        category.CategoryName,
                        category.Description,
                        category.Picture
                    });

                category.CategoryID = newID;
            }
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();

                var sql = @"UPDATE Categories
                            SET CategoryName = @CategoryName,
                                Description  = @Description,
                                Picture      = @Picture
                            WHERE CategoryID = @CategoryID";

                var rowsAffected = await connection.ExecuteAsync(sql,
                    new
                    {
                        category.CategoryName,
                        category.Description,
                        category.Picture,
                        category.CategoryID
                    });

                return rowsAffected == 1;
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();
                var rowsAffected = await connection.ExecuteAsync("DELETE FROM Categories WHERE CategoryID = @id", new { id });

                return rowsAffected == 1;
            }
        }
        #endregion
    }
}
