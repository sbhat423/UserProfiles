using Newtonsoft.Json;

namespace UserProfiles.Models
{
    public class UserProfileModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string ProfilePicLink { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
