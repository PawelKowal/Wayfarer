using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    public class Post
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }
        public DateTime PublicationDate { get; set; }
        public int ReactionsCounter { get; set; }
        public List<PostReaction> PostReactions { get; set; }
        public List<Comment> Comments { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
