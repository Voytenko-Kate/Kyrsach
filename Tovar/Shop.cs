using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tovar
{
    public class Shop
    {
        private List<Item> items = new List<Item>();
        private Manufacturer manufacturer;

        public Manufacturer Manufacturer
        { get { return this.manufacturer; } }

        public List<Item> Items
        { get { return this.items; } }

        public Shop(Manufacturer manufacturer)
        {
            this.manufacturer = manufacturer;
        }

        public Shop() { }

        public void AddItem(Item item)
        {
            this.items.Add(item);
        }

    }
}
