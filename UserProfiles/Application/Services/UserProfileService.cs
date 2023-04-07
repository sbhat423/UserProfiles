using Microsoft.Azure.Cosmos;
using UserProfiles.Application.Repositories;
using UserProfiles.Models;

namespace UserProfiles.Application.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public UserProfileService(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public Task<UserProfileModel> GetById(string id, PartitionKey partitionKey)
        {
            return _userProfileRepository.GetItemById(id, partitionKey);
        }
    }
}
