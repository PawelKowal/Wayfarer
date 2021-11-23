using ApplicationCore.Dtos;
using AutoMapper;
using Controller.Dtos.Post;

namespace Controller.Profiles
{
    public class AreaProfile : Profile
    {
        public AreaProfile()
        {
            CreateMap<AreaRequest, AreaDto>();
        }
    }
}
