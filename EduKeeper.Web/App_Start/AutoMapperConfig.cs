using AutoMapper;
using EduKeeper.Entities;
using EduKeeper.Infrastructure;
using EduKeeper.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

                .ForMember(d => d.AuthorId, opt => opt.MapFrom(s => s.Author.Id))
                //.ForMember(d => d.CourseId, opt => opt.MapFrom(p => p.Course.Id))
                //.ForMember(d => d.Message, opt => opt.MapFrom(p => p.Message))
                //.ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Comments, opt => opt.Ignore());

            Mapper.CreateMap<Comment, CommentDTO>()
                .ForMember(d => d.AuthorName, opt => opt
                    .MapFrom(s => s.Author.FirstName + " " + s.Author.LastName))

                .ForMember(d => d.AuthorId, opt => opt.MapFrom(s => s.Author.Id))
                .ForMember(d => d.Files, opt => opt.Ignore());
        }
    }
}