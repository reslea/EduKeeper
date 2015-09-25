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
            Mapper.CreateMap<CourseModel, Course>();
            Mapper.CreateMap<Course, CourseModel>();

            Mapper.CreateMap<Post, PostDTO>()
                .ForMember(d => d.AuthorName, opt => opt
                    .MapFrom(s => s.Author.FirstName + " " + s.Author.LastName))
                .ForMember(d => d.AuthorId, opt => opt.MapFrom(s => s.Author.Id));

            Mapper.CreateMap<Comment, CommentDTO>()
                .ForMember(d => d.AuthorName, opt => opt
                    .MapFrom(s => s.Author.FirstName + " " + s.Author.LastName))

                .ForMember(d => d.AuthorId, opt => opt.MapFrom(s => s.Author.Id))
                .ForMember(d => d.Files, opt => opt.Ignore());
        }
    }
}