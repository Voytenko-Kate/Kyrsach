using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Tovar
{
    public static class ItemReader
    {

        private static string[] valuesInput = { "itemVendorCode", "itemName", "itemUnit", "itemZakupPrice", "itemShelfLife", "itemDateOfManufacturing", "itemCount",
            "manufacturerName", "manufacturingCountry", "manufacturerAddress", "manufacturerPhone"};

        private static string[] valuesInputStrings = { "Артикул (6 цифр), оставьте пустым для случайной генерации:", "Имя товара: ", "Единица измерения товара: ",
            "Закупочная цена: ", "Срок годности: ", "Дата производства: ", "Количество товара: ", "Название производителя: ", "Страна производства: ",
            "Адрес производителя: ", "Телефон производителя: "};

        public static List<Item> ReadItemsFromFile(string path)
        {
            FileInfo file;
            StreamReader reader;

            file = new FileInfo(path);

            if (!file.Exists) throw new Exception("Невозможно открыть файл.");
            reader = new StreamReader(file.OpenRead(), Encoding.Default);

            List<Item> itemList = new List<Item>();
            while (!reader.EndOfStream)
            {
                bool exists = false;

                string itemVendorCode = reader.ReadLine();
                if (itemVendorCode.StartsWith("\n") || itemVendorCode.StartsWith(" ") || itemVendorCode.Length != 6)
                {
                    Console.WriteLine("Некорректно указан артикул.");
                    itemVendorCode = Randomizer.RandomVendorCode();
                }
                string itemName = reader.ReadLine();
                if (itemName.StartsWith("\n") || itemName.StartsWith(" ") || itemName.Length == 0)
                {
                    Console.WriteLine("Некорректно указано имя товара.");
                }

                string itemUnit = reader.ReadLine();
                if (itemUnit.StartsWith("\n") || itemUnit.StartsWith(" ") || itemUnit.Length == 0)
                {
                    Console.WriteLine("Некорректно указана единица измерения товара {0}.", itemName);
                }

                float itemZakupPrice;
                bool successPrice = float.TryParse(reader.ReadLine(), out itemZakupPrice);
                if (!successPrice)
                {
                    itemZakupPrice = 0;
                    Console.WriteLine("Некорректно указана закупочная цена для товара {0}.", itemName);
                }

                string itemShelfLife = reader.ReadLine();
                if (itemShelfLife.StartsWith("\n") || itemShelfLife.StartsWith(" ") || itemShelfLife.Length == 0)
                {
                    Console.WriteLine("Некорректно указан срок годности товара {0}.", itemName);
                }

                DateTime itemDateOfManufacturing;
                IFormatProvider culture = CultureInfo.CreateSpecificCulture("ru-RU");
                bool successDate = DateTime.TryParse(reader.ReadLine(), culture, DateTimeStyles.AdjustToUniversal, out itemDateOfManufacturing);
                if (!successDate)
                {
                    itemDateOfManufacturing = new DateTime(0);
                    Console.WriteLine("Некорректно указана дата производства для товара {0}.", itemName);
                }

                uint itemCount;
                bool successCount = uint.TryParse(reader.ReadLine(), out itemCount);
                if (!successCount)
                {
                    itemCount = 0;
                    Console.WriteLine("Некорректно указано количество товара {0}.", itemName);
                }

                string manufacturerName = reader.ReadLine();
                if (manufacturerName.StartsWith("\n") || manufacturerName.StartsWith(" ") || manufacturerName.Length == 0)
                {
                    Console.WriteLine("Некорректно указано название производителя.");
                }
                Shop sShop;
                if (Shops.FindShopByManufacturerName(manufacturerName, out sShop))
                {
                    Item sItem = CreateItem(itemVendorCode, itemName, itemUnit, itemZakupPrice, itemShelfLife, itemCount, itemDateOfManufacturing, sShop.Manufacturer);
                    sShop.AddItem(sItem);
                    itemList.Add(sItem);
                    exists = true;
                }

                string manufacturingCountry = reader.ReadLine();
                if (manufacturingCountry.StartsWith("\n") || manufacturingCountry.StartsWith(" ") || manufacturingCountry.Length == 0)
                {
                    Console.WriteLine("Некорректно указана страна производства для производителя {0}.", manufacturerName);
                }

                string manufacturerAddress = reader.ReadLine();
                if (manufacturerAddress.StartsWith("\n") || manufacturerAddress.StartsWith(" ") || manufacturerAddress.Length == 0)
                {
                    Console.WriteLine("Некорректно указан адрес производителя {0}.", manufacturerAddress);
                }

                Phone manufacturerPhone;
                ulong number;
                bool successNumber = ulong.TryParse(reader.ReadLine(), out number);
                if (!successNumber)
                {
                    number = 0;
                    Console.WriteLine("Некорректно указан номер телефона для производителя {0}", manufacturerName);
                }
                try
                {
                    manufacturerPhone = new Phone(number);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Ошибка в товаре {1}:\n{0}", e.Message, itemName);
                    manufacturerPhone = new Phone(0);
                }
                if (!exists)
                {
                    Manufacturer itemManufacturer = new Manufacturer(manufacturerName, manufacturingCountry, manufacturerAddress, manufacturerPhone);
                    Item item = CreateItem(itemVendorCode, itemName, itemUnit, itemZakupPrice, itemShelfLife, itemCount, itemDateOfManufacturing, itemManufacturer);
                    itemList.Add(item);

                    Shop shop = new Shop(itemManufacturer);
                    shop.AddItem(item);
                    Shops.AddShop(shop);
                }
            }
            reader.Close();
            return itemList;
        }

        private static string Input 
        { 
            get 
            { 
                string input = Console.ReadLine();
                if (input.ToUpper() == "НАЗАД") throw new Exception("Отмена ввода.");
                else return input;
            } 
        }

        private static Item CreateItem(string itemVendorCode, string itemName, string itemUnit, float itemZakupPrice, string itemShelfLife, uint itemCount, DateTime itemDateOfManufacturing, Manufacturer itemManufacturer)
        {
            return new Item(itemVendorCode, itemName, itemUnit, itemZakupPrice, itemShelfLife, itemCount, itemDateOfManufacturing, itemManufacturer);
        }

        public static Item ReadItemFromConsole()
        {
            string itemVendorCode = "";
            string itemName = "";
            string itemUnit = "";
            float itemZakupPrice = 0;
            string itemShelfLife = "";
            DateTime itemDateOfManufacturing = new DateTime(0);
            uint itemCount = 0;
            string manufacturerName = "";
            string manufacturingCountry = "";
            string manufacturerAddress = "";
            Phone manufacturerPhone = new Phone();

            Console.WriteLine("Для отмены ввода и возврата напишите \"назад\".");

            for (int i = 0; i < valuesInput.Length; i++)
            {
                Console.WriteLine(valuesInputStrings[i]);

                switch (valuesInput[i])
                {
                    case "itemVendorCode":
                        {
                            itemVendorCode = Input;
                            if (itemVendorCode.Length == 0) itemVendorCode = Randomizer.RandomVendorCode();
                            while (itemVendorCode.StartsWith("\n") || itemVendorCode.StartsWith(" ") || itemVendorCode.Length != 6)
                            {
                                Console.WriteLine("Некорректно указан артикул. Повторите ввод.");
                                itemVendorCode = Input;
                                if (itemVendorCode.Length == 0) itemVendorCode = Randomizer.RandomVendorCode();
                            }
                            break;
                        }
                    case "itemName":
                        {
                            itemName = Input;
                            while (itemName.StartsWith("\n") || itemName.StartsWith(" ") || itemName.Length == 0)
                            {
                                Console.WriteLine("Имя товара не должно начинаться с пробела или быть пустым. Повторите ввод.");
                                itemName = Input;
                            }
                            break;
                        }
                    case "itemUnit":
                        {
                            itemUnit = Input;
                            while (itemUnit.StartsWith("\n") || itemUnit.StartsWith(" ") || itemUnit.Length == 0)
                            {
                                Console.WriteLine("Единица измерения товара не должна начинаться с пробела или быть пустой. Повторите ввод.");
                                itemUnit = Input;
                            }
                            break;
                        }
                    case "itemZakupPrice":
                        {
                            bool successPrice = float.TryParse(Input, out itemZakupPrice);
                            while (!successPrice)
                            {
                                Console.WriteLine("Некорректно указана закупочная цена. Повторите ввод.");
                                successPrice = float.TryParse(Input, out itemZakupPrice);
                            }
                            break;
                        }
                    case "itemShelfLife":
                        {
                            itemShelfLife = Input;
                            while (itemShelfLife.StartsWith("\n") || itemShelfLife.StartsWith(" ") || itemShelfLife.Length == 0)
                            {
                                Console.WriteLine("Некорректно указан срок годности. Повторите ввод.");
                                itemShelfLife = Input;
                            }
                            break;
                        }
                    case "itemDateOfManufacturing":
                        {
                            IFormatProvider culture = CultureInfo.CreateSpecificCulture("ru-RU");
                            bool successDate = DateTime.TryParse(Input, culture, DateTimeStyles.AdjustToUniversal, out itemDateOfManufacturing);
                            while (!successDate)
                            {
                                Console.WriteLine("Некорректно указана дата производства. Повторите ввод.");
                                successDate = DateTime.TryParse(Input, culture, DateTimeStyles.AdjustToUniversal, out itemDateOfManufacturing);
                            }
                            break;
                        }
                    case "itemCount":
                        {
                            bool successCount = uint.TryParse(Input, out itemCount);
                            while (!successCount)
                            {
                                Console.WriteLine("Некорректно указано количество товара. Повторите ввод.");
                                successCount = uint.TryParse(Input, out itemCount);
                            }
                            break;
                        }
                    case "manufacturerName":
                        {
                            manufacturerName = Input;
                            while (manufacturerName.StartsWith("\n") || manufacturerName.StartsWith(" ") || manufacturerName.Length == 0)
                            {
                                Console.WriteLine("Некорректно указано название производителя. Повторите ввод.");
                                manufacturerName = Input;
                            }
                            Shop sShop;
                            if(Shops.FindShopByManufacturerName(manufacturerName, out sShop))
                            {
                                Console.WriteLine("Производитель {0} уже есть в базе данных, для продолжения ввода данных производителя напишите \"продолжить\",\n" +
                                    "для подстановки значений из базы данных оставьте поле пустым.", sShop.Manufacturer.Name);
                                string input = Input;
                                if(input.ToUpper() == "ПРОДОЛЖИТЬ")
                                {
                                    break;
                                }
                                else
                                {
                                    Item sItem = CreateItem(itemVendorCode, itemName, itemUnit, itemZakupPrice, itemShelfLife, itemCount, itemDateOfManufacturing, sShop.Manufacturer);
                                    sShop.AddItem(sItem);
                                    return sItem;
                                }
                            }
                            break;
                        }
                    case "manufacturingCountry":
                        {
                            manufacturingCountry = Input;
                            while (manufacturingCountry.StartsWith("\n") || manufacturingCountry.StartsWith(" ") || manufacturingCountry.Length == 0)
                            {
                                Console.WriteLine("Некорректно указана страна производства. Повторите ввод.");
                                manufacturingCountry = Input;
                            }
                            break;
                        }
                    case "manufacturerAddress":
                        {
                            manufacturerAddress = Input;
                            while (manufacturerAddress.StartsWith("\n") || manufacturerAddress.StartsWith(" ") || manufacturerAddress.Length == 0)
                            {
                                Console.WriteLine("Некорректно указан адрес производителя. Повторите ввод.");
                                manufacturerAddress = Input;
                            }
                            break;
                        }
                    case "manufacturerPhone":
                        {
                            ulong number;
                            bool successNumber = ulong.TryParse(Input, out number);
                            try
                            {
                                manufacturerPhone = new Phone(number);
                                successNumber = true;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                successNumber = false;
                            }
                            while (!successNumber)
                            {
                                Console.WriteLine("Некорректно указан номер телефона производителя. Повторите ввод.");
                                successNumber = ulong.TryParse(Input, out number);
                                try
                                {
                                    manufacturerPhone = new Phone(number);
                                    successNumber = true;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                    Console.WriteLine("Повторите ввод.");
                                    successNumber = false;
                                }
                            }
                            break;
                        }
                }
            }
            Manufacturer itemManufacturer = new Manufacturer(manufacturerName, manufacturingCountry, manufacturerAddress, manufacturerPhone);
            Item item = CreateItem(itemVendorCode, itemName, itemUnit, itemZakupPrice, itemShelfLife, itemCount, itemDateOfManufacturing, itemManufacturer);

            Shop shop = new Shop(itemManufacturer);
            shop.AddItem(item);
            Shops.AddShop(shop);

            return item;
        }
    }
}
