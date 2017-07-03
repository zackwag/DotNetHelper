using System;
using System.Text.RegularExpressions;

namespace Helper.Data.Vcard
{
    public class VCardHelper
    {
        private const string REGX_LINE = @"((?<strElement>[^\;^:]*) ((;(ENCODING=)?(?<strAttr>(QUOTED-PRINTABLE)))|(;CHARSET=UTF-?8)|(;[\w]*))*  (:(?<strValue> (([^\n\r]*=[\n\r]+)*[^\n\r]*[^=][\n\r]*) )))";
        private const string REGX_N = @"(?<strElement>(N)) ((;(ENCODING=)?(?<strAttr>(QUOTED-PRINTABLE)))|(;CHARSET=UTF-?8))* (:(?<strSurname>(([^;^\n\r]*(=\n\r)?)*[^;^\n\r]*[^;]*(\n\r)?) ))(;(?<strGivenName>(([^;^\n\r]*(=\n\r)?)*[^;^\n\r]*[^;]*(\n\r)?) ))? (;(?<strMidName>(([^;^\n\r]*(=\n\r)?)*[^;^\n\r]*[^;]*(\n\r)?) ))? (;(?<strPrefix>(([^;^\n\r]*(=\n\r)?)*[^;^\n\r]*[^;]*(\n\r)?) ))? (;(?<strSuffix>(([^;^\n\r]*(=\n\r)?)*[^;^\n\r]*[^;]*(\n\r)?) ))?";
        private const string REGX_FN = @"(?<strElement>(FN))((;(ENCODING=)?(?<strAttr>(QUOTED-PRINTABLE)))|(;CHARSET=UTF-?8))* (:(?<strFN>(([^\n\r]*=[\n\r]+)*[^\n\r]*[^=][\n\r]*) ))";
        private const string REGX_TITLE = @"(?<strElement>(TITLE))((;(ENCODING=)?(?<strAttr>(QUOTED-PRINTABLE)))|(;CHARSET=UTF-?8))* (:(?<strTITLE>[^\n\r]*))";
        private const string REGX_ORG = @"(?<strElement>(ORG)) ((;(ENCODING=)?(?<strAttr>(QUOTED-PRINTABLE)))|(;CHARSET=UTF-?8))*  (:(?<strORG>(([^;^\n\r]*(=\n\r)?)*[^;^\n\r]*[^;]*(\n\r)?) ))(;(?<strDept>(([^;^\n\r]*(=\n\r)?)*[^;^\n\r]*[^;]*(\n\r)?) ))?";
        private const string REGX_ADR = @"(?<strElement>(ADR))(;(?<strAttr>(HOME|WORK)))?((;(ENCODING=)?(?<strAttr>(QUOTED-PRINTABLE)))|(;CHARSET=UTF-?8))*(:(?<strPo>([^;]*)))(;(?<strBlock>([^;]*)))(;(?<strStreet>([^;]*)))(;(?<strCity>([^;]*)))(;(?<strRegion>([^;]*)))(;(?<strPostcode>([^;]*)))(;(?<strNation>(([^;^\n\r]*(=\n\r)?)*[^;^\n\r]*[^;]*(\n\r)?) ))?";
        private const string REGX_NOTE = @"((?<strElement>(NOTE)) ((;CHARSET=UTF-?8)|(;(ENCODING=)?(?<strAttr>(QUOTED-PRINTABLE))))*   (:(?<strValue> (([^\n\r]*=[\n\r]+)*[^\n\r]*[^=][\n\r]*) )))";
        private const string REGX_ROLE = @"(?<strElement>(ROLE)) ((;(ENCODING=)?(?<strAttr>(QUOTED-PRINTABLE)))|(;CHARSET=UTF-?8))*  (:(?<strROLE>(([^\n\r]*=[\n\r]+)*[^\n\r]*[^=][\n\r]*) ))";
        private const string REGX_BDAY = @"(?<strElement>(BDAY))   (:(?<strBDAY>[^\n\r]*))";
        private const string REGX_REV = @"(?<strElement>(REV)) (;CHARSET=utf-8)?  (:(?<strREV>[^\n\r]*))";
        private const string REGX_EMAIL = @"((?<strElement>(EMAIL)) ((;(?<strPref>(PREF))))* (;[^:]*)*  (:(?<strValue>[^\n\r]*)))";
        private const string REGX_TEL = @"((?<strElement>(TEL))  ((;(?<strType>(VOICE|CELL|PAGER|MSG|FAX)))| (;(?<strAttr>(HOME|WORK)))| (;(?<strPref>(PREF)))?)*  (:(?<strValue>[^\n\r]*)))";
        private const string REGX_URL = @"((?<strElement>(URL)) (;*(?<strAttr>(HOME|WORK)))?   (:(?<strValue>[^\n\r]*)))";

