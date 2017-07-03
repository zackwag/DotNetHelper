using System.Text.RegularExpressions;

namespace Helper.Validation
{
    public static class RegexHelper
    {
        internal static Regex ZipCode => new Regex(@"^\d{5}$|^\d{5}-\d{4}$");

        internal static Regex PhoneNumber => new Regex(@"^\(\d{3}\) \d{3}-\d{4}$");

        internal static Regex EmailAnywhere => new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", RegexOptions.IgnoreCase);

        #region Obsolete Methods
        //internal static Regex UrlAnywhere
        //{
        //    get
        //    {
        //        return new Regex(@"((?:https?://)|(?:www\.))([-\w]+(?:\.[-\w]+)*(?::\d+)?(?:/(?:(?:[~\w\+@%-]|(?:[,.;:][^\s$]))+)?)*(?:\?[\w\+@%&=.;-]+)?(?:\#[\w\-]*)?)([[:punct:]]|\s|<|$)");
        //    }
        //}

        //internal static Regex NumbersOnly
        //{
        //    get
        //    {
        //        return new Regex(@"^\d*$");
        //    }
        //}

        //internal static Regex IntOrFloat
        //{
        //    get
        //    {
        //        return new Regex(@"^\d*(\.\d+)?$");
        //    }
        //}

        //internal static Regex ShortDate
        //{
        //    get
        //    {
        //        return new Regex(@"^\d{2}\/\d{4}$");
        //    }
        //}

        //internal static Regex LongDate
        //{
        //    get
        //    {
        //        return new Regex(@"^\d{2}\/\d{2}\/\d{4}$");
        //    }
        //}

        //public static String RemoveAllNonAlphaNumericChars(this String value)
        //{
        //    var returnStr = String.Empty;

        //    if (!String.IsNullOrEmpty(value))
        //        returnStr = new String(Array.FindAll<char>(value.ToCharArray(), (c => (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-'))));

        //    return returnStr;
        //}

        /// <summary>
        /// Returns a Regex that checks to see if the year is in between 1700 and 2099
        /// Expression: ^(17|18|19|20)\d{2}$
        /// </summary>
        //public static Regex Year
        //{
        //    get
        //    {
        //        return new Regex(@"^(17|18|19|20)\d{2}$");
        //    }
        //}

        //public static Regex PasswordStrengthRequirements(Int32 charNum, Boolean includeCase, Boolean includeDigits, Boolean includeSymbols)
        //{
        //    String charRequirement = "(?=.{" + charNum + ",})";
        //    String caseRequirement = includeCase ? "(?=.*[a-z])(?=.*[A-Z])" : String.Empty;
        //    String digitSymbolRequirement = includeDigits || includeSymbols ? String.Format("(?=.*[{0}{1}])", includeDigits ? @"\d" : String.Empty, includeSymbols ? @"\W" : String.Empty) : String.Empty;

        //    return new Regex(@"^.*" + charRequirement + caseRequirement + digitSymbolRequirement + ".*$");
        //}

        //public static String RemoveInvalidFilenameCharacters(String filename)
        //{
        //    if (!String.IsNullOrEmpty(filename))
        //    {
        //        filename = filename.Replace(" ", "_");

        //        foreach (Char invalidChar in Path.GetInvalidFileNameChars())
        //        {
        //            filename = filename.Replace(invalidChar.ToString(), String.Empty);
        //        }

        //        foreach (Char invalidChar in Path.GetInvalidPathChars())
        //        {
        //            filename = filename.Replace(invalidChar.ToString(), String.Empty);
        //        }
        //    }

        //    return filename;
        //}

        //public static Regex FilePath
        //{
        //    get
        //    {
        //        return new Regex("^(([a-zA-Z]:|\\\\)\\\\)?(((\\.)|(\\.\\.)|([^\\\\/:\\*\\?\"\\|<>\\. ](([^\\\\/:\\*\\?\"\\|<>\\. ])|([^\\\\/:\\*\\?\"\\|<>]*[^\\\\/:\\*\\?\"\\|<>\\. ]))?))\\\\)*[^\\\\/:\\*\\?\"\\|<>\\. ](([^\\\\/:\\*\\?\"\\|<>\\. ])|([^\\\\/:\\*\\?\"\\|<>]*[^\\\\/:\\*\\?\"\\|<>\\. ]))?$");
        //    }
        //}

        //public static String Twitterify(String origStr)
        //{
        //    try
        //    {
        //        String finalStr = origStr;

        //        finalStr = Regex.Replace(finalStr, "[a-zA-Z]+://([.]?[a-zA-Z0-9_/-])*", delegate(Match match)
        //        {
        //            return String.Format("<a href=\"{0}\" title=\"{0}\" target=\"_blank\">{0}</a>", match);
        //        });

        //        finalStr = Regex.Replace(finalStr, "(^| )(www([.]?[a-zA-Z0-9_/-])*)", delegate(Match match)
        //        {
        //            return String.Format("<a href=\"{0}\" title=\"{0}\" target=\"_blank\">{0}</a>", match);
        //        });

        //        finalStr = Regex.Replace(finalStr, "@([A-Za-z0-9_]+)", delegate(Match match)
        //        {
        //            return String.Format("@<a href=\"http://twitter.com/{0}\" title=\"{0}\" target=\"_blank\">{0}</a>", match.Groups[1].Value);
        //        });

        //        finalStr = Regex.Replace(finalStr, "\\B#([^\\-\\+\\)\\(\\[\\]\\?\\=\\*\\}\\{\\:\\.\\;\\,\\\"\\'\\!\\<\\>\\|\\s\\~\\&\\§\\$\\%\\/\\\\\\\\µ#]{1,})", delegate(Match match)
        //        {
        //            return String.Format("<a href=\"http://twitter.com/search?q={0}\" title=\"#{0}\" target=\"_blank\">#{0}</a>", match.Groups[1].Value);
        //        });

        //        return finalStr;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("error on Twitterify, message: " + ex.Message);
        //    }
        //}

        //[Obsolete("Url is deprecated, please use ValidateData.Uri instead.")]
        //public static Regex Url
        //{
        //    get
        //    {
        //        return new Regex(@"^(?#Protocol)(?:(?:ht|f)tp(?:s?)\:\/\/|~/|/)?(?#Username:Password)(?:\w+:\w+@)?(?#Subdomains)(?:(?:[-\w]+\.)+(?#TopLevel Domains)(?:com|org|net|gov|edu|mil|biz|info|mobi|name|aero|jobs|museum|travel|priv|[a-z]{2}))(?#Port)(?::[\d]{1,5})?(?#Directories)(?:(?:(?:/(?:[-\w~!$+|.,=&]|%[a-f\d]{2})+)+|/)+|\?|#)?(?#Query)(?:(?:\?(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)(?:&(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)*)*(?#Anchor)(?:#(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)?$");
        //    }
        //}

        //[Obsolete("Email is deprecated, please use ValidateData.Email instead.")]
        //public static Regex Email
        //{
        //    get
        //    {
        //        return new Regex(@"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$", RegexOptions.IgnoreCase);
        //    }
        //}
        #endregion
    }
}
