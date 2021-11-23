using ApplicationCore.Dtos;
using AutoMapper;
using Controller.Dtos.Follow;

namespace Controller.Profiles
{
    public class FollowProfile : Profile
    {
        public FollowProfile()
        {
            CreateMap<FollowDto, FollowResponse>();
        }
    }
}
