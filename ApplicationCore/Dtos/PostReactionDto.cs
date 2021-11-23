namespace ApplicationCore.Dtos
{
    public class PostReactionDto
    {
        public int PostReactionId { get; set; }
        public int UserId { get; set; }
        public UserDto User { get; set; }
        public int PostId { get; set; }
        public PostDto Post { get; set; }
    }
}
