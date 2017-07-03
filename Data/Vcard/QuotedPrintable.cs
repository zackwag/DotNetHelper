using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Helper.Extensions;

namespace Helper.Data.Vcard
{
    /// <summary>
    /// Provide encoding and decoding of Quoted-Printable.
    /// </summary>
    internal class QuotedPrintable
    {
        /// <summary>
        /// so including the = connection, the length will be 76
        /// </summary>
        private const int RFC_1521_MAX_CHARS_PER_LINE = 75;

        /// <summary>
        /// Return quoted printable string with 76 characters per line.
        /// </summary>
        /// <param name="textToEncode"></param>
        /// <returns></returns>
        public static string Encode(string textToEncode)
        {
            if (textToEncode.IsNull())
                throw new ArgumentNullException();

            return Encode(textToEncode, RFC_1521_MAX_CHARS_PER_LINE);
        }

        private static string Encode(string textToEncode, int charsPerLine)
        {
            if (textToEncode.IsNull())
                throw new ArgumentNullException();

            if (charsPerLine <= 0)
                throw new ArgumentOutOfRangeException();

            return FormatEncodedString(EncodeString(textToEncode), charsPerLine);
        }

        /// <summary>
        /// Return quoted printable string, all in one line.
        /// </summary>
        /// <param name="textToEncode"></param>
        /// <returns></returns>
        public static string EncodeString(string textToEncode)
        {
            if (textToEncode.IsNull())
                throw new ArgumentNullException();

            var bytes = Encoding.UTF8.GetBytes(textToEncode);
            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                if (b == 0) continue;
                if ((b < 32) || (b > 126))
                    builder.Append($"={b:X2}");
                else
                {
                    switch (b)
                    {
                        case 13:
                            builder.Append("=0D");
                            break;
                        case 10:
                            builder.Append("=0A");
                            break;
                        case 61:
                            builder.Append("=3D");
                            break;
                        default:
                            builder.Append(Convert.ToChar(b));
                            break;
                    }
                }
            }

            return builder.ToString();
        }

        private static string FormatEncodedString(string qpstr, int maxcharlen)
        {
            if (qpstr .IsNull())
                throw new ArgumentNullException();

            var builder = new StringBuilder();
            var charArray = qpstr.ToCharArray();
            var i = 0;
            foreach (char c in charArray)
            {
                builder.Append(c);
                i++;
                if (i != maxcharlen) continue;
                builder.AppendLine("=");
                i = 0;
            }

            return builder.ToString();
        }

        private static string HexDecoderEvaluator(Match m)
        {
            if (string.IsNullOrEmpty(m.Value))
                return null;

            var captures = m.Groups[3].Captures;
            var bytes = new byte[captures.Count];

            for (var i = 0; i < captures.Count; i++)
            {
                bytes[i] = Convert.ToByte(captures[i].Value, 16);
            }

            return bytes.ToString(Encoding.UTF8);
        }

        public static string HexDecoder(string line)
        {
            if (line.IsNull())
                throw new ArgumentNullException();

            return new Regex("((\\=([0-9A-F][0-9A-F]))*)", RegexOptions.IgnoreCase).Replace(line, HexDecoderEvaluator);
        }

        public static string Decode(string encodedText)
        {
            if (encodedText.IsNull())
                throw new ArgumentNullException();

            if (encodedText == string.Empty)
                return string.Empty;

            using (var sr = new StringReader(encodedText))
            {
                var builder = new StringBuilder();

                string line;

                while (!(line = sr.ReadLine()).IsNull())
                {
                    builder.Append(line.EndsWith("=") ? line.Substring(0, line.Length - 1) : line);
                }

                return HexDecoder(builder.ToString());
            }
        }
    }
}
