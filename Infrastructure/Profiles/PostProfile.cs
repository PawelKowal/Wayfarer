using ApplicationCore.Dtos;
using AutoMapper;
using Infrastructure.Entities;
using NetTopologySuite.Geometries;

namespace Infrastructure.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, PostDto>()
                .ForMember(postDto => postDto.Longitude, opt => opt.MapFrom(post => post.Location.X))
                .ForMember(postDto => postDto.Latitude, opt => opt.MapFrom(post => post.Location.Y));
            CreateMap<PostDto, Post>()
                .ForMember(post => post.Location, opt => opt.MapFrom(post => new Point(post.Longitude, post.Latitude) { SRID = 4326 }));
        }
    }
}
