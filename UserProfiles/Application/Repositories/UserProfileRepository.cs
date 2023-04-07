using Microsoft.Azure.Cosmos;
using UserProfiles.Models;

namespace UserProfiles.Application.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        public string DatabaseId => Constants.CosmosDBOptions.Database;
        public string ContainerId => Constants.CosmosDBOptions.ContainerId;

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

        public async Task EnsureDatabaseExists(ThroughputProperties throughputProperties)
        {
            var response = await _cosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseId, throughputProperties);
            _database = response.Database;
        }

        public async Task EnsureContainerExists(ContainerProperties containerProperties ,ThroughputProperties throughputProperties)
        {
            var response = await _database.CreateContainerIfNotExistsAsync(containerProperties, throughputProperties);
            _container = response.Container;
        }

        public async Task Create(UserProfileModel userProfile)
        {
            await _container.CreateItemAsync<UserProfileModel>(userProfile, PartitionKey.None);
        }

        public async Task<UserProfileModel> GetItemById(string id)
        {
            var response = await _container.ReadItemAsync<UserProfileModel>(id, PartitionKey.None);
            return response.Resource;
        }
    }
}
