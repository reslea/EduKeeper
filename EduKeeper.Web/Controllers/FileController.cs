using EduKeeper.Web.Services.Interfaces;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EduKeeper.Web.Controllers
{
    public class FileController : Controller
    {
        private IFileServices fileServices;

        public FileController(IFileServices fileServices)
        {
            this.fileServices = fileServices;
        }

        public ActionResult DownloadFile(Guid fileIdentifier)
        {
            var file = fileServices.GetFile(fileIdentifier);
            return File(file.Path, System.Net.Mime.MediaTypeNames.Application.Octet, file.Name);
        }
    }
}
