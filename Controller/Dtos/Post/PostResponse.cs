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
        public string Location { get; set; }
        public string Image { get; set; }
        public DateTime PublicationDate { get; set; }
        public int ReactionsCounter { get; set; }
        public List<CommentResponse> Comments { get; set; }
        public Guid UserId { get; set; }
        public UserResponse User { get; set; }
        public bool? Reacted { get; set; }
    }
}
