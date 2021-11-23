using ApplicationCore.Dtos;
using ApplicationCore.Dtos.Auth;
using ApplicationCore.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public class AuthService : IAuthService
    {
        private readonly int refreshTokenExpirationInHours;
        private readonly int accessTokenExpirationInMinutes;
        private readonly string validAudience;
        private readonly string validIssuer;

        private readonly IUsersRepository _usersRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(IUsersRepository usersRepository, IConfiguration configuration, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _configuration = configuration;
            _mapper = mapper;

            refreshTokenExpirationInHours = int.Parse(_configuration["Tokens:RefreshTokenExpirationInHours"]);
            accessTokenExpirationInMinutes = int.Parse(_configuration["Tokens:AccessTokenExpirationInMinutes"]);
            validAudience = _configuration["JWT:ValidAudience"];
            validIssuer = _configuration["JWT:ValidIssuer"];
        }

        public async Task<AuthServiceResultDto> LoginUserAsync(LoginDto loginModel)
        {
            var user = await _usersRepository.GetUserByEmailAsync(loginModel.Email);

            var errorResult = GenerateResultWithSingleError("", "Email or password are incorrect.");

            if (user is null)
            {
                return errorResult;
            }

            var result = await _usersRepository.CheckUserPasswordAsync(user, loginModel.Password);

            if (!result)
            {
                return errorResult;
            }

            var accessToken = GenerateAccessToken(user);

            var generateRefreshTokenResultDto = await _usersRepository.GenerateRefreshTokenAsync(user, refreshTokenExpirationInHours);
            if (!generateRefreshTokenResultDto.IsSuccess)
            {
                return GenerateResultWithSingleError("", "An unknown error occured.");
            }

            return new AuthServiceResultDto() { IsSuccess = true, AccessToken = accessToken, RefreshToken = generateRefreshTokenResultDto.RefreshToken };
        }

        public Task<SimpleResultDto> LogoutUserAsync(int userId)
        {
            return _usersRepository.DeleteRefreshTokenAsync(userId);
        }

        public async Task<AuthServiceResultDto> RefreshTokenAsync(string refreshToken)
        {
            var user = await _usersRepository.GetUserByRefreshTokenAsync(refreshToken);

            if (user is null)
            {
                return GenerateResultWithSingleError("Token", "User not found for given token.");
            }

            var refreshTokenDto = user.RefreshToken;

            if (refreshTokenDto.Expires < DateTime.UtcNow)
            {
                return GenerateResultWithSingleError("Token", "Token expired.");
            }

            var newAccessToken = GenerateAccessToken(user);

            var generateRefreshTokenResultDto = await _usersRepository.GenerateRefreshTokenAsync(user, refreshTokenExpirationInHours);
            if (!generateRefreshTokenResultDto.IsSuccess)
            {
                return GenerateResultWithSingleError("", "An unknown error occured.");
            }

            return new AuthServiceResultDto() { IsSuccess = true, AccessToken = newAccessToken, RefreshToken = generateRefreshTokenResultDto.RefreshToken };
        }

        public async Task<AuthServiceResultDto> RegisterUserAsync(RegistrationDto registrationModel)
        {
            //TODO: store it with other images?
            byte[] imageArray = System.IO.File.ReadAllBytes(@"./Utils/defaultAvatar.jpg");
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);

            var user = _mapper.Map<UserDto>(registrationModel);
            user.Image = base64ImageRepresentation;

            var createResult = await _usersRepository.CreateUserAsync(user, registrationModel.Password);
            var newUser = await _usersRepository.GetUserByEmailAsync(user.Email);

            if (createResult.IsSuccess)
            {
                var accessToken = GenerateAccessToken(user);

                var generateRefreshTokenResultDto = await _usersRepository.GenerateRefreshTokenAsync(newUser, refreshTokenExpirationInHours);
                if (!generateRefreshTokenResultDto.IsSuccess)
                {
                    return GenerateResultWithSingleError("", "An unknown error occured.");
                }

                return new AuthServiceResultDto() { IsSuccess = true, AccessToken = accessToken, RefreshToken = generateRefreshTokenResultDto.RefreshToken };
            }

            return _mapper.Map<AuthServiceResultDto>(createResult);
        }

        private string GenerateAccessToken(UserDto user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var accessToken =  new JwtSecurityToken(
                issuer: validIssuer,
                audience: validAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(accessTokenExpirationInMinutes),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(accessToken);
        }

        private static AuthServiceResultDto GenerateResultWithSingleError(string key, string value)
        {
            var errors = new Dictionary<string, string>();
            errors.Add(key, value);

            return new AuthServiceResultDto() { IsSuccess = false, ErrorMessages = errors };
        }
    }
}
