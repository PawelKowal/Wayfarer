using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public class User: IdentityUser<Guid>
    {
        public string Image { get; set; }
        public string ProfileDescription { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public List<Post> Posts { get; set; }
        public List<Follow> Following { get; set; }
        public List<Follow> Followers { get; set; }
    }
}