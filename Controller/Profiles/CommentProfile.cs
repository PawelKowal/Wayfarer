using ApplicationCore.Dtos;
using AutoMapper;
using Controller.Dtos.Comment;

namespace Controller.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<CommentRequest, CommentDto>();
            CreateMap<CommentDto, CommentResponse>();
        }
    }
}
