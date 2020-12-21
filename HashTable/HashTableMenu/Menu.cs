using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashTableTest;

namespace HashTable.HashTableMenu
{
    public static class Menu
    {
        private static List<String> MenuItems { get; } = new List<string>()
        {
            "Create new hashtable",
            "Add new data",
            "Remove data",
            "Search for data",
            "Update data",
            "Clear hashtable",
            "Show hashtable",
            "Get object hash",
            "Exit"
        };
        private static HashTable<String> HashTable { get; set; } = new HashTable<String>();
        public static void CreateHashTable(int length)
        {
            if (HashTable != null) HashTable.Clear();
            HashTable = new HashTable<string>(length);
        }
        private static void Show()
        {
            for (var i = 0; i < MenuItems.Count; i++)
            {
                Console.WriteLine($"{(i + 1).ToString().PadLeft(MenuItems.Count.ToString().Length)}. {MenuItems[i]}");
            }
            Console.WriteLine();
        }
        private static int ReadCommand()
        {
            var line = String.Empty;
            do
            {
                Console.Write("Enter menu item number: ");
                line = Console.ReadLine();
            } while (line == String.Empty || !Int32.TryParse(line, out int res) || Int32.Parse(line) < 1 || Int32.Parse(line) > MenuItems.Count);
            Console.WriteLine();

            return Int32.Parse(line);
        }
        private static int ReadLength()
        {
            var line = String.Empty;
            do
            {
                Console.Write("Enter hashtable length: ");
                line = Console.ReadLine();
            } while (line == String.Empty || !Int32.TryParse(line, out int res));

            return Int32.Parse(line);
        }
        private static void ExecuteCommand(int commandID)
        {
            switch (commandID)
            {
                case 1:
                    {
                        CreateHashTable(ReadLength());
                        Console.WriteLine("Hashtable has been created!\n");
                        break;
                    }
                case 2:
                    {
                        Console.Write("Enter line to add it into a hashtable: ");
                        try
                        {
                            HashTable.Add(Console.ReadLine());
                            Console.WriteLine("Data has been added into a hashtable!\n");
                        }
                        catch
                        {
                            Console.WriteLine("Data has not been added into a hashtable!\n");
                        }
                        break;
                    }
                case 3:
                    {
                        Console.Write("Enter line to remove it from hashtable: ");
                        if (HashTable.Remove(Console.ReadLine()))
                            Console.WriteLine("Data has been removed from hashtable!\n");
                        else
                            Console.WriteLine("Hashtable does not contain the same data!\n");
                        break;
                    }
                case 4:
                    {
                        Console.Write("Enter line to search it in a hashtable: ");
                        Console.WriteLine(HashTable.Search(Console.ReadLine()));
                        break;
                    }
                case 5:
                    {
                        Console.Write("Enter line to be updated: ");
                        var firstLine = Console.ReadLine();
                        Console.Write("Enter new line: ");
                        var secondLine = Console.ReadLine();
                        try
                        {
                            var updated = HashTable.Update(firstLine, secondLine);
                            if (updated)
                                Console.WriteLine("Data has been updated!\n");
                            else
                                Console.WriteLine("Data has not been updated!\n");
                        }
                        catch
                        {
                            Console.WriteLine("Lines must have the same hash!\n");
                        }
                        break;
                    }
                case 6:
                    {
                        if (HashTable.IsEmpty)
                            Console.WriteLine("Hashtable is empty!\n");
                        else
                        {
                            HashTable.Clear();
                            Console.WriteLine("Hashtable has been cleared!\n");
                        }
                        break;
                    }
                case 7:
                    {
                        Console.WriteLine(HashTable);
                        break;
                    }
                case 8:
                    {
                        Console.Write("Enter line to see its hash: ");
                        Console.WriteLine($"Data hash is: {HashTable.GetHash(Console.ReadLine())}");
                        break;
                    }
                case 9:
                    {
                        Environment.Exit(0);
                        break;
                    }
            }
            Console.ReadKey();
        }
        public static void Run()
        {
            while (true)
            {
                Show();
                ExecuteCommand(ReadCommand());
                Console.Clear();
            }
        }
    }
}
