using System;

namespace EduKeeper.Infrastructure.ErrorUtilities
{
    public class ErrorDescriptionAttribute : Attribute
    {
        public string Text { get; set; }

        public ErrorDescriptionAttribute(string text)
        {
            Text = text;
        }
    }
}
