using Microsoft.AspNetCore.Http;

namespace Controller.Dtos.User
{
    public class UserRequest
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public IFormFile Image { get; set; }
        public string ProfileDescription { get; set; }
    }
}
