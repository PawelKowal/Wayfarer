using ApplicationCore.Dtos;
using ApplicationCore.Dtos.Auth;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IAuthService
    {
        public Task<AuthServiceResultDto> RegisterUserAsync(RegistrationDto registrationModel);
        public Task<AuthServiceResultDto> LoginUserAsync(LoginDto loginModel);
        public Task<AuthServiceResultDto> RefreshTokenAsync(string refreshToken);
        public Task<SimpleResultDto> LogoutUserAsync(string userId);
    }
}
