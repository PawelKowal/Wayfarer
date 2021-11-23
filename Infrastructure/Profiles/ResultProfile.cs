using ApplicationCore.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Profiles
{
    public class ResultProfile : Profile
    {
        public ResultProfile()
        {
            CreateMap<IdentityResult, SimpleResultDto>()
                .ForCtorParam(ctorParamName: "IsSuccess", opt => opt.MapFrom(identityResult => identityResult.Succeeded))
                .ForCtorParam(ctorParamName: "ErrorMessages", opt => opt.MapFrom(identityResult => identityResult.Errors.Select(error => new KeyValuePair<string, string>(error.Code, error.Description))));
        }
    }
}
