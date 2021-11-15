using System.Collections.Generic;

namespace ApplicationCore.Dtos.Auth
{
    public class AuthServiceResultDto
    {
        public bool IsSuccess { get; set; }
        public IDictionary<string, string> ErrorMessages { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }

    }
}
