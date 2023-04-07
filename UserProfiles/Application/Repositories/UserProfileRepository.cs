using Microsoft.Azure.Cosmos;
using UserProfiles.Models;

namespace UserProfiles.Application.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        public string DatabaseId => Constants.CosmosDbOptions.Database;
        public string ContainerId => Constants.CosmosDbOptions.ContainerId;

        private readonly CosmosClient _cosmosClient;
        private Database _database;
        private Container _container;

        public UserProfileRepository(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient ?? throw new ArgumentNullException(nameof(cosmosClient));
        }

        public async Task Initialize(ContainerProperties containerProperties)
        {
            _database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseId);
            _container = await _database.CreateContainerIfNotExistsAsync(containerProperties);
        }

        public async Task<UserProfileModel> GetItemById(string id, PartitionKey partitionKey)
        {
            var response = await _container.ReadItemAsync<UserProfileModel>(id, partitionKey);
            return response.Resource;
        }
    }
}
