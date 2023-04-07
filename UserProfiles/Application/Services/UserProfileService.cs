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

        public async Task Create(UserProfileModel userProfile)
        {
            await _userProfileRepository.Create(userProfile);
        }

        public async Task<UserProfileModel> GetById(string id)
        {
            return await _userProfileRepository.GetItemById(id);
        }
    }
}
