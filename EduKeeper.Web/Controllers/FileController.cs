using EduKeeper.Infrastructure.ServicesInretfaces;
using EduKeeper.Web.Attributes;
using System;
//using System.Net.Mime;
using System.Web.Mvc;

namespace EduKeeper.Web.Controllers
{
    [UserAuthorization] 
    public class FileController : Controller
    {
        protected IFileService FileService { get; set; }

        public FileController(IFileService fileService)
        {
            FileService = fileService;
        }

        [AllowAnonymous]
        public ActionResult DownloadFile(Guid fileIdentifier)
        {
            var file = FileService.Get(fileIdentifier);

            return File(file.Path, System.Net.Mime.MediaTypeNames.Application.Octet, file.Name);
        }
    }
}
