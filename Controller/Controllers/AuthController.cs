using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Interfaces;
using AutoMapper;
using Controller.Dtos.Auth;
using ApplicationCore.Dtos.Auth;

namespace Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService,
                            IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        //TODO: remove user
        //TODO: change email
        //TODO: update user
        //TODO: change password

        // /api/auth/register
        [HttpPost("Register")]
        public async Task<ActionResult<AuthResponse>> RegisterUserAsync([FromBody]RegisterUserRequest registerUserRequest)
        {
            var result = await _authService.RegisterUserAsync(_mapper.Map<RegistrationDto>(registerUserRequest));

            if (result.IsSuccess)
            {
                SetTokenCookie(result.RefreshToken);

                return Ok(_mapper.Map<AuthResponse>(result));
            }

            foreach (var errorMessage in result.ErrorMessages)
            {
                ModelState.AddModelError(errorMessage.Key, errorMessage.Value);
            }
            return ValidationProblem(statusCode: StatusCodes.Status400BadRequest);
        }

        // /api/auth/login
        [HttpPost("Login")]
        public async Task<ActionResult<AuthResponse>> LoginUserAsync([FromBody]LoginUserRequest loginUserRequest)
        {
            var result = await _authService.LoginUserAsync(_mapper.Map<LoginDto>(loginUserRequest));

            if (result.IsSuccess)
            {
                SetTokenCookie(result.RefreshToken);

                return Ok(_mapper.Map<AuthResponse>(result));
            }
            foreach (var errorMessage in result.ErrorMessages)
            {
                ModelState.AddModelError(errorMessage.Key, errorMessage.Value);
            }
            return ValidationProblem(statusCode: StatusCodes.Status400BadRequest);
        }

        // /api/auth/refresh-token
        [HttpGet("Refresh-token")]
        public async Task<ActionResult<AuthResponse>> RefreshTokenAsync()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (result.IsSuccess)
            {
                SetTokenCookie(result.RefreshToken);

                return Ok(_mapper.Map<AuthResponse>(result));
            }

            foreach (var errorMessage in result.ErrorMessages)
            {
                ModelState.AddModelError(errorMessage.Key, errorMessage.Value);
            }
            return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
        }

        // /api/auth/logout
        [HttpGet("Logout")]
        [Authorize]
        public async Task<IActionResult> LogoutUserAsync()
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (authorizedUserId is not null)
            {
                var result = await _authService.LogoutUserAsync(int.Parse(authorizedUserId));

                if (result.IsSuccess)
                {
                    return NoContent();
                }

                foreach (var errorMessage in result.ErrorMessages)
                {
                    ModelState.AddModelError(errorMessage.Key, errorMessage.Value);
                }
                return ValidationProblem(statusCode: StatusCodes.Status400BadRequest);
            }

            ModelState.AddModelError(string.Empty, "Unauthorized.");
            return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized);
        }

        //helper methods
        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Secure = true
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
