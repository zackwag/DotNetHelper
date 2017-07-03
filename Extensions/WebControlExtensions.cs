using Helper.Enumeration;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace Helper.Extensions
{
    public static class WebControlExtensions
    {
        public static void AddCssClass(this WebControl value, string cssClass)
        {
            // Ensure CSS class is defined
            if (string.IsNullOrEmpty(cssClass))
                return;

            // Append CSS class
            if (string.IsNullOrEmpty(value.CssClass))
            {
                // Set our CSS Class as only one
                value.CssClass = cssClass;
            }
            else
            {
                // Append new CSS class with space as separator
                value.CssClass += " " + cssClass;
            }
        }

        public static bool HasCssClass(this WebControl value, string cssClass)
        {
            return value.CssClass.Split(Convert.ToChar(ControlCharacters.Space)).Any(c => c.Equals(cssClass, StringComparison.OrdinalIgnoreCase));
        }
    }
}
