using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xunit;

namespace FireplaceApi.Api.IntegrationTests
{
    [Collection("Api Integration Test Collection")]
    public class UserTests
    {
        private readonly ILogger<UserTests> _logger;
        private readonly ClientPool _clientPool;
        private readonly TestUtils _testUtils;

        public UserTests(ApiIntegrationTestFixture testFixture)
        {
            _logger = testFixture.ServiceProvider.GetRequiredService<ILogger<UserTests>>();
            _clientPool = testFixture.ClientPool;
            _testUtils = testFixture.TestUtils;
        }

        [Fact]
        public async Task TestGetUserWithIdReturnDoesNotExistsErrorAsync()
        {
            _logger.LogInformation($"{nameof(TestGetUserWithIdReturnDoesNotExistsErrorAsync)} | Start | ()");

            var id = 999;
            var requestUri = $"/v0.1/users/{id}";
            var response = await _clientPool.TheHulkClient.GetAsync(requestUri);

            await _testUtils.AssertResponseContainsErrorAsync(ErrorName.USER_ID_DOES_NOT_EXIST_OR_ACCESS_DENIED,
                response, nameof(TestGetUserWithIdReturnDoesNotExistsErrorAsync));

            _logger.LogInformation($"{nameof(TestGetUserWithIdReturnDoesNotExistsErrorAsync)} | End");
        }

        //[Fact]
        //public async Task TestGetStockItemAsync()
        //{
        //    // Arrange
        //    var request = "/api/v1/Warehouse/StockItem/1";

        //    // Act
        //    var response = await _client.GetAsync(request);

        //    // Assert
        //    response.EnsureSuccessStatusCode();
        //}

        //[Fact]
        //public async Task TestPostStockItemAsync()
        //{
        //    // Arrange
        //    var request = new
        //    {
        //        Url = "/api/v1/Warehouse/StockItem",
        //        Body = new
        //        {
        //            StockItemName = string.Format("USB anime flash drive - Vegeta {0}", Guid.NewGuid()),
        //            SupplierID = 12,
        //            UnitPackageID = 7,
        //            OuterPackageID = 7,
        //            LeadTimeDays = 14,
        //            QuantityPerOuter = 1,
        //            IsChillerStock = false,
        //            TaxRate = 15.000m,
        //            UnitPrice = 32.00m,
        //            RecommendedRetailPrice = 47.84m,
        //            TypicalWeightPerUnit = 0.050m,
        //            CustomFields = "{ \"CountryOfManufacture\": \"Japan\", \"Tags\": [\"32GB\",\"USB Powered\"] }",
        //            Tags = "[\"32GB\",\"USB Powered\"]",
        //            SearchDetails = "USB anime flash drive - Vegeta",
        //            LastEditedBy = 1,
        //            ValidFrom = DateTime.Now,
        //            ValidTo = DateTime.Now.AddYears(5)
        //        }
        //    };

        //    // Act
        //    var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
        //    var value = await response.Content.ReadAsStringAsync();

        //    // Assert
        //    response.EnsureSuccessStatusCode();
        //}

        //[Fact]
        //public async Task TestPutStockItemAsync()
        //{
        //    // Arrange
        //    var request = new
        //    {
        //        Url = "/api/v1/Warehouse/StockItem/1",
        //        Body = new
        //        {
        //            StockItemName = string.Format("USB anime flash drive - Vegeta {0}", Guid.NewGuid()),
        //            SupplierID = 12,
        //            Color = 3,
        //            UnitPrice = 39.00m
        //        }
        //    };

        //    // Act
        //    var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));

        //    // Assert
        //    response.EnsureSuccessStatusCode();
        //}

        //[Fact]
        //public async Task TestDeleteStockItemAsync()
        //{
        //    // Arrange

        //    var postRequest = new
        //    {
        //        Url = "/api/v1/Warehouse/StockItem",
        //        Body = new
        //        {
        //            StockItemName = string.Format("Product to delete {0}", Guid.NewGuid()),
        //            SupplierID = 12,
        //            UnitPackageID = 7,
        //            OuterPackageID = 7,
        //            LeadTimeDays = 14,
        //            QuantityPerOuter = 1,
        //            IsChillerStock = false,
        //            TaxRate = 10.000m,
        //            UnitPrice = 10.00m,
        //            RecommendedRetailPrice = 47.84m,
        //            TypicalWeightPerUnit = 0.050m,
        //            CustomFields = "{ \"CountryOfManufacture\": \"USA\", \"Tags\": [\"Sample\"] }",
        //            Tags = "[\"Sample\"]",
        //            SearchDetails = "Product to delete",
        //            LastEditedBy = 1,
        //            ValidFrom = DateTime.Now,
        //            ValidTo = DateTime.Now.AddYears(5)
        //        }
        //    };

        //    // Act
        //    var postResponse = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));
        //    var jsonFromPostResponse = await postResponse.Content.ReadAsStringAsync();

        //    var singleResponse = JsonConvert.DeserializeObject<SingleResponse<StockItem>>(jsonFromPostResponse);

        //    var deleteResponse = await _client.DeleteAsync(string.Format("/api/v1/Warehouse/StockItem/{0}", singleResponse.Model.StockItemID));

        //    // Assert
        //    postResponse.EnsureSuccessStatusCode();

        //    Assert.False(singleResponse.DidError);

        //    deleteResponse.EnsureSuccessStatusCode();
        //}
    }
}