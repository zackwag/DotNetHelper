using Helper.Validation;
using System;

namespace Helper.Security
{
    public static class Caesar
    {
        public static string Decrypt(string value)
        {
            try
            {
                var returnStr = string.Empty;

                value = value.Replace(" ", string.Empty);

                for (var i = 0; i < value.Length; i++)
                {
                    if (i % 2 != 0) continue;

                    if (ValidateData.Integer(value[i].ToString()).Success)
                    {
                        var currDigit = Convert.ToInt32(value[i].ToString());

                        switch (currDigit)
                        {
                            case 0:
                                returnStr += "9";
                                break;
                            default:
                                returnStr += (currDigit - 1).ToString();
                                break;
                        }
                    }
                    else
                    {
                        var currChar = value[i].ToString();

                        switch (currChar.ToUpper())
                        {
                            case "A":
                                returnStr += "Z";
                                break;
                            default:
                                returnStr += Convert.ToChar(Convert.ToInt32(value[i] - 1)).ToString();
                                break;
                        }
                    }
                }

                return returnStr;
            }
            catch (Exception ex)
            {
                throw new Exception("error on Decrypt method, message: " + ex.Message);
            }
        }

        public static string Encrypt(string value)
        {
            const string randStr = "ABCDEFGHIJKLMONPQRSTUVWXYZ0123456789";
            var rand = new Random(DateTime.Now.Millisecond);
            var returnStr = string.Empty;

            value = value.Replace(" ", string.Empty).Replace("-", string.Empty);

            foreach (var t in value)
            {
                if (ValidateData.Integer(t.ToString()).Success)
                {
                    var currDigit = Convert.ToInt32(t.ToString());
                    var nextDigit = currDigit + 1;

                    if (nextDigit == 10)
                        nextDigit = 0;

                    returnStr += nextDigit.ToString();
                }
                else
                {
                    var currChar = Convert.ToChar(t);
                    var nextChar = Convert.ToChar(Convert.ToInt32(currChar) + 1);

                    if (nextChar == '[')
                        nextChar = 'A';

                    returnStr += nextChar.ToString();
                }

                returnStr += randStr[rand.Next(value.Length)].ToString();
            }

            return returnStr;
        }
    }
}
