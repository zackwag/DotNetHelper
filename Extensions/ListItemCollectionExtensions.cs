using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Helper.Extensions
{
    public static class ListItemCollectionExtensions
    {
        public static void Add(this ListItemCollection value, ListItem item, string group)
        {
            item.Attributes["Group"] = group;

            value.Add(item);
        }

        public static IList<ListItem> SelectedItems(this ListItemCollection value)
        {
            return value.Cast<ListItem>().Where(li => li.Selected).ToList();
        }

        public static bool IsAnyItemsSelected(this ListItemCollection value)
        {
            return value.SelectedItems().HasItems();
        }
    }
}
