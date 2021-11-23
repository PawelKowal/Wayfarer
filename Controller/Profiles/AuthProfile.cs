using ApplicationCore.Dtos.Auth;
using AutoMapper;
using Controller.Dtos.Auth;

namespace Controller.Profiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<LoginUserRequest, LoginDto>();
            CreateMap<RegisterUserRequest, RegistrationDto>();
            CreateMap<AuthServiceResultDto, AuthResponse>();
        }
    }
}
