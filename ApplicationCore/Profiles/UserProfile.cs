using ApplicationCore.Dtos;
using ApplicationCore.Dtos.Auth;
using AutoMapper;

namespace ApplicationCore.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegistrationDto, UserDto>();
        }
    }
}
