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

        public async Task EnsureContainerExists(ContainerProperties containerProperties, ThroughputProperties throughputProperties)
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

        public async Task<IEnumerable<UserProfileModel>> GetItems(
            QueryDefinition query,
            string continuationToken = default,
            QueryRequestOptions queryRequestOptions = null)
        {
            FeedIterator<UserProfileModel> feedIterator = _container.GetItemQueryIterator<UserProfileModel>(query, continuationToken, queryRequestOptions);
            List<UserProfileModel> userProfiles = new List<UserProfileModel>();
            while (feedIterator.HasMoreResults)
            {
                userProfiles.AddRange(await feedIterator.ReadNextAsync());
            }

            return userProfiles;
        }

        public async Task<UserProfileModel> Update(string id, UserProfileModel userProfile)
        {
            if ((userProfile.Id != null) && (userProfile.Id != id))
            {
                throw new ArgumentException("Id provided for the update operation doesnot matches with the Id of the user profile");
            }

            var originalUserProfile = await GetItemById(id);
            if (originalUserProfile == null)
            {
                throw new ArgumentNullException($"User profile for the given id: {id} not found");
            }

            originalUserProfile.FirstName = userProfile.FirstName ?? originalUserProfile.FirstName;
            originalUserProfile.LastName = userProfile.LastName ?? originalUserProfile.LastName;
            originalUserProfile.Email = userProfile.Email ?? originalUserProfile.Email;
            originalUserProfile.PhoneNumber = userProfile.PhoneNumber ?? originalUserProfile.PhoneNumber;

            return await _container.ReplaceItemAsync<UserProfileModel>(originalUserProfile, id);
        }
    }
}
