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
        Task<Customer?>         GetCustomerAsync(string id);
        Task<List<Customer>>    GetCustomersAsync();
        Task<bool>              AddCustomerAsync(Customer customer);
        Task<bool>              UpdateCustomerAsync(Customer customer);
        Task<bool>              DeleteCustomerAsync(string id);

        Task<bool>              AddDemographicToCustomer(string customerID, string customerTypeID);
        Task<bool>              DeleteDemographicFromCustomer(string customerID, string customerTypeID);
        #endregion

        #region Customer Demographics
        Task<CustomerDemographic?>         GetCustomerDemographicsAsync(string id);
        Task<List<CustomerDemographic>>    GetCustomerDemographicsAsync();
        Task<bool>                          AddCustomerDemographicsAsync(CustomerDemographic demographics);
        Task<bool>                          UpdateCustomerDemographicsAsync(CustomerDemographic demographics);
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
        /// <summary>
        /// Returns true if Customer has the specified Demographic Added
        /// </summary>
        /// <param name="customerID">ID of the Customer to Check</param>
        /// <param name="customerTypeID">ID of the Demographic to Find</param>
        /// <returns>True if Demographic is Added to Customer, False Otherwise</returns>
        private async Task<bool> CustomerHasDemographic(string customerID, string customerTypeID)
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();

                var parameters  = new { customerID, customerTypeID };
                var sql         = @"SELECT CustomerTypeID FROM CustomerCustomerDemo WHERE CustomerID = @customerID AND CustomerTypeID = @customerTypeID";
                var result      = await connection.QuerySingleOrDefaultAsync<string>(sql, parameters);

                return !string.IsNullOrWhiteSpace(result);
            }
        }

        /// <summary>
        /// Gets the Customer Record with the Specified ID
        /// </summary>
        /// <param name="id">ID of the Customer to Retrieve</param>
        /// <returns>Customer Record if Exists, Null if Not</returns>
        public async Task<Customer?> GetCustomerAsync(string id)
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();

                var query = @"SELECT C.*, CD.* FROM Customers C
                              LEFT JOIN CustomerCustomerDemo CCD ON C.CustomerID       = CCD.CustomerID
                              LEFT JOIN CustomerDemographics CD  ON CCD.CustomerTypeID = CD.CustomerTypeID
                              WHERE C.CustomerID = @id";

                var customer = (Customer?)null;
                (await connection.QueryAsync<Customer, CustomerDemographic, Customer>(
                    query,
                    (c, cd) =>
                    {
                        if (customer is null)
                            customer = c;

                        customer.Demographics.Add(cd);
                        return c;
                    },
                    new { id },
                    splitOn: "CustomerTypeID")).FirstOrDefault();

                return customer;
            }
        }

        /// <summary>
        /// Gets All Customer Records
        /// </summary>
        /// <returns>A List of All Customers</returns>
        public async Task<List<Customer>> GetCustomersAsync()
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();

                var query = @"SELECT C.*, CD.* FROM Customers C
                              LEFT JOIN CustomerCustomerDemo CCD ON C.CustomerID       = CCD.CustomerID
                              LEFT JOIN CustomerDemographics CD  ON CCD.CustomerTypeID = CD.CustomerTypeID";

                // Get Customers and Demographics and Map them to the Data Objects
                var lookup    = new Dictionary<string, Customer>();
                var customers = await connection.QueryAsync<Customer, CustomerDemographic, Customer>(
                    query,
                    (c, cd) =>
                    {
                        Customer customer;
                        if (!lookup.TryGetValue(c.CustomerID, out customer))
                        {
                            lookup.Add(c.CustomerID, c);
                            customer = c;
                        }

                        customer.Demographics.Add(cd);
                        return customer;
                    }, 
                    splitOn: "CustomerTypeID");

                return customers.ToList();
            }
        }

        /// <summary>
        /// Adds a New Customer to the Database
        /// </summary>
        /// <param name="customer">Customer Record Data to Add</param>
        /// <returns>True if Successful, False if record with same ID already exists or Add fails</returns>
        public async Task<bool> AddCustomerAsync(Customer customer)
        {
            // Return False if Customer with Given ID already Exists
            if (await GetCustomerAsync(customer.CustomerID) is not null)
                return false;

            // Insert the new Customer Record into the Database
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();

                var sql = @"INSERT INTO Customers(CustomerID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax)
                            VALUES(@CustomerID, @CompanyName, @ContactName, @ContactTitle, @Address, @City, @Region, @PostalCode, @Country, @Phone, @Fax)";

                return await connection.ExecuteAsync(sql, customer) == 1;
            }
        }

        /// <summary>
        /// Updates an Existing Customer Record with New Data
        /// </summary>
        /// <param name="customer">Updated Customer Data</param>
        /// <returns>True if the Customer was Successfully Updated, False if not exists or Update Fails</returns>
        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            // Return False if Customer Doesn't Exist
            if (await GetCustomerAsync(customer.CustomerID) is null)
                return false;

            // Update the Customer
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

        /// <summary>
        /// Deletes a Customer with the Given ID from the Database
        /// </summary>
        /// <param name="id">ID of the Customer to Delete</param>
        /// <returns>True if the Delete is Successful or record doesn't exist, False Otherwise</returns>
        public async Task<bool> DeleteCustomerAsync(string id)
        {
            // Return true if record doesn't exist
            if (await GetCustomerAsync(id) is null)
                return true;

            // Delete the Record
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();
                var parameters  = new { id };
                var sql         = @"DELETE FROM Customers WHERE CustomerID = @id";

                return await connection.ExecuteAsync(sql, parameters) == 1;
            }
        }

        /// <summary>
        /// Associates a CustomerDemographic Record with the Customer with the Given ID
        /// </summary>
        /// <param name="customerID">ID of the Customer to Add the Demographic to</param>
        /// <param name="customerTypeID">ID of the Demographic to Add the Customer to</param>
        /// <returns>True if Add Succeeds or Demographic is Already Added, False if Demographic Doesn't Exist or Add Fails</returns>
        public async Task<bool> AddDemographicToCustomer(string customerID, string customerTypeID)
        {
            // Return True if Customer Already has Demographic Added
            if (await CustomerHasDemographic(customerID, customerTypeID))
                return true;

            // Return False if Demographic Doesn't Exist
            if (!await DemographicExistsAsync(customerTypeID))
                return false;


            // Add Demographic to Customer
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();
                var parameters  = new { customerID, customerTypeID };
                var sql         = @"INSERT INTO CustomerCustomerDemo(CustomerID, CustomerTypeID)
                                    VALUES (@customerID, @customerTypeID)";

                return await connection.ExecuteAsync(sql, parameters) == 1;
            }
        }

        /// <summary>
        /// Removes Associated Demographic from Customer
        /// </summary>
        /// <param name="customerID">ID of the Customer to Remove Demographic From</param>
        /// <param name="customerTypeID">ID of the Demographic to Remove</param>
        /// <returns>True if Delete Succeeds or Customer Doesn't Have Demographic, False if Delete Fails</returns>
        public async Task<bool> DeleteDemographicFromCustomer(string customerID, string customerTypeID)
        {
            // Return True if Customer Doesn't Have Demographic
            if (!await CustomerHasDemographic(customerID, customerTypeID))
                return true;

            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();

                var parameters  = new { customerID, customerTypeID };
                var sql         = @"DELETE FROM CustomerCustomerDemo WHERE CustomerID = @customerID AND CustomerTypeID = @customerTypeID";

                return await connection.ExecuteAsync(sql, parameters) == 1;
            }
        }

        #endregion

        #region Customer Demographics
        private async Task<bool> DemographicExistsAsync(string customerTypeID)
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();

                var parameters  = new { customerTypeID };
                var sql         = @"SELECT CustomerTypeID FROM CustomerDemographics WHERE CustomerTypeID = @customerTypeID";
                var result      = await connection.QuerySingleOrDefaultAsync<string>(sql, parameters);

                return !string.IsNullOrWhiteSpace(result);
            }
        }

        public async Task<CustomerDemographic?> GetCustomerDemographicsAsync(string id)
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();
                return await connection.QuerySingleOrDefaultAsync<CustomerDemographic>("SELECT * FROM CustomerDemographics WHERE CustomerTypeID = @id", new { id }); ;
            }
        }

        public async Task<List<CustomerDemographic>> GetCustomerDemographicsAsync()
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();
                return (await connection.QueryAsync<CustomerDemographic>("SELECT * FROM CustomerDemographics")).ToList();
            }
        }

        public async Task<List<CustomerDemographic>> GetCustomerDemographicsForCustomerAsync(string customerID)
        {
            using (var connection = _ConnectionManager.GetConnection())
            {
                await connection.OpenAsync();
                var demographicIDs = await connection.QueryAsync<string>(               "SELECT CustomerTypeID FROM CustomeCustomerDemo WHERE CustomerID = @customerID" , new { customerID      });
                var demographics   = await connection.QueryAsync<CustomerDemographic>( "SELECT * FROM CustomerDemographics WHERE CustomerTypeID IN @demographicIDs"    , new { demographicIDs  });

                return demographics.ToList();
            }
        }

        public async Task<bool> AddCustomerDemographicsAsync(CustomerDemographic demographics)
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

        public async Task<bool> UpdateCustomerDemographicsAsync(CustomerDemographic demographics)
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
