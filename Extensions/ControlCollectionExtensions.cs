using System.Collections;
using System.Web.UI;

namespace Helper.Extensions
{
    public static class ControlCollectionExtensions
    {
        public static IEnumerable All(this ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control.HasControls())
                {
                    foreach (Control child in control.Controls.All())
                    {
                        yield return child;
                    }
                }
                else
                {
                    yield return control;
                }
            }
        }
    }
}
