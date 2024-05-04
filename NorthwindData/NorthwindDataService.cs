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

        #region Customers
        Task<Customer?>         GetCustomerAsync(string id);
        Task<List<Customer>>    GetCustomersAsync();
        Task                    AddCustomerAsync(Customer customer);
        Task<bool>              UpdateCustomerAsync(Customer customer);
        Task<bool>              DeleteCustomerAsync(string id);
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
                    new { id });

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

                var parameters = new
                {
                    category.CategoryName,
                    category.Description,
                    category.Picture
                };

                var newID = await connection.QuerySingleAsync<int>(sql, parameters);

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

                var parameters = new
                {
                    category.CategoryName,
                    category.Description,
                    category.Picture,
                    category.CategoryID
                };

                var rowsAffected = await connection.ExecuteAsync(sql, parameters);

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

        #region Customers
        public async Task<Customer?> GetCustomerAsync(string id)
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();

                var customer = await connection.QuerySingleOrDefaultAsync<Customer>(
                    "SELECT * FROM Customers WHERE CustomerID = @id",
                    new { id });

                return customer;
            }
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();

                var customers = await connection.QueryAsync<Customer>("SELECT * FROM Customers");

                return customers.ToList();
            }
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();

                var sql = @"INSERT INTO Customers(CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax)
                            OUTPUT INSERTED.CustomerID
                            VALUES(@CompanyName, @ContactName, @ContactTitle, @Address, @City, @Region, @PostalCode, @Country, @Phone, @Fax)";

                var parameters = new
                {
                    customer.CompanyName,
                    customer.ContactName, 
                    customer.ContactTitle,
                    customer.Address,
                    customer.City,
                    customer.Region,
                    customer.PostalCode,
                    customer.Country,
                    customer.Phone,
                    customer.Fax
                };

                var newID = await connection.QuerySingleAsync<string>(sql, parameters);

                customer.CustomerID = newID;
            }
        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();

                var sql = @"UPDATE Customers
                            SET CompanyName     = @CompanyName
                                ContactName     = @ContactName
                                ContactTitle    = @ContactTitle
                                Address         = @Address
                                City            = @City
                                Region          = @Region
                                PostalCode      = @PostalCode
                                Country         = @Country
                                Phone           = @Phone
                                Fax             = @Fax
                            WHERE CustomerID = @CustomerID";

                var rowsAffected = await connection.ExecuteAsync(sql, customer);

                return rowsAffected == 1;
            }
        }

        public async Task<bool> DeleteCustomerAsync(string id)
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();
                var rowsAffected = await connection.ExecuteAsync("DELETE FROM Customers WHERE CompanyID = @id", new { id });

                return rowsAffected == 1;
            }
        }
        #endregion
    }
}
