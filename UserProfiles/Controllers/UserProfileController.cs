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

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            if (page == 0)
            {
                throw new ArgumentNullException("Value of page is invalid");
            }
            if (size == 0)
            {
                throw new ArgumentNullException("Value of size is invalid");
            }

            var result = await _userProfileService.GetUserProfiles(page, size);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserProfileModel userProfile)
        {
            try
            {
                userProfile.Id ??= Guid.NewGuid().ToString();
                await _userProfileService.Create(userProfile);
            }
            catch (Exception)
            {
                throw;
            }
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UserProfileModel userProfile)
        {
            try
            {
                var result = await _userProfileService.Update(id, userProfile);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _userProfileService.Delete(id);
                return NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}