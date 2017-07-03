using Helper.Extensions;
using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Helper.Security
{
    public class SecureQuery
    {
        private static string TamperKey => ConfigurationManager.AppSettings.ContainsKey("privateKey") ? "alksfjlkasjfl3425" : ConfigurationManager.AppSettings["privateKey"];

        public static string Encode(string val)
        {
            return string.IsNullOrEmpty(val) ? string.Empty : HttpUtility.UrlEncode(TamperProofStringEncode(val, TamperKey));
        }

        public static string Decode(string val)
        {
            var decoded = TamperProofStringDecode(val, TamperKey);

            if (decoded == "-1")
                decoded = TamperProofStringDecode(HttpUtility.UrlDecode(val), TamperKey);

            return decoded;
        }

        //Function to encode the string
        private static string TamperProofStringEncode(string val, string key)
        {
            var md5 = new MD5CryptoServiceProvider();
            var mac3Des = new MACTripleDES
            {
                Key = md5.ComputeHash(Encoding.UTF8.GetBytes(key))
            };

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(val)) + Convert.ToChar("-")  + Convert.ToBase64String(mac3Des.ComputeHash(Encoding.UTF8.GetBytes(val)));
        }

        //Function to decode the string
        //Throws an exception if the data is corrupt
        private static string TamperProofStringDecode(string value, string key)
        {
            string dataValue;

            var md5 = new MD5CryptoServiceProvider();
            var mac3Des = new MACTripleDES
            {
                Key = md5.ComputeHash(Encoding.UTF8.GetBytes(key))
            };

            try
            {
                dataValue = Encoding.UTF8.GetString(Convert.FromBase64String(value.Split('-')[0]));
                var storedHash = Encoding.UTF8.GetString(Convert.FromBase64String(value.Split('-')[1]));
                var calcHash = Encoding.UTF8.GetString(mac3Des.ComputeHash(Encoding.UTF8.GetBytes(dataValue)));

                if (storedHash != calcHash)
                {
                    //Data was corrupted
                    throw new ArgumentException("Hash value does not match");
                    //This error is immediately caught below
                }
            }
            catch (Exception)
            {
                dataValue = "-1";
            }

            return dataValue;
        }
    }
}
