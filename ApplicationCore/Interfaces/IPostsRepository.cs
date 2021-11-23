using ApplicationCore.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IPostsRepository
    {
        Task<PostDto> GetPostByIdAsync(int postId, int authorizedUserId);
        Task<IEnumerable<PostDto>> GetAllPostsAsync(int authorizedUserId);
        Task<PostDto> AddPostAsync(PostDto postDto);
        Task<PostDto> UpdatePostAsync(PostDto postDto);
        Task DeletePostAsync(int postId);
        Task<IEnumerable<PostDto>> GetPostsFromAreaAsync(AreaDto areaDto, int authorizedUserId);
    }
}
