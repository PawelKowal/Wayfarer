using System;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IReactionsRepository
    {
        Task<bool> AddPostReactionAsync(Guid userId, int postId);
        Task<bool> DoesPostReactionExistAsync(Guid userId, int reactionId);
        Task<bool> DeletePostReactionAsync(Guid userId, int postId);
        Task<bool> AddCommentReactionAsync(Guid userId, int commentId);
        Task<bool> DoesCommentReactionExistAsync(Guid userId, int reactionId);
        Task<bool> DeleteCommentReactionAsync(Guid userId, int postId);
    }
}
