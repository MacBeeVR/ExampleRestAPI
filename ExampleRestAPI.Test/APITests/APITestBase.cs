using Microsoft.Extensions.Configuration;

namespace ExampleRestAPI.Test.APITests
{
    [TestClass]
    public abstract class APITestBase
    {
        protected readonly Random       Rand;
        protected readonly HttpClient   APIClient;

        public APITestBase()
        {
            Rand        = new Random();
            APIClient   = new TestExampleRestAPIFactory<Program>().CreateDefaultClient();
        }

        protected string GetRandomString(int maxLength)
        {
            var newStr          = new char[Rand.Next(maxLength + 1)];
            const string chars  = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            for (var i = 0; i < newStr.Length; i++)
                newStr[i] = chars[Rand.Next(chars.Length)];


            return new string(newStr);
        }

        protected async Task<T> GetRandomRecordFromURLAsync<T>(string url)
        {
            var existingRecords = await APIClient.GetFromJsonAsync<List<T>>(url);
            return existingRecords[Rand.Next(0, existingRecords.Count)];
        }

        protected async Task AssertResponseIsExpectedAsync(HttpResponseMessage response, HttpStatusCode statusCode, string? messageContent = null)
        {
            Assert.IsTrue(response.StatusCode == statusCode);
            if (!string.IsNullOrWhiteSpace(messageContent))
            {
                var responseMessage = await response.Content.ReadAsStringAsync();
                Assert.IsTrue(responseMessage == messageContent);
            }
        }
    }
}
