using Microsoft.Azure.Cosmos;
using UserProfiles.Models;

namespace UserProfiles.Application.Repositories
{
    public interface IUserProfileRepository
    {

        Task Create(UserProfileModel userProfile);
        Task EnsureContainerExists(ContainerProperties containerProperties, ThroughputProperties throughputProperties);
        Task EnsureDatabaseExists(ThroughputProperties throughputProperties);
        Task<UserProfileModel> GetItemById(string id);
        Task<UserProfileModel> Update(string id, UserProfileModel userProfile);
    }
}
