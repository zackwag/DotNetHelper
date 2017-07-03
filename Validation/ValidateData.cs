using Helper.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Helper.Validation
{
    public static class ValidateData
    {
        public static ValidationResult Decimal(string value)
        {
            try
            {
                decimal test;

                var number = GetTestValue(value);

                return String(number).Success ? GenerateResult(value.TryParse(out test), number) : GenerateResult(false, number);
            }
            catch (Exception ex)
            {
                throw new Exception("error on Decimal method, message: " + ex.Message);
            }
        }

        public static ValidationResult Integer(string value)
        {
            try
            {
                int test;

                var number = GetTestValue(value);

                return String(number).Success ? GenerateResult(number.TryParse(out test), number) : GenerateResult(false, number);
            }
            catch (Exception ex)
            {
                throw new Exception("error on Integer method, message: " + ex.Message);
            }
        }

        public static ValidationResult DateTime(string value)
        {
            try
            {
                DateTime test;

                var date = GetTestValue(value);

                return String(date).Success ? GenerateResult(date.TryParse(out test), date) : GenerateResult(false, date);
            }
            catch (Exception ex)
            {
                throw new Exception("error on DateTime method, message: " + ex.Message);
            }
        }

        public static ValidationResult Boolean(string value)
        {
            try
            {
                bool test;

                var boolean = GetTestValue(value);

                return String(boolean).Success ? GenerateResult(boolean.TryParse(out test), boolean) : GenerateResult(false, boolean);
            }
            catch (Exception ex)
            {
                throw new Exception("error on DateTime method, message: " + ex.Message);
            }
        }

        public static ValidationResult String(string value, bool allowNullOrEmpty = false)
        {
            try
            {
                var testValue = GetTestValue(value);

                return GenerateResult(allowNullOrEmpty || !string.IsNullOrEmpty(testValue), testValue);
            }
            catch (Exception ex)
            {
                throw new Exception("error on String method, message: " + ex.Message);
            }
        }

        public static ValidationResult Email(string value)
        {
            try
            {
                var email = GetTestValue(value);

                MailAddress test;

                return GenerateResult(value.TryParse(out test), email);
            }
            catch (Exception ex)
            {
                throw new Exception("error on Email method, message: " + ex.Message);
            }
        }

        public static ValidationResult FileType(string value, IList<string> acceptableTypes)
        {
            try
            {
                var ext = GetTestValue(value);

                return !String(ext).Success ? GenerateResult(false, ext) : GenerateResult(!acceptableTypes.HasItems() || acceptableTypes.Contains(value.ToLower()), ext);
            }
            catch (Exception ex)
            {
                throw new Exception("error on FileType method, message: " + ex.Message);
            }
        }

        public static ValidationResult PhoneNumber(string value)
        {
            try
            {
                var testValue = GetTestValue(value);
                var phone = string.Empty;

                if (!String(testValue).Success) return GenerateResult(false, testValue, phone);

                if (RegexHelper.PhoneNumber.IsMatch(testValue))
                {
                    phone = testValue;
                }
                else
                {
                    if (testValue.RemoveNonDigits().Length != 10) return GenerateResult(false, testValue, phone);

                    phone = $"({testValue.RemoveNonDigits().Substring(0, 3)}) {testValue.RemoveNonDigits().Substring(3, 3)}-{testValue.RemoveNonDigits().Substring(6, 4)}";
                }

                return GenerateResult(true, testValue, phone);
            }
            catch (Exception ex)
            {
                throw new Exception("error on PhoneNumber method, message: " + ex.Message);
            }
        }

        public static ValidationResult Uri(string value, bool allowRelativeUri = true)
        {
            try
            {
                var testValue = GetTestValue(value);
                var url = GetTestValue(null);
                var success = false;

                if (!String(value).Success) return GenerateResult(false, testValue, url);

                Uri myUri;

                if (System.Uri.TryCreate(testValue, UriKind.Absolute, out myUri))
                {
                    url = myUri.AbsoluteUri;
                    success = true;
                }
                else if (Email(testValue).Success)
                {
                    url = $"mailto:{Email(testValue).ProcessedValue}";
                    success = true;
                }
                else if (allowRelativeUri && System.Uri.TryCreate(testValue, UriKind.Relative, out myUri))
                {
                    var result = Uri($"http://{testValue}");

                    if (!result.Success) return GenerateResult(false, testValue, url);

                    url = result.ProcessedValue;
                    success = true;
                }

                return GenerateResult(success, testValue, url);
            }
            catch (Exception ex)
            {
                throw new Exception("error on Uri method, message: " + ex.Message);
            }
        }

        public static ValidationResult Year(string value)
        {
            try
            {
                var year = GetTestValue(value);

                DateTime test;

                return GenerateResult(Integer(year).Success && year.Length == 4 && ("01/01/" + year).TryParse(out test), year);
            }
            catch (Exception ex)
            {
                throw new Exception("error on Year method, message: " + ex.Message);
            }
        }

        public static ValidationResult ZipCode(string value)
        {
            try
            {
                var testValue = GetTestValue(value);
                var zip = GetTestValue(null);
                var success = false;

                if (!String(testValue).Success) return GenerateResult(false, testValue, zip);

                if (RegexHelper.ZipCode.IsMatch(testValue))
                {
                    zip = testValue;
                    success = true;
                }
                else
                {
                    if (testValue.RemoveNonDigits().Length == 9)
                    {
                        zip = $"{testValue.RemoveNonDigits().Substring(0, 5)}-{testValue.RemoveNonDigits().Substring(5, 4)}";
                        success = true;
                    }
                    else if (testValue.RemoveNonDigits().Length == 5)
                    {
                        zip = testValue.RemoveNonDigits();
                        success = true;
                    }
                }

                return GenerateResult(success, testValue, zip);
            }
            catch (Exception ex)
            {
                throw new Exception("error on ZipCode method, message: " + ex.Message);
            }
        }

        #region Helper Methods
        private static string GetTestValue(string value)
        {
            try
            {
                return (string.IsNullOrEmpty(value) ? string.Empty : value).SafeTrim();
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetTestValue method, message: " + ex.Message);
            }
        }

        private static ValidationResult GenerateResult(bool success, string preVal)
        {
            try
            {
                return GenerateResult(success, preVal, success ? preVal : GetTestValue(null));
            }
            catch (Exception ex)
            {
                throw new Exception("error on GenerateResult method, message: " + ex.Message);
            }
        }

        private static ValidationResult GenerateResult(bool success, string preVal, string postVal)
        {
            try
            {
                return new ValidationResult
                {
                    Success = success,
                    OriginalValue = preVal,
                    ProcessedValue = success ? GetTestValue(postVal) : GetTestValue(null)
                };
            }
            catch (Exception ex)
            {
                throw new Exception("error on GenerateResult method, message: " + ex.Message);
            }
        }

        internal static string CleanInput(string origStr)
        {
            try
            {
                return Regex.Replace(origStr, @"[^\w\.@-]", string.Empty);
            }
            catch (Exception ex)
            {
                throw new Exception("error on CleanInput method, message: " + ex.Message);
            }
        }
        #endregion
    }
}
