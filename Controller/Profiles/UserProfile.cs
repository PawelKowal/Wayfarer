using ApplicationCore.Dtos;
using AutoMapper;
using Controller.Dtos.User;

namespace Controller.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRequest, UserDto>();
            CreateMap<UserDto, UserResponse>();
        }
    }
}