        /// <summary>
        /// Analyze vCard text into vCard properties.
        /// </summary>
        /// <param name="vCardText">vCard text.</param>
        /// <returns>vCard object.</returns>
        public static VCard ParseText(string vCardText)
        {
            var v = new VCard();
            var options = RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace;
            var regex = new Regex(REGX_LINE, options);

            foreach (Match match in regex.Matches(vCardText))
            {
                var matchValue = match.Value;
                var matchLine = regex.Match(matchValue);

                MatchCollection matches;
                switch (match.Groups["strElement"].Value)
                {
                    case "FN":
                        regex = new Regex(REGX_FN, options);

                        if (matchLine.Success)
                            v.FormattedName = matchLine.Groups["strAttr"].Value == "QUOTED-PRINTABLE" ? QuotedPrintable.Decode(matchLine.Groups["strFN"].Value) : matchLine.Groups["strFN"].Value;
                        break;
                    case "N":
                        regex = new Regex(REGX_N, options);

                        if (matchLine.Success)
                        {
                            v.Surname = matchLine.Groups["strAttr"].Value == "QUOTED-PRINTABLE" ? QuotedPrintable.Decode(matchLine.Groups["strSurname"].Value) : matchLine.Groups["strSurname"].Value;
                            v.GivenName = matchLine.Groups["strAttr"].Value == "QUOTED-PRINTABLE" ? QuotedPrintable.Decode(matchLine.Groups["strGivenName"].Value) : matchLine.Groups["strGivenName"].Value;
                            v.MiddleName = matchLine.Groups["strAttr"].Value == "QUOTED-PRINTABLE" ? QuotedPrintable.Decode(matchLine.Groups["strMidName"].Value) : matchLine.Groups["strMidName"].Value;
                            v.Prefix = matchLine.Groups["strAttr"].Value == "QUOTED-PRINTABLE" ? QuotedPrintable.Decode(matchLine.Groups["strPrefix"].Value) : matchLine.Groups["strPrefix"].Value;
                            v.Suffix = matchLine.Groups["strAttr"].Value == "QUOTED-PRINTABLE" ? QuotedPrintable.Decode(matchLine.Groups["strSuffix"].Value) : matchLine.Groups["strSuffix"].Value;
                        }
                        break;
                    case "TITLE":
                        regex = new Regex(REGX_TITLE, options);

                        if (matchLine.Success)
                            v.Title = matchLine.Groups["strAttr"].Value == "QUOTED-PRINTABLE" ? QuotedPrintable.Decode(matchLine.Groups["strTITLE"].Value) : matchLine.Groups["strTITLE"].Value;
                        break;
                    case "ORG":
                        regex = new Regex(REGX_ORG, options);

                        if (matchLine.Success)
                        {
                            v.Organization = matchLine.Groups["strAttr"].Value == "QUOTED-PRINTABLE" ? QuotedPrintable.Decode(matchLine.Groups["strORG"].Value) : v.Organization = matchLine.Groups["strORG"].Value;
                            v.Department = matchLine.Groups["strAttr"].Value == "QUOTED-PRINTABLE" ? QuotedPrintable.Decode(matchLine.Groups["strDept"].Value) : v.Department = matchLine.Groups["strDept"].Value;
                        }
                        break;
                    case "BDAY":
                        regex = new Regex(REGX_BDAY, options);

                        if (matchLine.Success)
                        {
                            var bdayStr = matchLine.Groups["strBDAY"].Value;

                            if (!string.IsNullOrEmpty(bdayStr))
                            {
                                string[] expectedFormats = { "yyyyMMdd", "yyMMdd", "yyyy-MM-dd" };
                                v.Birthday = DateTime.ParseExact(bdayStr, expectedFormats, null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                            }
                        }
                        break;
                    case "REV":
                        regex = new Regex(REGX_REV, options);

                        if (matchLine.Success)
                        {
                            var revStr = matchLine.Groups["strREV"].Value;
                            if (!string.IsNullOrEmpty(revStr))
                            {
                                string[] expectedFormats = { "yyyyMMddHHmmss", "yyyyMMddTHHmmssZ" };
                                v.Rev = DateTime.ParseExact(revStr, expectedFormats, null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                            }
                        }
                        break;
                    case "EMAIL":
                        regex = new Regex(REGX_EMAIL, options);

                        matches = regex.Matches(matchValue);
                        if (matches.Count > 0)
                        {
                            foreach (Match emailMatch in matches)
                            {
                                v.Emails.Add(new EmailAddress(emailMatch.Groups["strValue"].Value, emailMatch.Groups["strPref"].Value == "PREF"));
                            }
                        }
                        break;
                    case "TEL":
                        regex = new Regex(REGX_TEL, options);

                        matches = regex.Matches(matchValue);
                        if (matches.Count > 0)
                        {
                            foreach (Match phoneMatch in matches)
                            {
                                var number = phoneMatch.Groups["strValue"].Value;
                                var homeworkType = HomeWorkTypes.None;
                                var phoneType = PhoneNumber.PhoneTypes.None;
                                var preferred = phoneMatch.Groups["strPref"].Value == "PREF";
                                var types = matchLine.Groups["strType"].Captures;

                                switch (phoneMatch.Groups["strAttr"].Value)
                                {
                                    case "HOME":
                                        homeworkType = HomeWorkTypes.Home;
                                        break;
                                    case "WORK":
                                        homeworkType = HomeWorkTypes.Work;
                                        break;
                                }

                                foreach (Capture capture in types)
                                {
                                    switch (capture.Value)
                                    {
                                        case "VOICE":
                                            phoneType |= PhoneNumber.PhoneTypes.Voice;
                                            break;
                                        case "CELL":
                                            phoneType |= PhoneNumber.PhoneTypes.Cell;
                                            break;
                                        case "PAGER":
                                            phoneType |= PhoneNumber.PhoneTypes.Pager;
                                            break;
                                        case "MSG":
                                            phoneType |= PhoneNumber.PhoneTypes.Msg;
                                            break;
                                        case "FAX":
                                            phoneType |= PhoneNumber.PhoneTypes.Fax;
                                            break;
                                    }
                                }

                                v.Phones.Add(new PhoneNumber(number, homeworkType, phoneType, preferred));
                            }
                        }
                        break;
                    case "ADR":
                        regex = new Regex(REGX_ADR, options);

                        matches = regex.Matches(matchValue);
                        if (matches.Count > 0)
                        {
                            foreach (Match adrMatch in matches)
                            {
                                var address = new Address();

                                if (adrMatch.Groups["strAttr"].Value == "HOME")
                                    address.HomeWorkType = HomeWorkTypes.Home;
                                else if (adrMatch.Groups["strAttr"].Value == "WORK")
                                    address.HomeWorkType = HomeWorkTypes.Work;
                                else
                                    address.HomeWorkType = HomeWorkTypes.None;

                                address.PoBox = matchLine.Groups["strAttr"].Value == "QUOTED-PRINTABLE" ? QuotedPrintable.Decode(matchLine.Groups["strPo"].Value) : matchLine.Groups["strPo"].Value;
                                address.Ext = matchLine.Groups["strAttr"].Value == "QUOTED-PRINTABLE" ? QuotedPrintable.Decode(matchLine.Groups["strBlock"].Value) : matchLine.Groups["strBlock"].Value;
                                address.Street = matchLine.Groups["strAttr"].Value == "QUOTED-PRINTABLE" ? QuotedPrintable.Decode(matchLine.Groups["strStreet"].Value) : matchLine.Groups["strStreet"].Value;
                                address.Locality = matchLine.Groups["strAttr"].Value == "QUOTED-PRINTABLE" ? QuotedPrintable.Decode(matchLine.Groups["strCity"].Value) : matchLine.Groups["strCity"].Value;
                                address.Region = matchLine.Groups["strAttr"].Value == "QUOTED-PRINTABLE" ? QuotedPrintable.Decode(matchLine.Groups["strRegion"].Value) : matchLine.Groups["strRegion"].Value;
                                address.Postcode = matchLine.Groups["strAttr"].Value == "QUOTED-PRINTABLE" ? QuotedPrintable.Decode(matchLine.Groups["strPostcode"].Value) : matchLine.Groups["strPostcode"].Value;
                                address.Country = matchLine.Groups["strAttr"].Value == "QUOTED-PRINTABLE" ? QuotedPrintable.Decode(matchLine.Groups["strNation"].Value) : matchLine.Groups["strNation"].Value;

                                v.Addresses.Add(address);
                            }
                        }
                        break;
                    case "NOTE":
                        regex = new Regex(REGX_NOTE, options);

                        if (matchLine.Success)
                            v.Note = matchLine.Groups["strAttr"].Value == "QUOTED-PRINTABLE" ? QuotedPrintable.Decode(matchLine.Groups["strValue"].Value) : matchLine.Groups["strValue"].Value;
                        break;
                    case "URL":
                        regex = new Regex(REGX_URL, options);

                        matches = regex.Matches(matchValue);
                        if (matches.Count > 0)
                        {
                            foreach (Match urlMatch in matches)
                            {
                                var url = new Url {Address = urlMatch.Groups["strValue"].Value};

                                switch (urlMatch.Groups["strAttr"].Value)
                                {
                                    case "HOME":
                                        url.HomeWorkType = HomeWorkTypes.Home;
                                        break;
                                    case "WORK":
                                        url.HomeWorkType = HomeWorkTypes.Work;
                                        break;
                                }

                                v.Urls.Add(url);
                            }
                        }

                        break;
                    case "ROLE":
                        regex = new Regex(REGX_ROLE, options);

                        if (matchLine.Success)
                            v.Role = matchLine.Groups["strROLE"].Value;
                        break;
                }
            }

            return v;
        }
    }
}
