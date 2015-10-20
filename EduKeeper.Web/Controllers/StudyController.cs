using AutoMapper;
using EduKeeper.Infrastructure;
using EduKeeper.Infrastructure.DTO;
using EduKeeper.Infrastructure.ErrorUtilities;
using EduKeeper.Infrastructure.ServicesInretfaces;
using EduKeeper.Web.Attributes;
using EduKeeper.Web.Models;
using System;
using System.Web.Mvc;

namespace EduKeeper.Web.Controllers
{
    [UserAuthorization]
    public class StudyController : Controller
    {
        protected IUserContext UserContext { get; set; }

        protected ICommentService CommentService { get;set; }

        protected ICourseService CourseService { get;set; } 

        protected IFileService FileService { get;set; }

        protected IPostService PostService { get;set; }

        protected IUserService UserService { get; set; }
        
        public StudyController( IUserContext userContext,
                                ICommentService commentService,
                                ICourseService courseServices, 
                                IFileService fileService,
                                IPostService postService,
                                IUserService userService)        
        {
            UserContext = userContext;
            CommentService = commentService;
            CourseService = courseServices;
            FileService = fileService;
            PostService = postService;
            UserService = userService;
        }

        public ActionResult Courses(string searchTerm, int pageNumber = 1)
        {
            var courses = CourseService.GetCourses(searchTerm, pageNumber);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Courses", courses);
            }

            return View(courses);
        }

        public ActionResult AddCourse()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCourse(CourseDTO newCourse)
        {
            if (!ModelState.IsValid)
                return View();
            
            string title = newCourse.Title;
            string description = newCourse.Description;
            
            var course =CourseService.Add(title, description);
            return RedirectToAction("Course", new { courseId = course.Id });
        }

        public JsonResult AutocompleteCourse(string term)
        {
            var model = CourseService.Autocomplete(term);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPosts(int courseId, int pageNumber = 1)
        {
            var posts = PostService.GetLatestForCourse(courseId, pageNumber);

            if (posts == null)
                return null;

            var model = new PostCollectionModel()
            {
                Posts = posts,
                IsHasMore = posts.HasNextPage
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Course(int courseId)
        {
            var course = CourseService.Get(courseId);

            if (course == null)
                return RedirectToAction("Error", "Account", new { ErrorCase.CourseNotExist });

            return View(course);
        }

        public ActionResult JoinCourse(int courseId)
        {
            CourseService.Join(courseId);

            return View();
        }

        public ActionResult LeaveCourse(int courseId)
        {
            CourseService.Leave(courseId);

            return View();
        }

        [HttpPost]
        public ActionResult PostMessage(PostDTO post)
        {
            if (!ModelState.IsValid)
                return Json(null);

            PostDTO addedPost;
            try
            {
                addedPost = PostService.Add(post.CourseId, post.Message);

                if (addedPost != null && Request.Files.Count > 0)
                {
                    FileService.Attach(addedPost.Id, addedPost.CourseId, Request.Files);
                    addedPost = PostService.Get(addedPost.Id);
                }

            }
            catch (AccessViolationException)
            {
                return RedirectToAction("Error", "Account", new { ErrorCase.UnauthorizedAccess });
            }

            return Json(addedPost);
        }

        [HttpPost]
        public ActionResult PostComment(CommentDTO comment)
        {
            if (!ModelState.IsValid)
                return Json(null);

            var dto = CommentService.Add(comment.PostId, comment.Message);

            return Json(dto);
        }

        public PartialViewResult ViewLeftMenu()
        {
            var userId = UserContext.CurrentUserId.Value;

            var joinedCourses = CourseService.GetJoinedForUser(userId);

            var user = UserService.GetAuthentificated(userId);

            var model = new LeftMenuModel()
            {
                User = Mapper.Map<UserModel>(user),
                Courses = joinedCourses
            };

            return PartialView("_LeftMenu", model);
        }

        public JsonResult GetCourses()
        {
            int userId = UserContext.CurrentUserId.Value;

            var courses = CourseService.GetJoinedForUser(userId);

            return Json(courses, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetComments(int postId, int pageNumber = 1)
        {
            var comments = CommentService.GetPage(postId, pageNumber);

            if (comments == null)
                return null;

            var model = new 
            {
                comments,
                comments.HasNextPage
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult News()
        {
            return View();
        }

        public ActionResult GetNews(int pageNumber = 1)
        {
            var model = PostService.GetLatestForJoinedCourses(pageNumber);

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}