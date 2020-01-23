using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tovar
{
    public static class Shops
    {
        private static List<Shop> shops = new List<Shop>();

        public static List<Shop> GetShops
        { get { return shops; } }

        public static void AddShop(Shop shop)
        {
            shops.Add(shop);
        }

        public static bool FindShopByManufacturerName(string manufacturerName, out Shop shop)
        {
            for(int i = 0; i < shops.Count; i++)
            {
                if (shops[i].Manufacturer.Name == manufacturerName)
                {
                    shop = shops[i];
                    return true;
                }
            }
            shop = new Shop();
            return false;
        }
    }
}
