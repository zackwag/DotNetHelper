using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace Helper.Extensions
{
    public static class ListControlExtensions
    {
        public static ListItem FindItemByText(this ListControl control, string text, bool ignoreCase = true)
        {
            if (control.Items.HasItems())
                return (from i in control.Items.Cast<ListItem>()
                        where ignoreCase ? string.Equals(i.Text, text, StringComparison.OrdinalIgnoreCase) : i.Text == text
                        select i).FirstOrDefault();
            return default(ListItem);
        }

        public static int FindItemIndexByText(this ListControl control, string text, bool ignoreCase = true)
        {
            var item = FindItemByText(control, text, ignoreCase);

            return item.IsDefault() ? -1 : control.Items.IndexOf(item);
        }

        public static ListItem FindItemByValue(this ListControl control, string value, bool ignoreCase = true)
        {
            if (control.Items.HasItems())
                return (from i in control.Items.Cast<ListItem>()
                        where ignoreCase ? string.Equals(i.Value, value, StringComparison.OrdinalIgnoreCase) : i.Value == value
                        select i).FirstOrDefault();
            return default(ListItem);
        }

        public static int FindItemIndexByValue(this ListControl control, string value, bool ignoreCase = true)
        {
            var item = FindItemByValue(control, value, ignoreCase);

            return item.IsDefault() ? -1 : control.Items.IndexOf(item);
        }

        public static void SelectItem(this ListControl control, string value)
        {
            var item = FindItemByValue(control, value, true);

            if (item.IsDefault()) return;
            if (control.GetSelectionMode() == ListSelectionMode.Single)
                control.ClearSelection();

            item.Selected = true;
        }

        private static ListSelectionMode GetSelectionMode(this ListControl control)
        {
            var mode = ListSelectionMode.Single;

            CheckBoxList cbl;
            ListBox lb = null;

            if (!control.TryCast<CheckBoxList>(out cbl) && !control.TryCast<ListBox>(out lb)) return mode;

            if (cbl.HasValue())
                mode = ListSelectionMode.Multiple;
            else if (lb.HasValue())
                mode = lb.SelectionMode;

            return mode;
        }

        public static void DataBind(this ListControl control, object datasource, string textField, string valueField)
        {
            DataBind(control, datasource, textField, null, valueField);
        }

        public static void DataBind(this ListControl control, object datasource, string textField, string textFieldFormat, string valueField)
        {
            control.DataTextField = textField;
            control.DataValueField = valueField;

            if (!string.IsNullOrEmpty(textFieldFormat))
                control.DataTextFormatString = textFieldFormat;

            control.DataSource = datasource;
            control.DataBind();
        }

    }
}
