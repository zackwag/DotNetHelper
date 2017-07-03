using System;
using System.IO;
using System.Web;
using Helper.Extensions;

namespace Helper
{
    public class Address
    {
        private string _name;
        private string _address1;
        private string _address2;
        private string _city;
        private string _state;
        private string _zip;
        private string _subName;

        public Address(string address1, string address2, string city, string state, string zip)
        {
            SetClassData(string.Empty, address1, address2, city, state, zip, null, string.Empty);
        }

        public Address(string name, string address1, string address2, string city, string state, string zip)
        {
            SetClassData(name, address1, address2, city, state, zip, null, string.Empty);
        }

        public Address(string name, string address1, string address2, string city, string state, string zip, Uri link, string subName)
        {
            SetClassData(name, address1, address2, city, state, zip, link, subName);
        }

        private void SetClassData(string name, string address1, string address2, string city, string state, string zip, Uri link, string subName)
        {
            _name = string.IsNullOrEmpty(name) ? string.Empty : name;
            _address1 = string.IsNullOrEmpty(address1) ? string.Empty : address1;
            _address2 = string.IsNullOrEmpty(address2) ? string.Empty : address2;
            _city = string.IsNullOrEmpty(city) ? string.Empty : city;
            _state = string.IsNullOrEmpty(state) ? string.Empty : state;
            _zip = string.IsNullOrEmpty(zip) ? string.Empty : zip;
            Link = link;
            _subName = subName;
        }

        public Uri AppleMapsUrl => GetMapUrl("http://maps.apple.com/");

        public Uri GoogleMapsUrl => GetMapUrl("http://maps.google.com/");

        private Uri GetMapUrl(string baseUrl)
        {
            Uri u = null;

            try
            {
                u = new Uri($"{baseUrl}?daddr={(string.IsNullOrEmpty(_address1) ? string.Empty : HttpUtility.UrlEncode(_address1))}{(string.IsNullOrEmpty(_city) ? string.Empty : "+" + HttpUtility.UrlEncode(_city))}{(string.IsNullOrEmpty(_state) ? string.Empty : "+" + HttpUtility.UrlEncode(_state))}{(string.IsNullOrEmpty(_zip) ? string.Empty : "+" + HttpUtility.UrlEncode(_zip))}");
            }
            catch (Exception)
            {
                //fail silently
            }

            return u;
        }

        public string LocationName
        {
            set { _name = value; }
            get { return ReturnValue(_name); }
        }

        public string SubLocationName
        {
            set { _subName = value; }
            get { return ReturnValue(_subName); }
        }

        public string Address1
        {
            set { _address1 = value; }
            get { return ReturnValue(_address1); }
        }

        public string Address2
        {
            set { _address2 = value; }
            get { return ReturnValue(_address2); }
        }

        public string City
        {
            set { _city = value; }
            get { return ReturnValue(_city); }
        }

        public string State
        {
            set { _state = value; }
            get { return ReturnValue(_state); }
        }

        public Zipcode Zip
        {
            set { _zip = value.ToString(); }
            get { return new Zipcode(ReturnValue(_zip)); }
        }

        public Uri Link { get; private set; }

        private static string ReturnValue(string origVal) => !string.IsNullOrEmpty(origVal) ? origVal.Trim() : string.Empty;

        public string ToMicroData()
        {
            var address = string.Empty;
            var cityStateZip = string.Empty;
            var returnStr = string.Empty;

            if (!string.IsNullOrEmpty(_name))
            {
                var location = $"<span itemprop=\"name\">{_name + (!string.IsNullOrEmpty(_subName.Trim()) && _subName != "-" ? "<br />" + _subName : string.Empty)}</span>";

                if (!Link.IsNull())
                    location = $"<a itemprop=\"url\" href=\"{Link}\">{location}</a>";

                returnStr = "<div itemprop=\"location\" itemscope itemtype=\"http://data-vocabulary.org/Organization\">" + location + "{0}</div>";
            }

            if (!string.IsNullOrEmpty(_address1.Trim()) && _name.ToLower() != _address1.ToLower())
            {
                address = _address1;

                if (!string.IsNullOrEmpty(_address2.Trim()))
                    address += "<br />" + _address2;

                address = $"<span itemprop=\"street-address\">{address}</span>";
            }

            if (!string.IsNullOrEmpty(_city.Trim()))
                cityStateZip += $"<span itemprop=\"locality\">{_city}</span>";

            if (!string.IsNullOrEmpty(_state.Trim()))
            {
                if (!string.IsNullOrEmpty(cityStateZip.Trim()))
                    cityStateZip += ", ";

                cityStateZip += $"<span itemprop=\"region\">{_state}</span>";
            }

            if (!string.IsNullOrEmpty(_zip.Trim()))
            {
                if (!string.IsNullOrEmpty(cityStateZip.Trim()))
                    cityStateZip += " ";

                cityStateZip += $"<span itemprop=\"postal-code\">{_zip}</span>";
            }

            if (!string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(cityStateZip))
                address = $"{address}<br />{cityStateZip}";

            if (!string.IsNullOrEmpty(address))
                address = $"<div itemprop=\"address\" itemscope itemtype=\"http://data-vocabulary.org/Address\">{address}</div>";

            return string.IsNullOrEmpty(returnStr) ? address : string.Format(returnStr, address);
        }

        public override string ToString()
        {
            return ToString(false, false, false);
        }

        public string ToString(bool includeAddress2)
        {
            return ToString(false, includeAddress2, false);
        }

        public string ToString(bool includeLocationName, bool includeAddress2, bool includeLocationLink)
        {
            var sw = new StringWriter();
            var cityStateZip = string.Empty;

            if (includeLocationName)
            {
                sw.WriteLine((!Link.IsNull() && includeLocationLink) ? $"<a href=\"{Link}\" target=\"_blank\">{_name}</a>" : _name);

                if (!string.IsNullOrEmpty(_subName.Trim()) && _subName != "-")
                    sw.WriteLine(_subName);

                if (!string.IsNullOrEmpty(_address1.Trim()) && _name.ToLower() != _address1.ToLower())
                    sw.WriteLine(!includeAddress2 ? (_address1.Contains(",") ? _address1.Split(',')[0].Trim() : _address1) : _address1);
            }
            else
            {
                if (!string.IsNullOrEmpty(_address1.Trim()))
                    sw.WriteLine(!includeAddress2 ? (_address1.Contains(",") ? _address1.Split(',')[0].Trim() : _address1) : _address1);
            }

            if (includeAddress2 && !string.IsNullOrEmpty(_address2.Trim()))
                sw.WriteLine(_address2);

            if (!string.IsNullOrEmpty(_city.Trim()))
                cityStateZip += _city;

            if (!string.IsNullOrEmpty(_state.Trim()))
            {
                if (!string.IsNullOrEmpty(cityStateZip.Trim()))
                    cityStateZip += ", ";

                cityStateZip += _state;
            }

            if (!string.IsNullOrEmpty(_zip.Trim()))
            {
                if (!string.IsNullOrEmpty(cityStateZip.Trim()))
                    cityStateZip += " ";

                cityStateZip += _zip;
            }

            if (!string.IsNullOrEmpty(cityStateZip.Trim()))
                sw.Write(cityStateZip);

            return sw.ToString();
        }
    }
}
