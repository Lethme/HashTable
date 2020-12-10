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

            Table.Sort(SortType.Descending);
            
            Console.WriteLine(Table.AsIndentedString());
            Table.Update(5, 9);
            Console.WriteLine(Table.AsIndentedString());
        }
    }
}
