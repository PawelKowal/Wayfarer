using Controller.Dtos.Comment;
using Controller.Dtos.User;
using System;
using System.Collections.Generic;

namespace Controller.Dtos.Post
{
    public class PostResponse
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Image { get; set; }
        public DateTimeOffset PublicationDate { get; set; }
        public int ReactionsCounter { get; set; }
        public List<CommentResponse> Comments { get; set; }
        public int UserId { get; set; }
        public UserResponse User { get; set; }
        public bool? Reacted { get; set; }
    }
}
