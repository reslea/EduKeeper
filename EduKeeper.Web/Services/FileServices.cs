using EduKeeper.Infrastructure;
using EduKeeper.Infrastructure.DTO;
using EduKeeper.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduKeeper.Web.Services
{
    public class FileServices : IFileServices
    {
        private IDataAccess dataAccess;

        public FileServices(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }
        public FileDTO GetFile(Guid fileIdentifier)
        {
            int userId = SessionWrapper.Current.UserId;

            return dataAccess.GetFile(userId, fileIdentifier);
        }
    }
}