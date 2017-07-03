using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Helper.IO
{
    public static class FileExtensionGroup
    {
        public static IList<string> Archives => new ReadOnlyCollection<string>(new List<string> { "7z", "gz", "rar", "sitx", "zip" });

        public static IList<string> Documents => new ReadOnlyCollection<string>(new List<string> { "accdb", "doc", "docx", "dotx", "mdb", "pdf", "pps", "ppsx", "ppt", "pptx", "sldx", "xls", "xlsx" });

        public static IList<string> Images => new ReadOnlyCollection<string>(new List<string> { "fpx", "gif", "j2c", "j2k", "jfif", "jif", "jp2", "jpeg", "jpg", "jpx", "pcd", "pdf", "png", "tif", "tiff" });
    }
}
