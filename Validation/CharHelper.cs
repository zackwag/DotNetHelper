using System;
using Helper.Enumeration;

namespace Helper.Validation
{
    public static class CharHelper
    {
        public static char GetControlCharacter(ControlCharacters controlCharacter)
        {
            return Convert.ToChar(Convert.ToInt32(controlCharacter));
        }
    }
}
