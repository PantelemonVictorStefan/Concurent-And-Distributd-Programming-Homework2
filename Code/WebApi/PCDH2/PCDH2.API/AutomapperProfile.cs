using AutoMapper;
using PCDH2.Core.Entities;
using PCDH2.Services.Models;
using Presentation.PCDH2.API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCDH2.API
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Article, ArticleModel>().ReverseMap();
            CreateMap<Test, TestDTO>().ReverseMap();
        }
    }
}
