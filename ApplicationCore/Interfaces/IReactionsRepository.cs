using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IReactionsRepository
    {
        Task<bool> AddPostReactionAsync(int userId, int postId);
        Task<bool> DoesPostReactionExistAsync(int userId, int reactionId);
        Task<bool> DeletePostReactionAsync(int userId, int postId);
        Task<bool> AddCommentReactionAsync(int userId, int commentId);
        Task<bool> DoesCommentReactionExistAsync(int userId, int reactionId);
        Task<bool> DeleteCommentReactionAsync(int userId, int postId);
    }
}
