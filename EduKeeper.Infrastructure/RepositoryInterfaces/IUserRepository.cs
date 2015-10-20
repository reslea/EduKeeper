using EduKeeper.Entities;

namespace EduKeeper.Infrastructure.RepositoryInterfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User AuthentificateUser(string email, string password);
    }
}
