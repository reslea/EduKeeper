using EduKeeper.Entities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduKeeper.EntityFramework.Interfaces
{
    interface IPostRepository : IRepository<Post>
    {
        IPagedList<Post> GetNotifications(int pageNumber);

        void AttachFiles(int postId, List<File> files);
    }
}
