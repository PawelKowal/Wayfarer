using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ApplicationCore.Dtos;
using ApplicationCore.Interfaces;
using AutoMapper;
using Controller.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;
        public UsersController(IUsersRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        //GET api/user/{id}
        [HttpGet("{Id}", Name = "GetUserById")]
        public async Task<ActionResult<UserResponse>> GetUserByIdAsync(int Id)
        {
            var user = await _usersRepository.GetUserByIdAsync(Id);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return ValidationProblem(statusCode: StatusCodes.Status404NotFound);
            }

            return Ok(_mapper.Map<UserResponse>(user));
        }

        //GET api/user/{id}
        [HttpGet("{Id}/posts")]
        public async Task<ActionResult<UserResponse>> GetUserWithPostsByIdAsync(int Id)
        {
            var user = await _usersRepository.GetUserWithPostsByIdAsync(Id);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return ValidationProblem(statusCode: StatusCodes.Status404NotFound);
            }

            return Ok(_mapper.Map<UserResponse>(user));
        }

        //GET /api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsersAsync()
        {
            var users = await _usersRepository.GetAllUsersAsync();

            return Ok(users.Select(user => _mapper.Map<UserResponse>(user)));
        }

        //PUT api/user
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UserRequest userRequest)
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (int.Parse(authorizedUserId.Value) != userRequest.UserId)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            var result = await _usersRepository.UpdateUserAsync(_mapper.Map<UserDto>(userRequest));
            if (!result.IsSuccess)
            {
                var key = "NotFound";
                if (result.ErrorMessages.TryGetValue(key, out var message))
                {
                    ModelState.AddModelError(key, message);
                    return ValidationProblem(statusCode: StatusCodes.Status404NotFound);
                }

                foreach (var errorMessage in result.ErrorMessages)
                {
                    ModelState.AddModelError(errorMessage.Key, errorMessage.Value);
                }
                return ValidationProblem(statusCode: StatusCodes.Status400BadRequest);
            }

            return NoContent();
        }
    }
}
