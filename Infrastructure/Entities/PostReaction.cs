using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    public class PostReaction
    {
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int PostId { get; set; }
        [ForeignKey("PostId")]
        public Post Post { get; set; }
    }
}
