using Helper.Enumeration;
using Helper.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Helper.Extensions
{
    public static class StringExtensions
    {
        #region Private Members

        private static Dictionary<char, string> Replacements => new Dictionary<char, string>
        {
            {'’', "'"},
            {'–', "-"},
            {'‘', "'"},
            {'”', "\""},
            {'“', "\""},
            {'…', "..."},
            {'£', "GBP"},
            {'•', "*"},
            {' ', " "},
            {'é', "e"},
            {'ï', "i"},
            {'´', "'"},
            {'—', "-"},
            {'·', "*"},
            {'„', "\""},
            {'€', "EUR"},
            {'®', "(R)"},
            {'¹', "(1)"},
            {'«', "\""},
            {'è', "e"},
            {'á', "a"},
            {'™', "TM"},
            {'»', "\""},
            {'ç', "c"},
            {'½', "1/2"},
            {'­', "-"},
            {'°', " degrees "},
            {'ä', "a"},
            {'É', "E"},
            {'‚', ","},
            {'ü', "u"},
            {'í', "i"},
            {'ë', "e"},
            {'ö', "o"},
            {'à', "a"},
            {'¬', " "},
            {'ó', "o"},
            {'â', "a"},
            {'ñ', "n"},
            {'ô', "o"},
            {'¨', ""},
            {'å', "a"},
            {'ã', "a"},
            {'ˆ', ""},
            {'©', "(c)"},
            {'Ä', "A"},
            {'Ï', "I"},
            {'ò', "o"},
            {'ê', "e"},
            {'î', "i"},
            {'Ü', "U"},
            {'Á', "A"},
            {'ß', "ss"},
            {'¾', "3/4"},
            {'È', "E"},
            {'¼', "1/4"},
            {'†', "+"},
            {'³', "'"},
            {'²', "'"},
            {'Ø', "O"},
            {'¸', ","},
            {'Ë', "E"},
            {'ú', "u"},
            {'Ö', "O"},
            {'û', "u"},
            {'Ú', "U"},
            {'Œ', "Oe"},
            {'º', "?"},
            {'‰', "0/00"},
            {'Å', "A"},
            {'ø', "o"},
            {'˜', "~"},
            {'æ', "ae"},
            {'ù', "u"},
            {'‹', "<"},
            {'±', "+/-"}
        };

        #endregion

        #region 'Is' Methods

        public static bool IsAllUpper(this string value)
        {
            try
            {
                return Regex.Split(value, @"\W|_").All(w => w.Equals(w.ToUpper()));
            }
            catch (Exception ex)
            {
                throw new Exception("error of IsAllUpper method, message: " + ex.Message);
            }
        }

        public static bool IsAllLower(this string value)
        {
            try
            {
                return Regex.Split(value, @"\W|_").All(w => w.Equals(w.ToLower()));
            }
            catch (Exception ex)
            {
                throw new Exception("error of IsAllLower method, message: " + ex.Message);
            }
        }

        public static bool IsHtmlTag(this string value)
        {
            return Regex.IsMatch(value, @"<([A-Z][A-Z0-9]*)\b[^>]*>(.*?)</\1>",
                RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant |
                RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
        }

        private static bool IsPossibleUri(this string value)
        {
            const string pattern = @"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[\-;:&=\+\$,\w]+@)?[A-Za-z0-9\.\-]+|(?:www\.|[\-;:&=\+\$,\w]+@)[A-Za-z0-9\.\-]+)((?:\/[\+~%\/\.\w\-]*)?\??(?:[\-\+=&;%@\.\w]*)#?(?:[\.\!\/\\\w]*))?)";
            return Regex.IsMatch(value, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        #endregion

        #region 'Remove' Methods

        public static string RemoveUrls(this string value)
        {
            return RegexHelper.EmailAnywhere.Replace(value, string.Empty);
        }

        public static string RemoveEmails(this string value)
        {
            return RegexHelper.EmailAnywhere.Replace(value, string.Empty);
        }

        public static string RemoveNonDigits(this string value)
        {
            return new Regex(@"\D").Replace(value, string.Empty);
        }

        public static string RemoveMarkup(this string value)
        {
            try
            {
                return Regex.Replace(value, @"<[^>]*>", string.Empty).Replace("&nbsp;", " ").Replace("&ndash;", "–");
            }
            catch (Exception ex)
            {
                throw new Exception("error on RemoveMarkup method, message: " + ex.Message);
            }
        }

        public static string RemoveMarkupObject(this string value, HtmlTextWriterTag tagName)
        {
            try
            {
                return value.RemoveMarkupObject(tagName.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("error on RemoveMarkupTag method, message: " + ex.Message);
            }
        }

        public static string RemoveMarkupObject(this string value, string tagName)
        {
            try
            {
                return Regex.Replace(value.Replace(Environment.NewLine, string.Empty), string.Format(@"<{0}\s*(.*?)>(.*?)</{0}>", tagName.ToLower()), string.Empty);
            }
            catch (Exception ex)
            {
                throw new Exception("error on RemoveMarkupTag method, message: " + ex.Message);
            }
        }

        public static string RemoveAccent(this string value)
        {
            return Encoding.ASCII.GetString(Encoding.GetEncoding("Cyrillic").GetBytes(value));
        }

        public static string ReplaceNewlines(this string value, string replacement = "<br />")
        {
            return Regex.Replace(ValidateData.String(value).ProcessedValue, @"\r\n|\n\r|\n|\r", replacement);
        }

        public static string ReplaceBreaklines(this string value)
        {
            return ReplaceBreaklines(value, Environment.NewLine);
        }

        public static string ReplaceBreaklines(this string value, string replacement)
        {
            return Regex.Replace(ValidateData.String(value).ProcessedValue, @"<br\s?/?>", replacement);
        }

        public static string ReplaceMarkupTag(this string value, HtmlTextWriterTag sourceTagName, HtmlTextWriterTag destTagName)
        {
            try
            {
                return value.ReplaceMarkupTag(sourceTagName.ToString(), destTagName.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("error on RemoveHTML method, message: " + ex.Message);
            }
        }

        public static string ReplaceMarkupTag(this string value, string sourceTagName, string destTagName)
        {
            try
            {
                var tagRegex = new Regex($"<{sourceTagName.ToLower()}\\s*(.*?)>");

                if (tagRegex.IsMatch(value))
                {
                    value = tagRegex.Replace(value, $"<{destTagName.ToLower()}>")
                                    .Replace($"</{sourceTagName.ToLower()}>", $"</{destTagName.ToLower()}>");
                }

                return value;
            }
            catch (Exception ex)
            {
                throw new Exception("error on ReplaceMarkupTag method, message: " + ex.Message);
            }
        }

        #endregion

        #region 'To' Methods
        public static string ToAscii(this string value)
        {
            return string.Join(string.Empty, value.Select(Asciify).ToArray());
        }

        private static string Asciify(char x)
        {
            return Replacements.ContainsKey(x) ? Replacements[x] : x.ToString();
        }

        public static string Encode(this string value, Encoding encoding)
        {
            return encoding.GetString(Encoding.Default.GetBytes(value));
        }

        public static string ToCapitalized(this string value)
        {
            try
            {
                var returnStr = string.Empty;

                if (string.IsNullOrEmpty(value)) return returnStr;
                foreach (var token in value.Split(Convert.ToChar(ControlCharacters.Space)))
                {
                    if (!string.IsNullOrEmpty(returnStr))
                        returnStr += " ";

                    returnStr += $"{token[0].ToString().ToUpper()}{token.Substring(1).ToLower()}";
                }

                return returnStr;
            }
            catch (Exception ex)
            {
                throw new Exception("error of ToCapitalized method, message: " + ex.Message);
            }
        }

        public static string ToCamelcase(this string value)
        {
            try
            {
                var returnStr = string.Empty;

                return string.IsNullOrEmpty(value) ? returnStr : value.Split(Convert.ToChar(ControlCharacters.Space)).Aggregate(returnStr, (current, token) => current + $"{token[0].ToString().ToUpper()}{token.Substring(1).ToLower()}");
            }
            catch (Exception ex)
            {
                throw new Exception("error of Camelcase method, message: " + ex.Message);
            }
        }

        public static string ToHtmlParagraph(this string value)
        {
            try
            {
                return string.IsNullOrEmpty(value) ? string.Empty : $"<p>{Regex.Replace(value, "[ \\t\\r\\f\\v]*\\n[ \\t\\r\\f\\v]*\\n[ \\t\\r\\f\\v]*", "</p><linebreak><p>").ReplaceNewlines().Replace("<linebreak>", Environment.NewLine)}</p>";
            }
            catch (Exception ex)
            {
                throw new Exception("error on ToHtmlParagraph, message: " + ex.Message);
            }
        }

        public static string ToPlainParagraph(this string value)
        {
            try
            {
                var returnStr = string.Empty;

                if (string.IsNullOrEmpty(value)) return returnStr;
                returnStr = value.Replace(Environment.NewLine, string.Empty);

                if (returnStr.StartsWith("<p>"))
                    returnStr = returnStr.Remove(returnStr.IndexOf("<p>", StringComparison.Ordinal), "<p>".Length);

                if (returnStr.EndsWith("</p>"))
                    returnStr = returnStr.Remove(returnStr.LastIndexOf("</p>", StringComparison.Ordinal), "</p>".Length);

                returnStr = returnStr.ReplaceBreaklines()
                                     .Replace("<p>", Environment.NewLine)
                                     .Replace("</p>", Environment.NewLine)
                                     .RemoveMarkup();

                return returnStr;
            }
            catch (Exception ex)
            {
                throw new Exception("error on ToPlainParagraph, message: " + ex.Message);
            }
        }

        //public static string ToUrlFileEncoded(this string value)
        //{
        //    return HttpUtility.UrlEncode(HttpUtility.UrlPathEncode(value)).Replace("%25", "%");
        //}

        public static string ToUrlSlug(this string value, bool removeStopWords = false, char delimiter = '-')
        {
            return value.ToUrlSlug(default(string[]), removeStopWords, delimiter);
        }

        public static string ToUrlSlug(this string value, string replace, bool removeStopWords = false, char delimiter = '-')
        {
            return value.ToUrlSlug(new[] {replace}, removeStopWords, delimiter);
        }

        public static string ToUrlSlug(this string value, string[] replace, bool removeStopWords = false, char delimiter = '-')
        {
            if (string.IsNullOrEmpty(value)) return value;
            if (replace.HasItems())
            {
                value = replace.Where(r => !string.IsNullOrEmpty(r)).Aggregate(value, (current, token) => current.Replace(token, " "));
            }

            value = Regex.Replace(Regex.Replace(value.RemoveAccent(), @"[^a-zA-Z0-9\/_|+ -]", string.Empty).Trim(delimiter).ToLower(), @"[\/_|+ -]+", delimiter.ToString());

            if (!removeStopWords) return value;
            var stopWords = StringHelper.StopWords;

            value = string.Join(delimiter.ToString(),
                value.Split(delimiter).Where(t => !t.In(stopWords.ToArray())).ToArray());

            return value;
        }
        #endregion

        #region 'Get' Methods
        public static int GetWordCount(this string value)
        {
            return SplitWords(value).Count;
        }

        public static string GetFileName(this string value)
        {
            if (value.Contains("\\"))
                value = value.Substring(value.LastIndexOf('\\') + 1);

            if (!string.IsNullOrEmpty(value.GetFileExtension()))
                value = value.Substring(0, value.LastIndexOf('.'));

            return value;
        }

        public static string GetFileExtension(this string value)
        {
            return value.LastIndexOf('.') != -1 ? value.Substring(value.LastIndexOf('.') + 1) : string.Empty;
        }

        public static List<HtmlGenericControl> GetHtmlObjects(this string value, HtmlTextWriterTag tagName)
        {
            try
            {
                return value.GetHtmlObjects(tagName.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("error on TagInnerHtml method, message: " + ex.Message);
            }
        }

        public static List<HtmlGenericControl> GetHtmlObjects(this string value, string tagName)
        {
            try
            {
                var rx = new Regex(string.Format(@"<{0}\s*(.*?)>(.*?)</{0}>", tagName.ToLower()), RegexOptions.IgnoreCase);

                return rx.Matches(value.Replace(Environment.NewLine, string.Empty)).Cast<Match>().Select(m => new HtmlGenericControl(tagName.ToLower()) {InnerHtml = m.Groups[2].Value}).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetHtmlObjects method, message: " + ex.Message);
            }
        }

        public static T GetEnumByAttribute<T>(this string value, string property, bool ignoreCase = true)
        {
            var type = typeof(T);

            if (!type.IsEnum)
                throw new InvalidOperationException();

            var enumVal = (from e in Enum.GetValues(type).Cast<Enum>()
                let a = e.GetAttributeOfType<EnumAttribute>()
                let p = a.GetPropertyDictionary()
                where
                p.ContainsKey(property) &&
                (ignoreCase ? p[property].ToString().ToLower() == value.ToLower() : p[property].ToString() == value)
                select e).FirstOrDefault();

            return enumVal.IsDefault() ? default(T) : (T) Convert.ChangeType(enumVal, type);
        }

        #endregion

        public static string SafeTrim(this string value)
        {
            return (value ?? string.Empty).Trim();
        }

        public static bool Contains(this string thisValue, string value, StringComparison comparisonType)
        {
            return thisValue.HasValue() && value.HasValue() && thisValue.IndexOf(value, comparisonType) != -1;
        }

        public static bool StartsWithBlockElement(this string value)
        {
            try
            {
                return Regex.IsMatch(value, "^<(div|h\\d|hr|ol|p|table|ul)");
            }
            catch (Exception ex)
            {
                throw new Exception("error on StartsWithBlockElement method, message: " + ex.Message);
            }
        }

        public static string Truncate(this string value, int maxLength, string suffix = "...")
        {
            if (string.IsNullOrEmpty(suffix))
                suffix = string.Empty;

            var truncatedString = value;

            if (maxLength <= 0)
                return truncatedString;

            var strLength = maxLength - suffix.Length;

            if (strLength <= 0)
                return truncatedString;

            if (value.IsNull() || value.Length <= maxLength)
                return truncatedString;

            truncatedString = value.Substring(0, strLength);
            truncatedString = truncatedString.TrimEnd();
            truncatedString += suffix;

            return truncatedString;
        }

        public static bool TryParse<T>(this string s, out T result)
        {
            if (typeof(T) == typeof(MailAddress))
            {
                try
                {
                    T test;

                    new MailAddress(ValidateData.CleanInput(s)).TryCast(out test);

                    result = test;
                    return true;
                }
                catch
                {
                    result = default(T);
                    return false;
                }
            }
            var converter = TypeDescriptor.GetConverter(typeof(T));

            try
            {
                result = (T) converter.ConvertFromString(s);
                return true;
            }
            catch
            {
                result = default(T);
                return false;
            }
        }

        public static List<string> SplitWords(this string origStr, bool includeWhiteSpace = false)
        {
            var values = new List<string>();
            var pos = 0;

            foreach (var match in Regex.Matches(origStr, @"\s+").Cast<Match>())
            {
                values.Add(origStr.Substring(pos, match.Index - pos));

                if (includeWhiteSpace)
                    values.Add(match.Value);

                pos = match.Index + match.Length;
            }

            values.Add(origStr.Substring(pos));

            return values;
        }

        public static string AutoHyperlink(this string value, string target = null, int? maxLinkLength = null)
        {
            var result = ValidateData.String(value);
            var resultHtml = result.ProcessedValue;

            if (!result.Success) return resultHtml;
            var lines = Regex.Split(value, Environment.NewLine);

            for (var i = 0; i < lines.Length; i++)
            {
                var words = SplitWords(lines[i], true);

                for (var j = 0; j < words.Count; j++)
                {
                    var word = words[j];
                    var puncIndex = word.Length;

                    for (var k = puncIndex - 1; k > 0; k--)
                    {
                        if (!char.IsPunctuation(word[k]) || word[k] == '/')
                            break;

                        puncIndex--;
                    }

                    var subWord = word.Substring(0, puncIndex);
                    var subPunc = puncIndex != word.Length ? word.Substring(puncIndex, word.Length - puncIndex) : string.Empty;

                    if (!IsHtmlTag(subWord))
                    {
                        result = ValidateData.Email(subWord);

                        if (result.Success)
                        {
                            var email = result.ProcessedValue;

                            subWord = $"<a href=\"mailto:{email}\">{(maxLinkLength.HasValue ? email.Truncate(maxLinkLength.Value) : email)}</a>";
                        }
                        else
                        {
                            if (subWord.IsPossibleUri())
                            {
                                result = ValidateData.Uri(subWord);

                                if (result.Success)
                                {
                                    var uri = new Uri(result.ProcessedValue);
                                    var href = result.ProcessedValue;
                                    var text = uri.Authority + uri.PathAndQuery;

                                    if (text.EndsWith("/"))
                                        text = text.Substring(0, text.Length - 1);

                                    subWord = $"<a href=\"{href}\"{(string.IsNullOrEmpty(target) ? string.Empty : $" target=\"{target}\"")}>{(maxLinkLength.HasValue ? text.Truncate(maxLinkLength.Value) : text)}</a>";
                                }
                            }
                        }
                    }

                    words[j] = $"{subWord}{subPunc}";
                }

                lines[i] = string.Join(string.Empty, words);
            }

            resultHtml = string.Join(Environment.NewLine, lines);

            return resultHtml;
        }
    }
}
