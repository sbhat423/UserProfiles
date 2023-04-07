using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using UserProfiles.Application.Services;

namespace UserProfiles.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        private readonly ILogger<UserProfileController> _logger;

        public UserProfileController(
            IUserProfileService userProfileService,
            ILogger<UserProfileController> logger)
        {
            _userProfileService = userProfileService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }

            var result = await _userProfileService.GetById(id, new PartitionKey());
            return Ok(result);
        }
    }
}