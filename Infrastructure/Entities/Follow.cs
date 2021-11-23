using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    public class Follow
    {
        public int FollowerId { get; set; }
        [ForeignKey("FollowerId")]
        public User Follower { get; set; }

        public int FollowedId { get; set; }
        [ForeignKey("FollowedId")]
        public User Followed { get; set; }
    }
}
