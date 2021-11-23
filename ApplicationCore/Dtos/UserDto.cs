using ApplicationCore.Dtos.Auth;
using System.Collections.Generic;

namespace ApplicationCore.Dtos
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
        public string ProfileDescription { get; set; }
        public RefreshTokenDto RefreshToken { get; set; }
        public List<PostDto> Posts { get; set; }
        public List<FollowDto> Following { get; set; }
        public List<FollowDto> Followers { get; set; }
        public string? ConnectionId { get; set; }
    }
}
