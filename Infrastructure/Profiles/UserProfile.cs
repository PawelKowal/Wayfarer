using ApplicationCore.Dtos;
using AutoMapper;
using Infrastructure.Entities;

namespace Infrastructure.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(userDto => userDto.UserId, opt => opt.MapFrom(user => user.Id))
                .ForMember(userDto => userDto.Email, opt => opt.MapFrom(user => user.Email))
                .ForMember(userDto => userDto.Username, opt => opt.MapFrom(user => user.UserName));
            CreateMap<UserDto, User>()
                .ForMember(user => user.Id, opt => opt.MapFrom(userDto => userDto.UserId))
                .ForMember(user => user.Email, opt => opt.MapFrom(userDto => userDto.Email))
                .ForMember(user => user.UserName, opt => opt.MapFrom(userDto => userDto.Username));
        }
    }
}
