using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Controller.Dtos.Post
{
    public class PostRequest
    {
        public string Content { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        [Required]
        public IFormFile Image { get; set; }
    }
}
