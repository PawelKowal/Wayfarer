using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    public class CommentReaction
    {
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int CommentId { get; set; }
        [ForeignKey("CommentId")]
        public Comment Comment { get; set; }
    }
}
