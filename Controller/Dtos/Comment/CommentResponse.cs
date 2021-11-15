using Controller.Dtos.User;
using System;

namespace Controller.Dtos.Comment
{
    public class CommentResponse
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime PublicationDate { get; set; }
        public int ReactionsCounter { get; set; }
        public int PostId { get; set; }
        public Guid UserId { get; set; }
        public UserResponse User { get; set; }
        public bool? Reacted { get; set; }
    }
}
