using ApplicationCore.Dtos;

namespace ApplicationCore.Interfaces
{
    public interface IPostsService
    {
        public PostDto UpdatePostData(PostDto oldPost, PostDto newPost);
    }
}
