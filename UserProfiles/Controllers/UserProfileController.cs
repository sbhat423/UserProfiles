using Microsoft.AspNetCore.Mvc;
using UserProfiles.Application.Services;
using UserProfiles.Models;

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

            var result = await _userProfileService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserProfileModel userProfile)
        {
            try
            {
                userProfile.Id = userProfile.Id ?? Guid.NewGuid().ToString();
                await _userProfileService.Create(userProfile);
            }
            catch (Exception)
            {
                throw;
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _userProfileService.Delete(id);
            }
            catch (Exception)
            {
                throw;
            }
            return NoContent();
        }
    }
}