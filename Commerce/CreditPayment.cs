using Helper.Security;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Helper.Commerce
{
    public class CreditPayment : Payment
    {
        private readonly string _cardNum;

        public CreditPayment(string cardNumber, string cvv, DateTime expiration, PaymentType type, Contact contact, string referenceNumber, string notes)
        {
            try
            {
                _cardNum = cardNumber.Replace("-", string.Empty).Replace(" ", string.Empty);
                Cvv = cvv;
                Expiration = expiration;
                Type = type;
                Contact = contact;
                ReferenceNumber = referenceNumber;
                Notes = notes;

                if (!IsCredit)
                    throw new Exception($"CreditPayment cannot have payment type of '{type}'.");
            }
            catch (Exception ex)
            {
                throw new Exception("error on CreditPayment constructor, message: " + ex.Message);
            }
        }

        public string GetCardNumber(bool isEncrypted)
        {
            return isEncrypted ? Caesar.Encrypt(_cardNum) : _cardNum;
        }

        public string GetLast4Digits(bool isEncrypted)
        {
            return isEncrypted ? Caesar.Encrypt(_cardNum.Substring(_cardNum.Length - 4)) : _cardNum.Substring(_cardNum.Length - 4);
        }

        public string Cvv { get; }

        public DateTime Expiration { get; }

        #region Helper Methods
        public static PaymentType GetTypeFromNumber(string cardNumber)
        {
            try
            {
                var p = PaymentType.NoPayment;
                cardNumber = cardNumber.Replace(" ", string.Empty).Replace("-", string.Empty);

                var rx = new Regex("^4[0-9]{12}(?:[0-9]{3})?$");

                if (rx.IsMatch(cardNumber))
                    p = PaymentType.Visa;
                else
                {
                    rx = new Regex("^5[1-5][0-9]{14}$");
                    if (rx.IsMatch(cardNumber))
                        p = PaymentType.Mastercard;
                    else
                    {
                        rx = new Regex("^3[47][0-9]{13}$");
                        if (rx.IsMatch(cardNumber))
                            p = PaymentType.AmericanExpress;
                        else
                        {
                            rx = new Regex("^6(?:011|5[0-9]{2})[0-9]{12}$");
                            if (rx.IsMatch(cardNumber))
                                p = PaymentType.Discover;
                        }
                    }
                }

                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetTypeFromNumber method, message: " + ex.Message);
            }
        }

        /// <summary>
        /// Runs Luhn Algorithm to see if it passes checksum and is valid
        /// </summary>
        /// <param name="cardNumber">Credit card number</param>
        /// <returns>Validity of card number.</returns>
        private static bool PassesLuhnAlgorithm(string cardNumber)
        {
            const string allowed = "0123456789";
            var cleanNumber = new StringBuilder();
            int i;

            for (i = 0; i < cardNumber.Length; i++)
            {
                if (allowed.IndexOf(cardNumber.Substring(i, 1), StringComparison.Ordinal) >= 0)
                    cleanNumber.Append(cardNumber.Substring(i, 1));
            }
            if (cleanNumber.Length < 13 || cleanNumber.Length > 16)
                return false;

            for (i = cleanNumber.Length + 1; i <= 16; i++)
                cleanNumber.Insert(0, "0");

            var total = 0;
            var number = cleanNumber.ToString();

            for (i = 1; i <= 16; i++)
            {
                var multiplier = 1 + (i % 2);
                var digit = int.Parse(number.Substring(i - 1, 1));
                var sum = digit * multiplier;
                if (sum > 9)
                    sum -= 9;
                total += sum;
            }
            return (total % 10 == 0);
        }

        /// <summary>
        /// Validates a set of credit card information
        /// </summary>
        /// <param name="cardName">Name on credit card</param>
        /// <param name="cardType">Type of card</param>
        /// <param name="cardNumber">Credit card number</param>
        /// <param name="cardCvv">Credit card validation number</param>
        /// <param name="expiry">Date of expiration</param>
        /// <returns>An error string summarizing problems with the input</returns>
        public static string Validate(string cardName, string cardType, string cardNumber, string cardCvv, DateTime expiry)
        {
            try
            {
                cardNumber = cardNumber.Replace(" ", string.Empty).Replace("-", string.Empty);

                var errorStr = string.Empty;

                if (cardName.Length == 0)
                    errorStr += "<li> Please enter a name on the credit card.</li>";

                if (cardType.Length == 0)
                    errorStr += "<li> Please select a credit card type.</li>";

                if (cardNumber.Length == 0)
                    errorStr += "<li> Please enter a credit card number.</li>";

                if (cardCvv.Length == 0)
                    errorStr += "<li> Please enter a CVV number.</li>";

                if (expiry == DateTime.MinValue)
                    errorStr += "<li> Please enter an expiration date.</li>";

                if (errorStr.Length == 0)
                {
                    var rx = new Regex(@"^(?:(?<Visa>4\d{3})|(?<Mastercard>5[1-5]\d{2})|(?<Discover>6011)|(?<DinersClub>(?:3[68]\d{2})|(?:30[0-5]\d))|(?<Amex>3[47]\d{2}))([ -]?)(?(DinersClub)(?:\d{6}\1\d{4})|(?(Amex)(?:\d{6}\1\d{5})|(?:\d{4}\1\d{4}\1\d{4})))$");

                    if (rx.IsMatch(cardNumber))
                    {
                        var m = rx.Match(cardNumber);

                        if (m.Groups[cardType].ToString() == "")
                        {
                            errorStr += "<li> Please enter a valid credit card number for the type: " + cardType.Replace("Amex", "American Express") + ".</li>";
                        }
                        else
                        {
                            if (!PassesLuhnAlgorithm(cardNumber))
                                errorStr += "<li> Please enter a valid credit card number.</li>";
                        }

                        switch (cardType)
                        {
                            case "Amex":
                                if (cardCvv.Length != 4)
                                    errorStr += "<li> Please enter the proper 4 digit CVV for an American Express card.</li>";
                                break;
                            default:
                                if (cardCvv.Length != 3)
                                    errorStr += "<li> Please enter the proper 3 digit CVV for a " + cardType + " card.</li>";
                                break;
                        }

                        if (expiry.Date < DateTime.Now.Date)
                            errorStr += "<li> Please enter a new expiration date as this date has passed.</li>";
                    }
                    else
                        errorStr += "<li> Please enter a valid credit card number.</li>";
                }
                return errorStr;
            }
            catch (Exception ex)
            {
                throw new Exception("error on ValidateCard method, message: " + ex.Message);
            }
        }
        #endregion

        #region Obsolete Methods
        //public String GetCardNumber(Boolean isEncrypted)
        //{
        //    return isEncrypted ? Encrypt(_cardNum) : _cardNum;
        //}
        #endregion
    }
}