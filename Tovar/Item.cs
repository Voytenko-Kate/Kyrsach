using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tovar
{
    public class Item
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
    }
}
