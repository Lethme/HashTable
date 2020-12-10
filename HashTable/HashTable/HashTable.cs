using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HashTableTest
{
    public class HashTable<T> : IEnumerable<T> where T : IEquatable<T>, IComparable<T>
    {
        private List<T>[] Items { get; }
        public int Length { get; private set; } = 50;
        public int Count => TableItems.Count();
        public int TotalCount => TableItems.Select(data => data.Count).Aggregate((x, y) => x + y);
        public IEnumerable<List<T>> TableItems => Items.Where(item => item != null && item.Count != 0);
        public IEnumerable<T> TableData => TableItems.Aggregate((x, y) => x.Concat(y).ToList());
        public HashTable() { this.Items = new List<T>[Length]; }
        public HashTable(int length) 
        {
            if (length <= 0) throw new ArgumentOutOfRangeException($"Hash table length must be more than zero.");

            this.Length = length;
            this.Items = new List<T>[Length];
        }
        public HashTable(int length, params T[] objSequence)
        {
            if (length <= 0) throw new ArgumentOutOfRangeException($"Hash table length must be more than zero.");

            this.Length = length;
            this.Items = new List<T>[Length];
            this.Add(objSequence);
        }
        public HashTable(int length, IEnumerable<T> objCollection)
        {
            if (length <= 0) throw new ArgumentOutOfRangeException($"Hash table length must be more than zero.");

            this.Length = length;
            this.Items = new List<T>[Length];
            this.Add(objCollection);
        }
        public void Add(T obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            
            var objectHash = GetHash(obj);
            if (Items[objectHash] == null)
            {
                Items[objectHash] = new List<T>();
            }

            if (Items[objectHash].Contains(obj))
            {
                throw new ArgumentException($"Tried to add an object which already exist in hash table.");
            }
            else
            {
                Items[objectHash].Add(obj);
            }
        }
        public void Add(IEnumerable<T> objColletion)
        {
            foreach (var obj in objColletion) this.Add(obj);
        }
        public void Add(params T[] objSequence)
        {
            foreach (var obj in objSequence) this.Add(obj);
        }
        public bool Remove(T obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var objectHash = GetHash(obj);
            if (Items[objectHash] == null || !Items[objectHash].Contains(obj)) return false;
            if (Items[objectHash].Count == 1)
            {
                Items[objectHash].Clear();
                Items[objectHash] = null;
            }
            else
            {
                Items[objectHash].Remove(obj);
            }

            return true;
        }
        public int Remove(IEnumerable<T> objColletion)
        {
            var count = 0;
            foreach (var obj in objColletion)
            {
                if (this.Remove(obj)) count++;
            }

            return count;
        }
        public int Remove(params T[] objSequence)
        {
            var count = 0;
            foreach (var obj in objSequence)
            {
                if (this.Remove(obj)) count++;
            }

            return count;
        }
        public bool Contains(T obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var objectHash = GetHash(obj);
            if (Items[objectHash] != null && Items[objectHash].Contains(obj)) return true;

            return false;
        }
        public SearchingResult Search(T obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var objectHash = GetHash(obj);
            if (Items[objectHash] == null) return (false, 0);

            var count = 0;
            foreach (var data in Items[objectHash])
            {
                count++;
                if (data.Equals(obj)) return (true, count);
            }

            return (false, count);
        }
        public void Sort(SortType sortType = SortType.Ascending)
        {
            foreach (var item in TableItems)
            {
                switch (sortType)
                {
                    case SortType.Ascending: item.Sort((x, y) => x.CompareTo(y)); break;
                    case SortType.Descending: item.Sort((x, y) => y.CompareTo(x)); break;
                    default: throw new ArgumentException(nameof(sortType));
                }
            }
        }
        public void Sort(Comparison<T> comparison)
        {
            foreach (var item in TableItems)
            {
                item.Sort(comparison);
            }
        }
        public void Sort(IComparer<T> comparer)
        {
            foreach (var item in TableItems)
            {
                item.Sort(comparer);
            }
        }
        public void Sort(int index, int count, IComparer<T> comparer)
        {
            foreach (var item in TableItems)
            {
                item.Sort(index, count, comparer);
            }
        }
        private int GetHash(T obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            return JsonConvert.SerializeObject(obj).Length % Length;
        }
        public IEnumerator<T> GetEnumerator()
        {
            return TableData.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return TableData.GetEnumerator();
        }
    }
    public static class HashTable
    {
        public static HashTable<T> Create<T>() where T : IEquatable<T>, IComparable<T> => new HashTable<T>();
        public static HashTable<T> Create<T>(int length) where T : IEquatable<T>, IComparable<T> => new HashTable<T>(length);
        public static HashTable<T> Create<T>(int length, params T[] objSequence) where T : IEquatable<T>, IComparable<T> => new HashTable<T>(length, objSequence);
        public static HashTable<T> Create<T>(int length, IEnumerable<T> objCollection) where T : IEquatable<T>, IComparable<T> => new HashTable<T>(length, objCollection);
    }
}
