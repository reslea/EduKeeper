﻿using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using EduKeeper.Entities;
using EduKeeper.Infrastructure;
using System.Collections.Generic;

namespace EduKeeper.EntityFramework
{
    public class DataAccess : IDataAccess
    {
        public bool RegistrateUser(User user)
        {
            using (var context = new EduKeeperContext())
            {
                user.RegDate = DateTime.Now;

                if (!context.Users.Any(u => u.Email == user.Email))
                {
                    context.Users.Add(user);
                }
                else
                {
                    return false;
                }
                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (DbUpdateException) { return false; }
            }
        }

        public User AuthenticateUser(string email, string password)
        {
            using (var context = new EduKeeperContext())
            {
                return context.Users
                        .SingleOrDefault(u => u.Email == email && u.Password == password);
            }
        }


        public User AuthenticateUser(string email)
        {
            if (String.IsNullOrEmpty(email))
                return null;

            using (var context = new EduKeeperContext())
            {
                return context.Users
                    .SingleOrDefault(u => u.Email == email);
            }
        }

        public User UpdateUserData(User user)
        {
            User result = null;

            using (var context = new EduKeeperContext())
            {
                result = context.Users.Find(user.Id);

                result.Age = user.Age;
                result.Email = user.Email;
                result.FirstName = user.FirstName;
                result.LastName = user.LastName;
                result.Sex = user.Sex; 

                context.SaveChanges();
            }

            return result;
        }

        public void LogError(Error error)
        {
            using (var context = new EduKeeperContext())
            {
                context.Errors.Add(error);
                context.SaveChanges();
            }
        }

        public List<Course> GetCourses()
        {
            using (var context = new EduKeeperContext())
            {
                return context.Courses.ToList();
            }
        }

        public void AddCourse(int ownerId, string title, string description)
        {
            using (var context = new EduKeeperContext())
            {
                User user = context.Users.SingleOrDefault(u => u.Id == ownerId);

                var users = new List<User>();
                users.Add(user);

                var course = new Course()
                {
                     Owner = user,
                     Title = title,
                     Description = description,
                     Users = users
                };

                context.Courses.Add(course);
                context.SaveChanges();
            }
        }

        public Course GetCourse(int courseId)
        {
            using (var context = new EduKeeperContext())
            {
                return context.Courses.SingleOrDefault(c => c.Id == courseId);
            }
        }

        public void JoinCourse(int courseId, int userId)
        {
            using (var context = new EduKeeperContext())
            {
                Course course = context.Courses.SingleOrDefault(c => c.Id == courseId);
                User user = context.Users.SingleOrDefault(u => u.Id == userId);
                course.Users.Add(user);
                user.Courses.Add(course);
                context.SaveChanges();
            }
        }

        public void LeaveCourse(int courseId, int userId)
        {
            using (var context = new EduKeeperContext())
            {
                Course course = context.Courses.SingleOrDefault(c => c.Id == courseId);
                User user = context.Users.SingleOrDefault(u => u.Id == userId);
                course.Users.Remove(user);
                user.Courses.Remove(course);
                context.SaveChanges();
            }
        }
    }
}