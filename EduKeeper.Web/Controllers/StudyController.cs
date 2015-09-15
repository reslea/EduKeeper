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
        // GET: Study
        [AllowAnonymous]
        public ActionResult Courses(string searchTerm, int pageNumber = 1)
        {
            int userId = SessionWrapper.Current.User != null ? 
                SessionWrapper.Current.User.Id : 0;

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
                return RedirectToAction("Courses");
            }
        }
        [AllowAnonymous]
        public JsonResult AutocompleteCourse(string term)
        {
            var model = dataAccess.AutocompleteCourse(term);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPosts(int courseId, int pageNumber = 1)
        {
            int userId = SessionWrapper.Current.User.Id;
            string courseTitle = dataAccess.GetCourseTitle(courseId);

            var posts = dataAccess.GetPosts(userId, courseId, pageNumber);

            var postCollection = new PostCollectionModel()
            {
                CourseId = courseId,
                Posts = posts,
                CourseTitle = courseTitle
            };
            return Json(postCollection.Posts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Course(int courseId)
        {
            var model = courseServices.GetCourse(courseId);
            if (model != null)
            {
                model.IsUserParticipant = SessionWrapper.Current.JoinedCourses.Contains(courseId);
                return View(model);
            }
            else
                return RedirectToAction("Error", "Account", new { ErrorCase.CourseNotExist });
        }

        public ActionResult JoinCourse(int courseId)
        {
            courseServices.JoinCourse(courseId);
            SessionWrapper.Current.JoinedCourses.Add(courseId);
            return View();
        }

        public ActionResult LeaveCourse(int courseId)
        {
            courseServices.LeaveCourse(courseId);
            SessionWrapper.Current.JoinedCourses.Remove(courseId);
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
    }
}