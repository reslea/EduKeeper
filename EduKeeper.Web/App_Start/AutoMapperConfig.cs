using AutoMapper;
using EduKeeper.Entities;
using EduKeeper.Infrastructure.DTO;
using EduKeeper.Web.Models;

namespace EduKeeper.Web
{
    public class AutoMapperConfig
    {
        public static void RegisterAutoMapper()
        {
            Mapper.CreateMap<UserModel, User>();
            Mapper.CreateMap<User, UserModel>();

            Mapper.CreateMap<User, UpdateUserModel>();
            Mapper.CreateMap<UpdateUserModel, User>();

            Mapper.CreateMap<CourseDTO, Course>();
            Mapper.CreateMap<Course, CourseDTO>();

            Mapper.CreateMap<Post, PostDTO>()
                .ForMember(d => d.AuthorName, opt => opt
                    .MapFrom(s => s.Author.FirstName + " " + s.Author.LastName))
                .ForMember(d => d.AuthorId, opt => opt.MapFrom(s => s.Author.Id))
                .ForMember(d => d.Comments, opt => opt.Ignore())
                .ForMember(d => d.Files, opt => opt.Ignore());

            Mapper.CreateMap<Comment, CommentDTO>()
                .ForMember(d => d.AuthorName, opt => opt
                    .MapFrom(s => s.Author.FirstName + " " + s.Author.LastName))

                .ForMember(d => d.AuthorId, opt => opt.MapFrom(s => s.Author.Id))
                .ForMember(d => d.Files, opt => opt.Ignore());

            Mapper.CreateMap<File, FileDTO>()
                .ForMember(d => d.Extention, 
                opt => opt.Ignore());
        }
    }
}