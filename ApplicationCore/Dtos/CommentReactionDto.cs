namespace ApplicationCore.Dtos
{
    public class CommentReactionDto
    {
        public int CommentReactionId { get; set; }
        public int UserId { get; set; }
        public UserDto User { get; set; }
        public int CommentId { get; set; }
        public CommentDto Comment { get; set; }
    }
}
