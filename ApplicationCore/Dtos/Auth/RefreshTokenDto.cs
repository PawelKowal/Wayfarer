using System;

namespace ApplicationCore.Dtos.Auth
{
    public class RefreshTokenDto
    {
        public int RefreshTokenId { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
