using System.Web.UI;

namespace Helper.Extensions
{
    public static class ControlExtensions
    {
        public static Control FindControlRecursive(this Control value, string id)
        {
            if (value.IsNull())
                return null;

            //try to find the control at the current level
            var ctrl = value.FindControl(id);

            if (!ctrl.IsNull()) return ctrl;

            //search the children
            foreach (Control child in value.Controls)
            {
                ctrl = child.FindControlRecursive(id);

                if (ctrl.HasValue())
                    break;
            }

            return ctrl;
        }
    }
}
