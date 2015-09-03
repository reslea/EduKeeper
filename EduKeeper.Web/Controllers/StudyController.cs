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

        public StudyController(ICourseServices courseServices)
        {
            this.courseServices = courseServices;
        }
        // GET: Study
        [AllowAnonymous]
        public ActionResult Courses()
        {
            var courses = courseServices.GetCourses();

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
    }
}