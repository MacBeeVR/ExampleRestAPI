namespace ExampleRestAPI.Test.APITests
{
    [TestClass]
    public class CustomerTests : APITestBase
    {
        [TestMethod]
        public async Task TestAdd()
        {
            #region Test Add Existing Record
            var selectedRecord  = await GetRandomRecordFromURLAsync<Customer>("/api/Customers");
            var failedAddResp   = await APIClient.PostAsJsonAsync("/api/Customers", selectedRecord);

            await AssertResponseIsExpectedAsync(failedAddResp, HttpStatusCode.BadRequest, ErrorMessageStrings.RecordAddError);
            #endregion

            #region Test Add New Record
            var newRecord = new Customer
            {
                CustomerID      = GetRandomString(5),
                CompanyName     = "Test Company",
                ContactName     = "Test Contact",
                ContactTitle    = "Test Title",
            };

            var successAddResp = await APIClient.PostAsJsonAsync("/api/Customers", newRecord);
            await AssertResponseIsExpectedAsync(successAddResp, HttpStatusCode.Created);
            #endregion
        }

        [TestMethod]
        public async Task TestUpdate()
        {
            #region Test Update NonExistent Record
            var nonExistentRecord = new Customer
            {
                CustomerID      = GetRandomString(5),
                ContactName     = "Test Contact",
                CompanyName     = "Test Company",
                ContactTitle    = "Test Title",
            };

            var failedUpdateResp = await APIClient.PutAsJsonAsync("/api/Customers", nonExistentRecord);
            await AssertResponseIsExpectedAsync(failedUpdateResp, HttpStatusCode.BadRequest, ErrorMessageStrings.RecordUpdateError);
            #endregion

            #region Test Update Existing Record
            var selectedRecord      = await GetRandomRecordFromURLAsync<Customer>("/api/Customers");
            var successUpdateResp   = await APIClient.PutAsJsonAsync("/api/Customers", selectedRecord);

            await AssertResponseIsExpectedAsync(successUpdateResp, HttpStatusCode.OK);
            #endregion
        }

        [TestMethod]
        public async Task TestDelete()
        {
            #region Test Delete NonExistent Record
            var nonExistentID       = GetRandomString(5);
            var failedDeleteResp    = await APIClient.DeleteAsync($"/api/Customers/{nonExistentID}");

            await AssertResponseIsExpectedAsync(failedDeleteResp, HttpStatusCode.BadRequest, ErrorMessageStrings.RecordDeleteError);
            #endregion

            #region Test Delete Existing Record
            var existingRecord      = await GetRandomRecordFromURLAsync<Customer>("/api/Customers");
            var successDeleteResp   = await APIClient.DeleteAsync($"/api/Customers/{existingRecord.CustomerID}");
            #endregion
        }
    }
}