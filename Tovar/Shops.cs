using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tovar
{
    public static class Shops
    {
        private static SaveManager manufacturerSaver = new SaveManager("manufacturers.txt");

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

        public static void AddShopItem(string manufacturerName, Item item)
        {
            for (int i = 0; i < shops.Count; i++)
            {
                if (shops[i].Manufacturer.Name == manufacturerName)
                {
                    shops[i].AddItem(item);
                }
            }
        }

        public static void SaveManufacturers()
        {
            manufacturerSaver.CreateFile();
            foreach (Shop shop in shops)
                manufacturerSaver.WriteObject(shop.Manufacturer);
        }

        public static void LoadManufacturers()
        {
            shops.Clear();
            LoadManager manufacturerLoader = new LoadManager("manufacturers.txt");
            Logger logger = new Logger(new FileInfo("log.txt").AppendText());
            LoadLogger loadLogger = new LoadLogger(manufacturerLoader, logger);
            manufacturerLoader.BeginRead();
            while (manufacturerLoader.IsLoading)
                shops.Add(new Shop(manufacturerLoader.Read(new Manufacturer.Loader())));
            manufacturerLoader.EndRead();
        }
    }
}
