using System.Text.RegularExpressions;

namespace Helper.IO.Ftp
{
    public class FtpListDetail
    {
        private const string PARSE_REGEX = @"^(?<dir>[\-ld])(?<permission>[\-rwx]{9})\s+(?<filecode>\d+)\s+(?<owner>\w+)\s+(?<group>\w+)\s+(?<size>\d+)\s+(?<month>\w{3})\s+(?<day>\d{1,2})\s+(?<timeyear>[\d:]{4,5})\s+(?<filename>(.*))$";

        public FtpListDetail()
        { }

        public FtpListDetail(string sourceText, string path = null)
        {
            var match = Regex.Match(sourceText, PARSE_REGEX);
            var currPath = "/";

            if (!string.IsNullOrEmpty(path))
                currPath = $"{(path.StartsWith("/") ? string.Empty : "/")}{path}{(path.EndsWith("/") ? string.Empty : "/")}";

            Dir = match.Groups["dir"].ToString();
            Permission = match.Groups["permission"].ToString();
            FileCode = match.Groups["filecode"].ToString();
            Owner = match.Groups["owner"].ToString();
            Group = match.Groups["group"].ToString();
            Name = match.Groups["filename"].ToString();
            FullPath = $"{currPath}{match.Groups["filename"]}";
        }

        internal string Dir { get; set; }
        public string Permission { get; set; }
        public string FileCode { get; set; }
        public string Owner { get; set; }
        public string Group { get; set; }
        public string Name { get; set; }
        public string FullPath { get; set; }

        public bool IsDirectory => !string.IsNullOrWhiteSpace(Dir) && Dir.ToLower().Equals("d");
    }
}
