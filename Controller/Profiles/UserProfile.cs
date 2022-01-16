using ApplicationCore.Dtos;
using AutoMapper;
using Controller.Dtos.User;
using System.Linq;

namespace Controller.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRequest, UserDto>();
            CreateMap<UserDto, UserResponse>();
            CreateMap<UserDto, FullUserResponse>()
                .ForMember(userResponse => userResponse.Followers, userDto => userDto.MapFrom(userDto => userDto.Followers.Select(follow => follow.FollowerId)))
                .ForMember(userResponse => userResponse.Following, userDto => userDto.MapFrom(userDto => userDto.Following.Select(follow => follow.FollowedId)));
        }
    }
}
