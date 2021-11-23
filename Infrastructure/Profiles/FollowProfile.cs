using ApplicationCore.Dtos;
using AutoMapper;
using Infrastructure.Entities;

namespace Infrastructure.Profiles
{
    public class FollowProfile : Profile
    {
        public FollowProfile()
        {
            CreateMap<Follow, FollowDto>();
        }
    }
}
