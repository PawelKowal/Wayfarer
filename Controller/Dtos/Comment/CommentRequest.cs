using System;

namespace Controller.Dtos.Comment
{
    public class CommentRequest
    {
        public string Content { get; set; }
        public DateTime PublicationDate { get; set; }
        public int ReactionsCounter { get; set; }
        public int PostId { get; set; }
    }
}
