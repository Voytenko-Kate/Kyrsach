using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tovar
{
    public class Manufacturer
    {
        private string manufacturerName;
        private string manufacturingCountry;
        private string manufacturerAddress;
        private Phone manufacturerPhone;

        public string Name
        { get { return this.manufacturerName; } }

        public string Country
        { get { return this.manufacturingCountry; } }

        public string Address
        { get { return this.manufacturerAddress; } }

        public Phone ManufacturerPhone
        { get { return this.manufacturerPhone; } }

        public Manufacturer(string name, string country, string address, Phone phone)
        {
            this.manufacturerName = name;
            this.manufacturingCountry = country;
            this.manufacturerAddress = address;
            this.manufacturerPhone = phone;
        }
    }
}
