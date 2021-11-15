using ApplicationCore.Dtos.Auth;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Dtos
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
        public string ProfileDescription { get; set; }
        public RefreshTokenDto RefreshToken { get; set; }
        public List<PostDto> Posts { get; set; }
    }
}
