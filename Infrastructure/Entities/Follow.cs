using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    public class Follow
    {
        public Guid FollowerId { get; set; }
        [ForeignKey("FollowerId")]
        public User Follower { get; set; }

        public Guid FollowedId { get; set; }
        [ForeignKey("FollowedId")]
        public User Followed { get; set; }
    }
}
