using AutoMapper;
using PCDH2.Core.Entities;
using PCDH2.Services.Models;
using System;

namespace PCDH2.API
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<ArticleModel, Article>()
                .ForMember(entity => entity.CreatedDate, act => act.MapFrom(model => DateTime.UtcNow))
                .ReverseMap();
        }
    }
}
