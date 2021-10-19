using AutoMapper;
using LmsApi.Core.Dtos;
using LmsApi.Core.Entities;

namespace LmsApi.Core.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Literature, LiteratureDto>()
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject.Name))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level.Name));

            // When updating there should not be able to change includes, except authors or id to the others.
            CreateMap<LiteratureForCreateUpdateDto, Literature>();

            CreateMap<Literature, LiteratureForCreateUpdateDto>();
                //.ForMember(dest => dest.Subject.Name);

            CreateMap<Author, AuthorDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(
                    src => src.FirstName + " " + src.LastName))
                .ReverseMap()
                .ForMember(dest => dest.Literatures, opt => opt.Ignore());
        }
    }
}
