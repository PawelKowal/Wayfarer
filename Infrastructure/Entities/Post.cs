using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    public class Post
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public Point Location { get; set; }
        public string Image { get; set; }
        public DateTimeOffset PublicationDate { get; set; }
        public int ReactionsCounter { get; set; }
        public List<PostReaction> PostReactions { get; set; }
        public List<Comment> Comments { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
