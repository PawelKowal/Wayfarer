using ApplicationCore.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IPostsRepository
    {
        Task<PostDto> GetPostByIdAsync(int postId, Guid authorizedUserId);
        Task<IEnumerable<PostDto>> GetAllPostsAsync(Guid authorizedUserId);
        Task<PostDto> AddPostAsync(PostDto postDto);
        Task<PostDto> UpdatePostAsync(PostDto postDto);
        Task DeletePostAsync(int postId);
        Task<IEnumerable<PostDto>> GetPostsFromAreaAsync(AreaDto areaDto, Guid guid);
    }
}
