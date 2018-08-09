using System;
using AutoMapper;
using Wiki.Core.Domain;
using Wiki.Infrastructure.DTO;

namespace Wiki.Infrastructure.Mappers
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<Agent, AgentDto>();
                cfg.CreateMap<Article, ArticleDto>();
                cfg.CreateMap<Article, ArticleDetailsDto>();
                cfg.CreateMap<Text, TextDto>();
                cfg.CreateMap<Text, TextDetailsDto>();
                cfg.CreateMap<TextTag, TextTagDto>();
                cfg.CreateMap<TextStatus, TextStatusDto>();
                cfg.CreateMap<ArticleCategory, ArticleCategoryDto>();
            })
            .CreateMapper();
        }
    }
}
