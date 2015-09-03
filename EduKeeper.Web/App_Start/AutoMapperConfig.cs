using AutoMapper;
using EduKeeper.Entities;
using EduKeeper.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduKeeper.App_Start
{
    public class AutoMapperConfig
    {
        public static void RegisterAutoMapper()
        {
            Mapper.CreateMap<UserModel, User>();
            Mapper.CreateMap<User, UserModel>();
            Mapper.CreateMap<CourseModel, Course>();
            Mapper.CreateMap<Course, CourseModel>();
        }
    }
}