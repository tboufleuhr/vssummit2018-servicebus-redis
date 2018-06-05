using VsSummit2018.Application;
using VsSummit2018.Application.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace VsSummit2018.Api.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("user-profile")]
    public class UserProfileController : ControllerBase
    {
        private readonly UserProfileAppService userProfileService;

        public UserProfileController(UserProfileAppService userProfileService)
        {
            this.userProfileService = userProfileService;
        }

        [HttpGet("{userProfileId}", Name = "GetUserProfileById")]
        public async Task<IActionResult> Get(int userProfileId)
        {
            var contractor = await userProfileService.GetAsync(userProfileId);
            if (contractor == null)
            {
                return NotFound(userProfileId);
            }

            return Ok(contractor);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]UserProfileCreate createCommandRequest)
        {
            var result = await userProfileService.CreateAsync(createCommandRequest);

            return Ok(result);
        }

        [HttpPut("{userProfileId}")]
        public async Task<IActionResult> Put(int userProfileId, [FromBody]UserProfileUpdateInfo updateCommandRequest)
        {
            if (!userProfileService.Exists(userProfileId))
            {
                return NotFound(userProfileId);
            }

            updateCommandRequest.UserProfileId = userProfileId; //Ensure same Id
            await userProfileService.UpdateAsync(updateCommandRequest);

            return Ok();
        }
    }
}
