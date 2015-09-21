using EduKeeper.Infrastructure;
using EduKeeper.Web.Models;
using EduKeeper.Web.Services;
using EduKeeper.Web.Services.Interfaces;
using System.Web.Mvc;

namespace EduKeeper.Web.Controllers
{
    [UserAuthorization]
    public class StudyController : Controller
    {
        private ICourseServices courseServices;
        private IDataAccess dataAccess;

        public StudyController(ICourseServices courseServices, IDataAccess dataAccess)
        {
            this.courseServices = courseServices;
            this.dataAccess = dataAccess;
        }

        public ActionResult Courses(string searchTerm, int pageNumber = 1)
        {
            int userId = SessionWrapper.Current.UserId;

            var courses = dataAccess.GetCourses(userId, searchTerm, pageNumber);

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
        public ActionResult AddCourse(CourseModel model)
        {
            if (!ModelState.IsValid)
                return View();
            else
            {
                courseServices.AddCourse(model);
                return RedirectToAction("Course", new {courseId = model.Id});
            }
        }

        public JsonResult AutocompleteCourse(string term)
        {
            var model = dataAccess.AutocompleteCourse(term);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPosts(int courseId, int pageNumber = 1)
        {
            int userId = SessionWrapper.Current.UserId;
            string courseTitle = dataAccess.GetCourseTitle(courseId);

            var posts = dataAccess.GetPosts(userId, courseId, pageNumber);

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
            int userId = SessionWrapper.Current.UserId;

            var model = courseServices.GetCourse(courseId);
            if (model == null)
                return RedirectToAction("Error", "Account", new { ErrorCase.CourseNotExist });

            model.IsUserJoined = dataAccess.IsPartisipant(userId, courseId);
            
            SessionWrapper.Current.VisitedCourses.Add(courseId);

            return View(model);
        }

        public ActionResult JoinCourse(int courseId)
        {
            int userId = SessionWrapper.Current.UserId;

            courseServices.JoinCourse(courseId);
            return View();
        }

        public ActionResult LeaveCourse(int courseId)
        {
            int userId = SessionWrapper.Current.UserId;

            courseServices.LeaveCourse(courseId);
            return View();
        }

        [HttpPost]
        public JsonResult PostMessage(string message, int courseId, int pageNumber = 1)
        {
            if (string.IsNullOrWhiteSpace(message))
                return Json(null, JsonRequestBehavior.AllowGet);
            
            var post = courseServices.PostMessage(message, courseId);

            return Json(post, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PostComment(string message, int postId)
        {
            var comment = courseServices.PostComment(message, postId);

            return Json(comment, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult ViewLeftMenu()
        {
            var model = courseServices.GetLeftMenu();

            return PartialView("_LeftMenu", model);
        }

        public JsonResult GetCourses()
        {
            int userId = SessionWrapper.Current.UserId;
            var courses = dataAccess.GetJoinedCourses(userId);

            return Json(courses, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetComments(int postId, int pageNumber = 1)
        {
            int userId = SessionWrapper.Current.UserId;

            var comments = dataAccess.GetComments(userId, postId, pageNumber);

            if (comments == null)
                return null;

            var model = new 
            {
                comments,
                comments.HasNextPage
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}