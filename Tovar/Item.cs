using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tovar
{
    public class Item : IWritableObject, IReadableObject
    {
        private string itemBarcode;
        private string itemVendorCode;
        private string itemName;
        private string itemUnit;
        private float itemZakupPrice;
        private string itemShelfLife;
        private float morzha;
        private uint itemCount;
        private DateTime itemDateOfManufacturing;
        private Manufacturer itemManufacturer;

        public string Barcode
        { get { return this.itemBarcode; } }

        public string VendorCode
        { get { return this.itemVendorCode; } }

        public string Name
        { get { return this.itemName; } }

        public string Unit
        { get { return this.itemUnit; } }

        public float ZakupPrice
        { get { return this.itemZakupPrice; } }

        public string ShelfLife
        { get { return this.itemShelfLife; } }

        public float Morzha
        { get { return this.morzha; } }

        public float NDS
        { get { return (this.itemZakupPrice + this.morzha) * (float)0.2; } }

        public float SellPrice
        { get { return this.itemZakupPrice + this.morzha + this.NDS; } }

        public uint Count
        { get { return this.itemCount; } }

        public DateTime DateOfManufacturing
        { get { return this.itemDateOfManufacturing; } }

        public Manufacturer ItemManufacturer
        { get { return this.itemManufacturer; } }

        public Item(string vendorCode, string name, string unit, float zakupPrice, string shelfLife, uint count, DateTime dateOfManufacuring, Manufacturer manufacturer)
        {
            this.itemBarcode = Randomizer.RandomBarcode();
            this.itemVendorCode = vendorCode;
            this.itemName = name;
            this.itemUnit = unit;
            this.itemZakupPrice = zakupPrice;
            this.itemShelfLife = shelfLife;
            this.itemCount = count;
            this.morzha = Randomizer.RandomMorzha();
            this.itemDateOfManufacturing = dateOfManufacuring;
            this.itemManufacturer = manufacturer;
        }

        private Item(ILoadManager man)
        {
            this.itemBarcode = man.ReadLine().Split(':')[1];
            this.itemVendorCode = man.ReadLine().Split(':')[1];
            this.itemName = man.ReadLine().Split(':')[1];
            this.itemUnit = man.ReadLine().Split(':')[1];
            float.TryParse(man.ReadLine().Split(':')[1], out this.itemZakupPrice);
            this.itemShelfLife = man.ReadLine().Split(':')[1];
            uint.TryParse(man.ReadLine().Split(':')[1], out this.itemCount);
            float.TryParse(man.ReadLine().Split(':')[1], out this.morzha);
            DateTime.TryParse(man.ReadLine().Split(':')[1], out this.itemDateOfManufacturing);
            Shop shop;
            Shops.FindShopByManufacturerName(man.ReadLine().Split(':')[1], out shop);
            this.itemManufacturer = shop.Manufacturer;
            Shops.AddShopItem(this.itemManufacturer.Name, this);
        }

        public void Write(ISaveManager man)
        {
            man.WriteLine($"itemBarcode:{this.itemBarcode}");
            man.WriteLine($"itemVendorCode:{this.itemVendorCode}");
            man.WriteLine($"itemName:{this.itemName}");
            man.WriteLine($"itemUnit:{this.itemUnit}");
            man.WriteLine($"itemZakupPrice:{this.itemZakupPrice.ToString()}");
            man.WriteLine($"itemShelfLife:{this.itemShelfLife}");
            man.WriteLine($"itemCount:{this.itemCount.ToString()}");
            man.WriteLine($"morzha:{this.morzha.ToString()}");
            man.WriteLine($"itemDateOfManufacturing:{this.itemDateOfManufacturing.ToString()}");
            man.WriteLine($"itemManufacturer:{this.itemManufacturer.Name}");
        }

        public class Loader : IReadableObjectLoader<Item>
        {
            public Loader() { }
            public Item Load(ILoadManager man)
            {
                return new Item(man);
            }
        }
    }
}
