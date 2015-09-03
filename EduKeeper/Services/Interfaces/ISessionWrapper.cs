using EduKeeper.Entities;
using EduKeeper.Models;

namespace EduKeeper.Services.Interfaces
{
    public interface ISessionWrapper
    {
        UserModel User { get; set; }
    }
}