using ApplicationCore.Dtos;
using ApplicationCore.Dtos.Auth;
using AutoMapper;

namespace ApplicationCore.Profiles
{
    public class ResultProfile : Profile
    {
        public ResultProfile()
        {
            CreateMap<SimpleResultDto, AuthServiceResultDto>();
        }
    }
}
