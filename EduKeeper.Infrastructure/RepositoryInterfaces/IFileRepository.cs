using EduKeeper.Entities;
using System;
using System.Collections.Generic;

namespace EduKeeper.Infrastructure.RepositoryInterfaces
{
    public interface IFileRepository : IRepository<File>
    {
        File Get(Guid identifier);

        List<File> GetFromPost(int postId);
    }
}
