using EduKeeper.Entities;
using EduKeeper.Web.Models;

namespace EduKeeper.Web.Services.Interfaces
{
    public interface ISessionWrapper
    {
        int UserId { get; set; }
    }
}