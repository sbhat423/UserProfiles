using Microsoft.Azure.Cosmos;
using UserProfiles.Models;

namespace UserProfiles.Application.Repositories
{
    public interface IUserProfileRepository
    {
        Task<UserProfileModel> GetItemById(string id, PartitionKey partitionKey);
    }
}
