using ApplicationCore.Dtos;
using ApplicationCore.Interfaces;

namespace ApplicationCore.Services
{
    public class PostsService : IPostsService
    {
        public PostDto UpdatePostData(PostDto oldPost, PostDto newPost)
        {
            newPost.PublicationDate = oldPost.PublicationDate;
            newPost.ReactionsCounter = oldPost.ReactionsCounter;
            newPost.UserId = oldPost.UserId;
            newPost.Reacted = oldPost.Reacted;

            return newPost;
        }
    }
}
