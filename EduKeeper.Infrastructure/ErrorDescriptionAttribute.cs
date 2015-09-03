using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduKeeper.Infrastructure
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
