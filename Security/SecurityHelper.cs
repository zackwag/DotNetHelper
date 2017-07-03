using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Helper.Security
{
    public class SecurityHelper
    {
        public static string GenerateHash(string origStr)
        {
            try
            {
                var md5 = new MD5CryptoServiceProvider();
                var source = Encoding.ASCII.GetBytes(origStr);
                var hash = md5.ComputeHash(source);

                return hash.Aggregate(string.Empty, (current, b) => current + b.ToString("X2"));
            }
            catch (Exception ex)
            {
                throw new Exception("error on GenerateHash method, message: " + ex.Message);
            }
        }

        public static string GenerateRandomPassword()
        {
            const int defaultMinPasswordLength = 8;
            const int defaultMaxPasswordLength = 10;

            return GenerateRandomPassword(defaultMinPasswordLength, defaultMaxPasswordLength);
        }

        public static string GenerateRandomPassword(int minLength, int maxLength)
        {
            try
            {
                const string passwordCharsLcase = "abcdefgijkmnopqrstwxyz";
                const string passwordCharsUcase = "ABCDEFGHJKLMNPQRSTWXYZ";
                const string passwordCharsNumeric = "123456789";
                const string passwordCharsSpecial = "*$!%";

                // Make sure that input parameters are valid.
                if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
                    return null;

                // Create a local array containing supported password characters
                // grouped by types. You can remove character groups from this
                // array, but doing so will weaken the password strength.
                var charGroups = new[]
                {
                    passwordCharsLcase.ToCharArray(),
                    passwordCharsUcase.ToCharArray(),
                    passwordCharsNumeric.ToCharArray(),
                    passwordCharsSpecial.ToCharArray()
                };

                // Use this array to track the number of unused characters in each
                // character group.
                var charsLeftInGroup = new int[charGroups.Length];

                // Initially, all characters in each group are not used.
                for (var i = 0; i < charsLeftInGroup.Length; i++)
                {
                    charsLeftInGroup[i] = charGroups[i].Length;
                }

                // Use this array to track (iterate through) unused character groups.
                var leftGroupsOrder = new int[charGroups.Length];

                // Initially, all character groups are not used.
                for (var i = 0; i < leftGroupsOrder.Length; i++)
                {
                    leftGroupsOrder[i] = i;
                }

                // Because we cannot use the default randomizer, which is based on the
                // current time (it will produce the same "random" number within a
                // second), we will use a random number generator to seed the
                // randomizer.

                // Use a 4-byte array to fill it with random bytes and convert it then
                // to an integer value.
                var randomBytes = new byte[4];

                // Generate 4 random bytes.
                var rng = new RNGCryptoServiceProvider();
                rng.GetBytes(randomBytes);

                // Convert 4 bytes into a 32-bit integer value.
                var seed = (randomBytes[0] & 0x7f) << 24 | randomBytes[1] << 16 | randomBytes[2] << 8 | randomBytes[3];

                //Create the random generator
                var random = new Random(seed);

                // This array will hold password characters.
                // Allocate appropriate memory for the password.
                var password = new char[minLength < maxLength ? random.Next(minLength, maxLength + 1) : minLength];

                // Index of the last non-processed group.
                var lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

                // Generate password characters one at a time.
                for (var i = 0; i < password.Length; i++)
                {
                    // If only one character group remained unprocessed, process it;
                    // otherwise, pick a random character group from the unprocessed
                    // group list. To allow a special character to appear in the
                    // first position, increment the second parameter of the Next
                    // function call by one, i.e. lastLeftGroupsOrderIdx + 1.
                    var nextLeftGroupsOrderIdx = lastLeftGroupsOrderIdx == 0 ? 0 : random.Next(0, lastLeftGroupsOrderIdx);

                    // Get the actual index of the character group, from which we will
                    // pick the next character.
                    var nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

                    // Get the index of the last unprocessed characters in this group.
                    var lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

                    // If only one unprocessed character is left, pick it; otherwise,
                    // get a random character from the unused character list.
                    var nextCharIdx = lastCharIdx == 0 ? 0 : random.Next(0, lastCharIdx + 1);

                    // Add this character to the password.
                    password[i] = charGroups[nextGroupIdx][nextCharIdx];

                    if (lastCharIdx == 0) // If we processed the last character in this group, start over.
                        charsLeftInGroup[nextGroupIdx] = charGroups[nextGroupIdx].Length;
                    else // There are more unprocessed characters left.
                    {
                        // Swap processed character with the last unprocessed character so that we don't pick it
                        // until we process all characters in this group.
                        if (lastCharIdx != nextCharIdx)
                        {
                            var temp = charGroups[nextGroupIdx][lastCharIdx];
                            charGroups[nextGroupIdx][lastCharIdx] = charGroups[nextGroupIdx][nextCharIdx];
                            charGroups[nextGroupIdx][nextCharIdx] = temp;
                        }

                        // Decrement the number of unprocessed characters in this group.
                        charsLeftInGroup[nextGroupIdx]--;
                    }

                    if (lastLeftGroupsOrderIdx == 0) // If we processed the last group, start all over.
                        lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                    else // There are more unprocessed groups left.
                    {
                        // Swap processed group with the last unprocessed group so that we don't pick it 
                        // until we process all groups.
                        if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                        {
                            int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                            leftGroupsOrder[lastLeftGroupsOrderIdx] = leftGroupsOrder[nextLeftGroupsOrderIdx];
                            leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                        }

                        // Decrement the number of unprocessed groups.
                        lastLeftGroupsOrderIdx--;
                    }
                }

                // Convert password characters into a string and return the result.
                return new string(password);
            }
            catch (Exception ex)
            {
                throw new Exception("error on GenerateRandomPassword method, message: " + ex.Message);
            }
        }

        public static string GenerateUniqueId()
        {
            const int maxSize = 8;

            var chars = new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '1', '2', '3', '4', '5',
                '6', '7', '8', '9', '0' };
            var data = new byte[maxSize];

            var crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);

            var result = new StringBuilder(maxSize);

            foreach (var b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }

            return result.ToString();
        }
    }
}
