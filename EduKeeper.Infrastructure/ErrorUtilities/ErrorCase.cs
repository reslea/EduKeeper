
namespace EduKeeper.Infrastructure.ErrorUtilities
{
    public enum ErrorCase
    {
        [ErrorDescriptionAttribute("User not found")]
        UserNotFound,


        [ErrorDescriptionAttribute("Invalid email or password")]
        InvalidUserData,

        [ErrorDescriptionAttribute("User with this userEmail is already exist")]
        DuplicateEmail,

        [ErrorDescriptionAttribute("Unauthorized access")]
        UnauthorizedAccess,

        [ErrorDescriptionAttribute("Course doesn`t exist")]
        CourseNotExist,

        [ErrorDescriptionAttribute("Ops, we have some errors here, please try again later")]
        UndefilenError,
    }
}
