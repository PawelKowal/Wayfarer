using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowsController : ControllerBase
    {
        private readonly IFollowsRepository _followsRepository;
        public FollowsController(IFollowsRepository followsRepository)
        {
            _followsRepository = followsRepository;
        }

        //PUT api/follows/{id}
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> FollowUserAsync(int id)
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizedUserId == null)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            var followerId = int.Parse(authorizedUserId.Value);
            var followedId = id;

            var result = await _followsRepository.FollowUserAsync(followerId, followedId);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "An error occured.");
                return ValidationProblem(statusCode: StatusCodes.Status422UnprocessableEntity);
            }

            return NoContent();
        }

        //DELETE api/follows/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFollowAsync(int id)
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizedUserId == null)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            var followerId = int.Parse(authorizedUserId.Value);
            var followedId = id;

            var doesFollowExist = await _followsRepository.CheckIfFollowExistAsync(followerId, followedId);
            if (!doesFollowExist)
            {
                ModelState.AddModelError(string.Empty, "Follow not found.");
                return ValidationProblem(statusCode: StatusCodes.Status404NotFound);
            }

            var result = await _followsRepository.UnfollowUserAsync(followerId, followedId);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "An error occured.");
                return ValidationProblem(statusCode: StatusCodes.Status422UnprocessableEntity);
            }

            return NoContent();
        }
    }
}
