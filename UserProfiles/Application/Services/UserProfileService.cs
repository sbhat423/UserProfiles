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

        public async Task<IEnumerable<UserProfileModel>> GetUserProfiles(int page, int size)
        {
            var query = @$"
                SELECT * FROM c 
                WHERE c.IsActive = true
                ORDER BY c._ts DESC
                OFFSET {(page - 1) * size}
                LIMIT {size}";

            var queryDefinition = new QueryDefinition(query);
            return await _userProfileRepository.GetItems(queryDefinition);
        }

        public async Task Delete(string id)
        {
            var userProfile = await _userProfileRepository.GetItemById(id);
            if (userProfile == null)
            {
                throw new ArgumentException($"User profile with given Id: {id} not found");
            }

            userProfile.IsActive = false;
            await _userProfileRepository.Update(id, userProfile);
        }
    }
}
