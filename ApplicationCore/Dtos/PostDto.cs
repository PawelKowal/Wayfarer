using System;
using System.Collections.Generic;

namespace ApplicationCore.Dtos
{
    public class PostDto
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Image { get; set; }
        public DateTimeOffset PublicationDate { get; set; }
        public int ReactionsCounter { get; set; }
        public List<PostReactionDto> PostReactions { get; set; }
        public List<CommentDto> Comments { get; set; }
        public int UserId { get; set; }
        public UserDto User { get; set; }
        public bool? Reacted { get; set; }
    }
}
