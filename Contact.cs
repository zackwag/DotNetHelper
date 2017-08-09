using System;
using System.Net.Mail;
using Helper.Extensions;
using Helper.Enumeration;
using Helper.Validation;

namespace Helper
{
    public class Contact
    {
        private string _prefix;
        private string _firstName;
        private string _middle;
        private string _lastName;
        private string _suffix;
        private string _title;
        private string _email;
        private string _workPhone;
        private string _ext;
        private string _fax;
        private string _cellPhone;
        private string _homePhone;
        private string _gender;

        public Contact(string fullName, string title, string email, string address1, string address2, string city, string state, string zip, string workPhone, string extension, string fax, string cellPhone, string homePhone)
        {
            SetMemberData(string.Empty, fullName.Split(Convert.ToChar(ControlCharacters.Space))[0], string.Empty, fullName.Contains(" ") ? fullName.Substring(fullName.IndexOf(Convert.ToChar(ControlCharacters.Space)) + 1) : string.Empty, string.Empty, title, email, address1, address2, city, state, zip, workPhone, extension, fax, cellPhone, homePhone, string.Empty, false);
        }

        public Contact(string prefix, string fullName, string title, string email, string address1, string address2, string city, string state, string zip, string workPhone, string extension, string fax, string cellPhone, string homePhone)
        {
            SetMemberData(prefix, fullName.Split(Convert.ToChar(ControlCharacters.Space))[0], string.Empty, fullName.Contains(" ") ? fullName.Substring(fullName.IndexOf(Convert.ToChar(ControlCharacters.Space)) + 1) : string.Empty, string.Empty, title, email, address1, address2, city, state, zip, workPhone, extension, fax, cellPhone, homePhone, string.Empty, false);
        }

        public Contact(string prefix, string fullName, string title, string email, string address1, string address2, string city, string state, string zip, string workPhone, string extension, string fax, string cellPhone, string homePhone, string gender, bool inheritAddress)
        {
            SetMemberData(prefix, fullName.Split(Convert.ToChar(ControlCharacters.Space))[0], string.Empty, fullName.Contains(" ") ? fullName.Substring(fullName.IndexOf(Convert.ToChar(ControlCharacters.Space)) + 1) : string.Empty, string.Empty, title, email, address1, address2, city, state, zip, workPhone, extension, fax, cellPhone, homePhone, gender, inheritAddress);
        }

        public Contact(string prefix, string first, string middle, string last, string suffix, string title, string email, string address1, string address2, string city, string state, string zip, string workPhone, string extension, string fax, string cellPhone, string homePhone)
        {
            SetMemberData(prefix, first, middle, last, suffix, title, email, address1, address2, city, state, zip, workPhone, extension, fax, cellPhone, homePhone, string.Empty, false);
        }

        public Contact(string prefix, string first, string middle, string last, string suffix, string title, string email, Address address, string workPhone, string extension, string fax, string cellPhone, string homePhone, string gender, bool inheritAddress)
        {
            SetMemberData(prefix, first, middle, last, suffix, title, email, address.Address1, address.Address2, address.City, address.State, address.Zip.ToString(), workPhone, extension, fax, cellPhone, homePhone, gender, inheritAddress);
        }

        public Contact(string prefix, string first, string middle, string last, string suffix, string title, string email, string address1, string address2, string city, string state, string zip, string workPhone, string extension, string fax, string cellPhone, string homePhone, string gender, bool inheritAddress)
        {
            SetMemberData(prefix, first, middle, last, suffix, title, email, address1, address2, city, state, zip, workPhone, extension, fax, cellPhone, homePhone, gender, inheritAddress);
        }

        private void SetMemberData(string prefix, string first, string middle, string last, string suffix, string title, string email, string address1, string address2, string city, string state, string zip, string workPhone, string extension, string fax, string cellPhone, string homePhone, string gender, bool inheritAddress)
        {
            _prefix = prefix;
            _firstName = first;
            _middle = middle;
            _lastName = last;
            _suffix = suffix;
            _title = title;
            _email = email;
            Address = new Address(address1, address2, city, state, zip);
            _workPhone = workPhone;
            _ext = extension;
            _fax = fax;
            _cellPhone = cellPhone;
            _homePhone = homePhone;
            _gender = gender;
            InheritAddress = inheritAddress;
        }

        public string Salutation
        {
            set { _prefix = value; }
            get { return ReturnValue(_prefix); }
        }

        public string FullName => $"{_firstName}{(string.IsNullOrEmpty(_firstName) ? string.Empty : " ") + _lastName}".safeTrim();

        public string FirstName
        {
            set { _firstName = value; }
            get { return ReturnValue(_firstName); }
        }

        public string Middle
        {
            set { _middle = value; }
            get { return ReturnValue(_middle); }
        }

        public string LastName
        {
            set { _lastName = value; }
            get { return ReturnValue(_lastName); }
        }

        public string Suffix
        {
            set { _suffix = value; }
            get { return ReturnValue(_suffix); }
        }

        public string Title
        {
            set { _title = value; }
            get { return ReturnValue(_title); }
        }

        public MailAddress Email
        {
            get
            {
                MailAddress ma = null;

                var result = ValidateData.Email(_email);

                if (!result.Success) return null;

                try
                {
                    ma = new MailAddress(result.ProcessedValue, FullName);
                }
                catch (Exception)
                {
                    //fail silently
                }

                return ma;
            }
        }

        public Address Address { get; private set; }

        public bool InheritAddress { get; private set; }

        public string WorkPhone => ValidateData.PhoneNumber(_workPhone).ProcessedValue;

        public string Extension => _ext.safeTrim();

        public string Fax => ValidateData.PhoneNumber(_fax).ProcessedValue;

        public string CellPhone => ValidateData.PhoneNumber(_cellPhone).ProcessedValue;

        public string HomePhone => ValidateData.PhoneNumber(_homePhone).ProcessedValue;

        public string Gender => _gender.safeTrim();

        public string GetFormattedWorkPhone(bool includeExt)
        {
            try
            {
                var returnStr = ValidateData.PhoneNumber(WorkPhone).ProcessedValue;

                if (includeExt && !string.IsNullOrEmpty(returnStr) && !string.IsNullOrEmpty(Extension))
                    returnStr += $" x{Extension}";

                return returnStr;
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetFormattedWorkPhone method, message: " + ex.Message);
            }
        }
    }
}
