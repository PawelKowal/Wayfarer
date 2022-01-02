using ApplicationCore.Dtos;
using ApplicationCore.Dtos.Auth;
using ApplicationCore.Interfaces;
using AutoMapper;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private WayfarerDbContext _context;
        private UserManager<User> _userManager;
        private IMapper _mapper;

        public UsersRepository(WayfarerDbContext context, UserManager<User> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<SimpleResultDto> CreateUserAsync(UserDto userDto, string password)
        {
            var result = await _userManager.CreateAsync(_mapper.Map<User>(userDto), password);

            return _mapper.Map<SimpleResultDto>(result);
        }

        public async Task<SimpleResultDto> DeleteRefreshTokenAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            user.RefreshToken = null;

            var updateResult = await _userManager.UpdateAsync(user);

            return _mapper.Map<SimpleResultDto>(updateResult);
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            return _mapper.Map<UserDto>(await _userManager.FindByEmailAsync(email));
        }

        public async Task<UserDto> GetUserByRefreshTokenAsync(string token)
        {
            return _mapper.Map<UserDto>(await _context.Users
                .Include(user => user.RefreshToken)
                .FirstOrDefaultAsync(user => user.RefreshToken.Token == token));
        }

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users
                .Include(user => user.Posts)
                .ThenInclude(post => post.User)
                .Include(user => user.Followers)
                .Include(user => user.Following)
                .FirstOrDefaultAsync(user => user.Id == userId);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserWithPostsByIdAsync(int userId)
        {
            var user = await _context.Users
                .Include(user => user.Posts)
                .Include(user => user.Followers)
                .Include(user => user.Following)
                .FirstOrDefaultAsync(user => user.Id == userId);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return await _context.Users
                .Select(user => _mapper.Map<UserDto>(user))
                .ToListAsync();
        }

        public async Task<bool> CheckUserPasswordAsync(UserDto userDto, string password)
        {
            var user = await _userManager.FindByIdAsync(userDto.UserId.ToString());

            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<GenerateRefreshTokenResultDto> GenerateRefreshTokenAsync(UserDto userDto, int refreshTokenExpirationInHours)
        {
            var user = await _userManager.FindByIdAsync(userDto.UserId.ToString());
            if (user is null)
            {
                return new GenerateRefreshTokenResultDto() { IsSuccess = false };
            }

            var token = await _userManager.GenerateUserTokenAsync(user, "MyApp", "RefreshToken");
            user.RefreshToken = new RefreshToken() { Token = token, Expires = DateTime.UtcNow.AddHours(refreshTokenExpirationInHours) };

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return new GenerateRefreshTokenResultDto() { IsSuccess = false };
            }

            return new GenerateRefreshTokenResultDto() { IsSuccess = true, RefreshToken = token };
        }

        public async Task<SimpleResultDto> UpdateUserAsync(UserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(userDto.UserId.ToString());

            if (user is null)
            {
                var errors = new Dictionary<string, string>();
                errors.Add("NotFound", "User not found.");

                return new SimpleResultDto(false, errors);
            }

            user.UserName = userDto.Username;
            if (userDto.Image is not null)
            {
                user.Image = userDto.Image;
            }
            user.ProfileDescription = userDto.ProfileDescription;

            var updateResult = await _userManager.UpdateAsync(user);
            return _mapper.Map<SimpleResultDto>(updateResult);
        }
    }
}
