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

        public ActionResult Course(int courseId)
        {
            var model = courseServices.GetCourse(courseId);
            return View(model);
        }

        public ActionResult JoinCourse(int courseId)
        {
            courseServices.JoinCourse(courseId);
            return View();
        }
    }
}