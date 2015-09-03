
namespace EduKeeper.Infrastructure
{
    public enum ErrorCase
    {
        [ErrorDescriptionAttribute("Owner not found")]
        UserNotFound,


        [ErrorDescriptionAttribute("Invalid email or password")]
        InvalidUserData,

        [ErrorDescriptionAttribute("Owner with this userEmail is already exist")]
        DuplicateEmail,

        [ErrorDescriptionAttribute("Unauthorized access")]
        UnauthorizedAccess,
    }
}
