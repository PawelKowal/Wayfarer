using System.Threading.Tasks;
using ApplicationCore.Dtos;

namespace ApplicationCore.Interfaces
{
    public interface ICommentsRepository
    {
        Task<CommentDto> AddCommentAsync(CommentDto commentDto);
        Task DeleteCommentAsync(int commentId);
        Task<CommentDto> GetCommentByIdAsync(int id);
    }
}
