using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tovar
{
    public struct Phone
    {
        private ulong phone;

        public Phone(ulong number)
        {
            if (number.ToString().Length > 15) throw new ArgumentException("Телефон должен содержать максимум 15 цифр.");
            if (number.ToString().Length < 11) throw new ArgumentException("Телефон должен содержать как минимум 11 цифр.");
            this.phone = number;
        }

        public string RawString{ get => this.phone.ToString(); }

        public override string ToString()
        {
            string phoneStr = "", numberStr = this.phone.ToString();
            int countryCodeLength = -1 * (10 - numberStr.Length);
            phoneStr += '+';
            phoneStr += numberStr.Substring(0, countryCodeLength);
            phoneStr += "-(";
            for(int i = countryCodeLength; i < countryCodeLength + 3; i++)
                phoneStr += numberStr[i];
            phoneStr += ")-";
            for (int i = countryCodeLength + 3; i < countryCodeLength + 6; i++)
                phoneStr += numberStr[i];
            phoneStr += '-';
            for (int i = countryCodeLength + 6; i < countryCodeLength + 8; i++)
                phoneStr += numberStr[i];
            phoneStr += '-';
            for (int i = countryCodeLength + 8; i < countryCodeLength + 10; i++)
                phoneStr += numberStr[i];
            return phoneStr;
        }
    }
}
