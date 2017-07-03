//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;

//namespace Helper
//{
//    public class PhoneNumber
//    {
//        private String _digits;
//        private String _extension;

//        public PhoneNumber(String number)
//        {
//            try
//            {
//                SetClassMembers(number, null);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("error on PhoneNumber constructor, message: " + ex.Message);
//            }
//        }

//        public PhoneNumber(String number, String extension)
//        {
//            try
//            {
//                SetClassMembers(number, null);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("error on PhoneNumber constructor, message: " + ex.Message);
//            }
//        }

//        private void SetClassMembers(String number, String extension)
//        {
//            Regex rx = new Regex(@"\D"); //remove all non-digit characters
//            _digits = rx.Replace(number, String.Empty);

//            if (_digits.Length == 10) //US Numbers Are 10 Digits Long
//            {
//                _extension = rx.Replace(extension, String.Empty); //remova all non-digit characters
//            }
//            else
//                throw new Exception("invalid number of digits in '" + number + "'.");
//        }

//        public String AreaCode
//        {
//            get
//            {
//                return _digits.Substring(0, 3);
//            }
//        }

//        public String LocalNumber
//        {
//            get
//            {
//                return String.Format("{0}-{1}", _digits.Substring(3, 3), _digits.Substring(6, 4));
//            }
//        }

//        public String Digits
//        {
//            get
//            {
//                return _digits;
//            }
//        }

//        public String Extension
//        {
//            get
//            {
//                return String.IsNullOrEmpty(_extension.Trim()) ? String.Empty : _extension;
//            }
//        }

//        public override String ToString()
//        {
//            return ToString(false);
//        }

//        public String ToString(Boolean includeExt)
//        {
//            String returnStr = String.Format("({0}) {1}-{2}", _digits.Substring(0, 3), _digits.Substring(3, 3), _digits.Substring(6, 4));

//            if (includeExt)
//                returnStr += String.Format(" x{0}", Extension);

//            return returnStr;
//        }
//    }
//}
