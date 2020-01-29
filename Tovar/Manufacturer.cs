using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tovar
{
    public class Manufacturer : IWritableObject, IReadableObject
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

        private Manufacturer(ILoadManager man)
        {
            this.manufacturerName = man.ReadLine().Split(':')[1];
            this.manufacturingCountry = man.ReadLine().Split(':')[1];
            this.manufacturerAddress = man.ReadLine().Split(':')[1];
            ulong phoneNum;
            ulong.TryParse(man.ReadLine().Split(':')[1], out phoneNum);
            this.manufacturerPhone = new Phone(phoneNum);
        }

        public void Write(ISaveManager man)
        {
            man.WriteLine($"manufacturerName:{this.manufacturerName}");
            man.WriteLine($"manufacturingCountry:{this.manufacturingCountry}");
            man.WriteLine($"manufacturerAddress:{this.manufacturerAddress}");
            man.WriteLine($"manufacturerPhone:{this.manufacturerPhone.RawString}");
        }

        public class Loader : IReadableObjectLoader<Manufacturer>
        {
            public Loader() { }
            public Manufacturer Load(ILoadManager man)
            {
                return new Manufacturer(man);
            }
        }
    }
}
