using System.Collections.Generic;

namespace ApplicationCore.Dtos.Auth
{
    public class GenerateRefreshTokenResultDto
    {
        public bool IsSuccess { get; set; }
        public string RefreshToken { get; set; }
    }
}
