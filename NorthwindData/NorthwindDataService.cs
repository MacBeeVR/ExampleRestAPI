using Dapper;
using NorthwindData.Models;

namespace NorthwindData
{
    public interface INorthwindDataService
    {
        #region Categories
        Task<Category?>         GetCategoryAsync(int id);
        Task<List<Category>>    GetCategoriesAsync();
        Task<bool>              AddCategoryAsync(Category category);
        Task<bool>              UpdateCategoryAsync(Category category);
        Task<bool>              DeleteCategoryAsync(int id);
        #endregion

        #region Customers
        Task<Customer?>         GetCustomerAsync(string id, bool includeRelations = false);
        Task<List<Customer>>    GetCustomersAsync(bool includeRelations = false);
        Task<bool>              AddCustomerAsync(Customer customer);
        Task<bool>              UpdateCustomerAsync(Customer customer);
        Task<bool>              DeleteCustomerAsync(string id);

        Task<bool>              AddCustomerDemographicForCustomerAsync(string customerID, string customerTypeID);
        Task<bool>              AddCustomerDemographicForCustomerAsync(string customerID, CustomerDemographics demographics);
        #endregion

        #region Customer Demographics
        Task<CustomerDemographics?>         GetCustomerDemographicsAsync(string id);
        Task<List<CustomerDemographics>>    GetCustomerDemographicsAsync();
        Task<bool>                          AddCustomerDemographicsAsync(CustomerDemographics demographics);
        Task<bool>                          UpdateCustomerDemographicsAsync(CustomerDemographics demographics);
        Task<bool>                          DeleteCustomerDemographicsAsync(string id);
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

        public async Task<bool> AddCategoryAsync(Category category)
        {
            if (await GetCategoryAsync(category.CategoryID) is not null)
                return false;

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

                return true;
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
        public async Task<Customer?> GetCustomerAsync(string id, bool includeRelations = false)
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();
                var customer = await connection.QuerySingleOrDefaultAsync<Customer>("SELECT * FROM Customers WHERE CustomerID = @id", new { id });

                return customer;
            }
        }

        public async Task<List<Customer>> GetCustomersAsync(bool includeRelations = false)
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();

                var customers = await connection.QueryAsync<Customer>("SELECT * FROM Customers");

                return customers.ToList();
            }
        }

        public async Task<bool> AddCustomerAsync(Customer customer)
        {
            if (await GetCustomerAsync(customer.CustomerID) is not null)
                return false;

            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();

                var sql = @"INSERT INTO Customers(CustomerID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax)
                            VALUES(@CustomerID, @CompanyName, @ContactName, @ContactTitle, @Address, @City, @Region, @PostalCode, @Country, @Phone, @Fax)";

                return await connection.ExecuteAsync(sql, customer) == 1;
            }
        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();

                var sql = @"UPDATE Customers
                            SET CompanyName     = @CompanyName,
                                ContactName     = @ContactName,
                                ContactTitle    = @ContactTitle,
                                Address         = @Address,
                                City            = @City,
                                Region          = @Region,
                                PostalCode      = @PostalCode,
                                Country         = @Country,
                                Phone           = @Phone,
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
                var rowsAffected = await connection.ExecuteAsync("DELETE FROM Customers WHERE CustomerID = @id", new { id });

                return rowsAffected == 1;
            }
        }

        public async Task<bool> AddCustomerDemographicForCustomerAsync(string customerID, string customerTypeID)
        {
            var demographicExists = (await GetCustomerDemographicsForCustomerAsync(customerID))
                .Where(demo => demo.CustomerTypeID == customerTypeID)
                .FirstOrDefault() is not null;


            if (demographicExists)
                return false;

            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();
                var sql = @"INSERT INTO CustomerCustomerDemo(CustomerID, CustomerTypeID)
                            VALUES(@customerID, @customerTypeID)";

                return await connection.ExecuteAsync(sql, new { customerID, customerTypeID }) == 1;
            }
        }

        public async Task<bool> AddCustomerDemographicForCustomerAsync(string customerID, CustomerDemographics demographics)
        {
            // Try Adding Non-Existent CustomerDemographics Record
            if (await GetCustomerDemographicsAsync(demographics.CustomerTypeID) is null)
                if (!await AddCustomerDemographicsAsync(demographics))
                    return false;
            
            
            return await AddCustomerDemographicForCustomerAsync(customerID, demographics.CustomerTypeID);
        }
        #endregion

        #region Customer Demographics
        public async Task<CustomerDemographics?> GetCustomerDemographicsAsync(string id)
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();
                return await connection.QuerySingleOrDefaultAsync<CustomerDemographics>("SELECT * FROM CustomerDemographics WHERE CustomerTypeID = @id", new { id }); ;
            }
        }

        public async Task<List<CustomerDemographics>> GetCustomerDemographicsAsync()
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();
                return (await connection.QueryAsync<CustomerDemographics>("SELECT * FROM CustomerDemographics")).ToList();
            }
        }

        public async Task<List<CustomerDemographics>> GetCustomerDemographicsForCustomerAsync(string customerID)
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();
                var demographicIDs = await connection.QueryAsync<string>(               "SELECT CustomerTypeID FROM CustomeCustomerDemo WHERE CustomerID = @customerID" , new { customerID      });
                var demographics   = await connection.QueryAsync<CustomerDemographics>( "SELECT * FROM CustomerDemographics WHERE CustomerTypeID IN @demographicIDs"    , new { demographicIDs  });

                return demographics.ToList();
            }
        }

        public async Task<bool> AddCustomerDemographicsAsync(CustomerDemographics demographics)
        {
            if (await GetCustomerDemographicsAsync(demographics.CustomerTypeID) is not null)
                return false;

            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();
                var sql = @"INSERT INTO CustomerDemographics(CustomerTypeID, CustomerDesc)
                            VALUES(@CustomerTypeID, @CustomerDesc)";

                return await connection.ExecuteAsync(sql, demographics) == 1;
            }
        }

        public async Task<bool> UpdateCustomerDemographicsAsync(CustomerDemographics demographics)
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();
                var sql = @"UPDATE CustomerDemographics
                            SET CustomerDesc = @CustomerDesc
                            WHERE CustomerTypeID = @CustomerTypeID";

                return await connection.ExecuteAsync(sql, demographics) == 1;
            }
        }

        public async Task<bool> DeleteCustomerDemographicsAsync(string id)
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();
                return await connection.ExecuteAsync("DELETE FROM CustomerDemographics WHERE CustomerTypeID = @id", new { id }) == 1;
            }
        }
        #endregion
    }
}
