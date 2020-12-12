using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTableTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var Table = HashTable.Create(10, 5, 10, 16, 4, 109, 1245, 3563357, 343, 8, 15);

            Console.WriteLine(Table.AsIndentedString());
            Console.WriteLine(Table.Search(109));
        }
    }
}
