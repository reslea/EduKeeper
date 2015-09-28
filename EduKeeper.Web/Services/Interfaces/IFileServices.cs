using EduKeeper.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduKeeper.Web.Services.Interfaces
{
    public interface IFileServices
    {
        FileDTO GetFile(Guid fileIdentifier);
    }
}
