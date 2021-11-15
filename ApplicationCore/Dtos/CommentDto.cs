using System;
using System.Collections.Generic;

namespace ApplicationCore.Dtos
{
    public class CommentDto
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime PublicationDate { get; set; }
        public int ReactionsCounter { get; set; }
        public List<CommentReactionDto> CommentReactions { get; set; }
        public int PostId { get; set; }
        public PostDto Post { get; set; }
        public Guid UserId { get; set; }
        public UserDto User { get; set; }
        public bool? Reacted { get; set; }
    }
}
