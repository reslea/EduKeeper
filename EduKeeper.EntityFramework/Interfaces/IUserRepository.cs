using EduKeeper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduKeeper.EntityFramework.Interfaces
{
    interface IUserRepository : IRepository<User>
    {
        User AuthentificateUser(string email, string password);

        int? GetAuthentificatedId(string email);
    }
}
