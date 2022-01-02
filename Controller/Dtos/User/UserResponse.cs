using Controller.Dtos.Post;
using System.Collections.Generic;

namespace Controller.Dtos.User
{
    public class UserResponse
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
        public string ProfileDescription { get; set; }
        public List<PostResponse> Posts { get; set; }
        public List<int> Following { get; set; }
        public List<int> Followers { get; set; }
    }
}
