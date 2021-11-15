using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime PublicationDate { get; set; }
        public int ReactionsCounter { get; set; }
        public List<CommentReaction> CommentReactions { get; set; }
        public int? PostId { get; set; }
        [ForeignKey("PostId")]
        public Post Post { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
