using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tovar
{
    class Program
    {
        private static SaveManager itemSaver = new SaveManager("items.txt");

        private static List<Item> items = new List<Item>();

        public static void SaveItems()
        {
            itemSaver.CreateFile();
            foreach (Item item in items)
                itemSaver.WriteObject(item);
        }

        public static void LoadItems()
        {
            items.Clear();
            LoadManager itemLoader = new LoadManager("items.txt");
            itemLoader.BeginRead();
            while (itemLoader.IsLoading)
                items.Add(itemLoader.Read(new Item.Loader()));
            itemLoader.EndRead();
        }

        static void Main(string[] args)
        {
            while (true)
            {
                if (items.Count > 0) Console.WriteLine("Всего товаров: {0}.", items.Count);
                Console.WriteLine("Список доступных команд: ");
                Console.WriteLine("Для ввода товаров через консоль напишите \"консоль\", для ввода товаров из файла напишите \"файл\";");
                Console.WriteLine("Для загрузки данных напишите \"загрузить\";");
                if (items.Count > 0)
                {
                    Console.WriteLine("Для вывода товаров в документ Excel напишите \"документ\";");
                    Console.WriteLine("Для сохранения данных напишите \"сохранить\";");
                    Console.WriteLine("для очистки списка товаров напишите \"очистить\";");
                }
                Console.Write("Для выхода из программы напишите \"выход\".");
                Console.WriteLine();
                string command = Console.ReadLine().ToUpper();
                switch (command)
                {
                    case "СОХРАНИТЬ":
                        {
                            if (items.Count == 0 && Shops.GetShops.Count == 0) break;
                            Shops.SaveManufacturers();
                            SaveItems();
                            break;
                        }

                    case "ЗАГРУЗИТЬ":
                        {
                            Shops.LoadManufacturers();
                            LoadItems();
                            break;
                        }

                    case "КОНСОЛЬ":
                        {
                            try
                            {
                                Item item = ItemReader.ReadItemFromConsole();
                                items.Add(item);
                                Console.WriteLine("Товар добавлен.");
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine(e.Message);
                                break;
                            }
                            break;
                        }
                    case "ФАЙЛ":
                        {
                            while(true)
                            {
                                try
                                {
                                    Console.WriteLine("Для возврата напишите \"назад\".");
                                    Console.WriteLine("Имя файла (с .txt на конце): ");
                                    string fileName = Console.ReadLine();
                                    if (fileName.ToUpper() == "НАЗАД") break;
                                    List<Item> list = ItemReader.ReadItemsFromFile(fileName);
                                    for(int i = 0; i < list.Count; i++)
                                    {
                                        items.Add(list[i]);
                                    }
                                    Console.WriteLine("Товаров в файле: {0}.", list.Count);
                                    break;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                            break;
                        }
                    case "ДОКУМЕНТ":
                        {
                            if (items.Count == 0) break;
                            while (true)
                            {
                                try
                                {
                                    Console.WriteLine("Имя файла (с .xlsx на конце): ");
                                    string fileName = Console.ReadLine();
                                    ItemWriter.WriteItemsToExcelDocument(items, fileName);
                                    Console.WriteLine("Товары успешно выведены в документ.");
                                    break;
                                }
                                catch(Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                            break;
                        }
                    case "ОЧИСТИТЬ":
                        {
                            if (items.Count == 0) break;
                            items.Clear();
                            Console.WriteLine("Список товаров очищен.");
                            break;
                        }
                    case "ВЫХОД":
                        {
                            System.Environment.Exit(1);
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Неизвестная команда.");
                            break;
                        }
                } 
            }
        }
    }
}
