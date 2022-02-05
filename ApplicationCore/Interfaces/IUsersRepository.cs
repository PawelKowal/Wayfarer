using ApplicationCore.Dtos;
using ApplicationCore.Dtos.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IUsersRepository
    {
        Task<SimpleResultDto> CreateUserAsync(UserDto userDto, string password);
        Task<SimpleResultDto> DeleteRefreshTokenAsync(int userId);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> GetUserByRefreshTokenAsync(string token);
        Task<UserDto> GetUserByIdAsync(int userId);
        Task<UserDto> GetUserWithPostsByIdAsync(int userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<bool> CheckUserPasswordAsync(UserDto userDto, string password);
        Task<GenerateRefreshTokenResultDto> GenerateRefreshTokenAsync(UserDto userDto, int refreshTokenExpirationInHours);
        Task<SimpleResultDto> UpdateUserAsync(UserDto userDto);
        Task<IEnumerable<UserDto>> SearchUsersByTextAsync(string text);
        Task<IEnumerable<UserDto>> GetAllUsersSortedByLastMessageTimeAsync(int userId);
    }
}
