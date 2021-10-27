using AutoMapper;
using LmsApi.Core.Dtos;
using LmsApi.Core.Entities;
using System.Linq;

namespace LmsApi.Core.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Literature, LiteratureDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject.Name))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level.Name));

            CreateMap<Literature, LiteratureAllDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject.Name))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level.Name));

            CreateMap<LiteratureForCreateDto, Literature>()
                .ForPath(dest => dest.Category.Id, opt => opt.MapFrom(src => src.CategoryId))
                .ForPath(dest => dest.Subject.Id, opt => opt.MapFrom(src => src.SubjectId))
                .ForPath(dest => dest.Level.Id, opt => opt.MapFrom(src => src.LevelId))
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Subject, opt => opt.Ignore())
                .ForMember(dest => dest.Level, opt => opt.Ignore());

            CreateMap<LiteratureForUpdateDto, Literature>()
                .ForPath(dest => dest.Category.Id, opt => opt.MapFrom(src => src.CategoryId))
                .ForPath(dest => dest.Subject.Id, opt => opt.MapFrom(src => src.SubjectId))
                .ForPath(dest => dest.Level.Id, opt => opt.MapFrom(src => src.LevelId))
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Subject, opt => opt.Ignore())
                .ForMember(dest => dest.Level, opt => opt.Ignore());

            CreateMap<Author, AuthorDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(
                            src => src.FirstName + " " + src.LastName));

            CreateMap<AuthorForCreateUpdateDto, Author>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Literatures, opt => opt.Ignore());


            CreateMap<Category, SelectListItemDto>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));

            CreateMap<Subject, SelectListItemDto>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));

            CreateMap<Level, SelectListItemDto>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));

        }
    }
}
