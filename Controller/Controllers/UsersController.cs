using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ApplicationCore.Dtos;
using ApplicationCore.Interfaces;
using AutoMapper;
using Controller.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _hostEnvironment;

        public UsersController(IUsersRepository usersRepository, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<FullUserResponse>> GetLoggedUserAsync()
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizedUserId == null)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            var userDto = await _usersRepository.GetUserByIdAsync(int.Parse(authorizedUserId.Value));
            if (userDto is not null)
            {
                return Ok(_mapper.Map<FullUserResponse>(userDto));
            }

            ModelState.AddModelError(string.Empty, "User not found.");
            return ValidationProblem(statusCode: StatusCodes.Status404NotFound);
        }

        //GET api/user/{id}
        [HttpGet("{Id}", Name = "GetUserById")]
        [Authorize]
        public async Task<ActionResult<FullUserResponse>> GetUserByIdAsync(int Id)
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizedUserId == null)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            var user = await _usersRepository.GetUserByIdAsync(Id);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return ValidationProblem(statusCode: StatusCodes.Status404NotFound);
            }

            return Ok(_mapper.Map<FullUserResponse>(user));
        }

        //GET api/user/{id}
        [HttpGet("{Id}/posts")]
        [Authorize]
        public async Task<ActionResult<FullUserResponse>> GetUserWithPostsByIdAsync(int Id)
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizedUserId == null)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            var user = await _usersRepository.GetUserWithPostsByIdAsync(Id);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return ValidationProblem(statusCode: StatusCodes.Status404NotFound);
            }

            return Ok(_mapper.Map<FullUserResponse>(user));
        }

        //GET /api/user
        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsersAsync()
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizedUserId == null)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            var users = await _usersRepository.GetAllUsersSortedByLastMessageTimeAsync(int.Parse(authorizedUserId.Value));

            return Ok(users.Select(user => _mapper.Map<UserResponse>(user)));
        }

        //PUT api/user
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUserAsync([FromForm] UserRequest userRequest)
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (int.Parse(authorizedUserId.Value) != userRequest.UserId)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            var userDto = _mapper.Map<UserDto>(userRequest);

            if(userRequest.Image is not null)
            {
                userDto.Image = await SaveImageAsync(userRequest.Image);
            }

            var result = await _usersRepository.UpdateUserAsync(userDto);
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

        //GET api/user/search/{text}
        [HttpGet("search/{text}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserResponse>>> SearchUsersByTextAsync(string text)
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizedUserId == null)
            {
                ModelState.AddModelError(string.Empty, "Unauthorized.");
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
            }

            var users = await _usersRepository.SearchUsersByTextAsync(text);

            return Ok(_mapper.Map<IEnumerable<UserResponse>>(users));
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            string imageName = new string(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }
    }
}
