using EduKeeper.Entities;

namespace EduKeeper.Infrastructure.ErrorUtilities
{

    public interface IErrorUtilities
    {
        string GetErrorDescriptionFromAttribute(ErrorCase errorCase);

        string GetRedirectionPage(ErrorCase errorCase);

        Error GetError(ErrorCase errorCase);

        Error LogError(ErrorCase errorCase);
    }
}