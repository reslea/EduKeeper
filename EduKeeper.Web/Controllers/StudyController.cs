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
            var courses = courseServices.GetCourses(searchTerm, pageNumber);

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
        public ActionResult AutocompleteCourse(string term)
        {
            var model = dataAccess.AutocompleteCourse(term);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPosts(int courseId, int pageNumber = 1)
        {
            int userId = SessionWrapper.Current.User.Id;
            string courseTitle = dataAccess.GetCourseTitle(courseId);

            var posts = new PostCollectionModel()
            {
                CourseId = courseId,
                Posts = dataAccess.GetPosts(userId, courseId, pageNumber),
                CourseTitle = courseTitle
            };
            return Json(posts.Posts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Course(int courseId)
        {
            var model = courseServices.GetCourse(courseId);
            if (model != null)
                return View(model);
            else
                return RedirectToAction("Error", "Account", new { ErrorCase.CourseNotExist });
        }

        public ActionResult JoinCourse(int courseId)
        {
            courseServices.JoinCourse(courseId);
            return View();
        }
        [HttpPost]
        public ActionResult PostMessage(string message, int courseId, int pageNumber = 1)
        {
            int userId = SessionWrapper.Current.User.Id;
            
            var post = courseServices.PostMessage(message, courseId);

            return Json(post, JsonRequestBehavior.AllowGet);
        }
    }
}