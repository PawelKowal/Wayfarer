using ApplicationCore.Dtos.Auth;
using AutoMapper;
using Infrastructure.Entities;

namespace Infrastructure.Profiles
{
    public class TokenProfile : Profile
    {
        public TokenProfile()
        {
            CreateMap<RefreshToken, RefreshTokenDto>();
        }
    }
}
