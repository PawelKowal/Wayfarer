using ApplicationCore.Dtos;
using AutoMapper;
using Controller.Dtos.Post;

namespace Controller.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<PostRequest, PostDto>();
            CreateMap<PostDto, PostResponse>();
        }
    }
}
