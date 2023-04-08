using Microsoft.Azure.Cosmos;
using UserProfiles.Models;

namespace UserProfiles.Application.Services
{
    public interface IUserProfileService
    {
        Task<UserProfileModel> GetById(string id);
        Task Create(UserProfileModel userProfile);
        Task Delete(string id);
        Task<IEnumerable<UserProfileModel>> GetUserProfiles(int page, int size);
        Task<UserProfileModel> Update(string id, UserProfileModel userProfile);
    }
}
