using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HashTableTest
{
    public class HashTable<T> : ICollection<T> where T : IEquatable<T>, IComparable<T>
    {
        private LinkedList<T>[] Items { get; }
        public int Length { get; private set; } = 50;
        public int Count => TableItems.Count();
        public bool IsEmpty => Items.Any(list => list == null);
        public int TotalCount => TableItems.Select(data => data.Count).Aggregate((x, y) => x + y);
        public IEnumerable<LinkedList<T>> TableItems => Items.Where(item => item != null && item.Count != 0);
        public IEnumerable<T> TableData => TableItems.Select(list => list.AsEnumerable()).Aggregate((x, y) => x.Concat(y));
        public IEnumerable<int> Keys => Items.IndexesWhere(item => item != null && item.Count != 0);
        public IEnumerable<int> UnusedKeys => Items.IndexesWhere(item => item == null || item.Count == 0);
        public bool IsReadOnly => Items.IsReadOnly;
        public HashTable() { this.Items = new LinkedList<T>[Length]; }
        public HashTable(int length)
        {
            if (length <= 0) throw new ArgumentOutOfRangeException($"Hash table length must be more than zero.");

            this.Length = length;
            this.Items = new LinkedList<T>[Length];
        }
        public HashTable(int length, params T[] objSequence)
        {
            if (length <= 0) throw new ArgumentOutOfRangeException($"Hash table length must be more than zero.");

            this.Length = length;
            this.Items = new LinkedList<T>[Length];
            this.Add(objSequence);
        }
        public HashTable(int length, IEnumerable<T> objCollection)
        {
            if (length <= 0) throw new ArgumentOutOfRangeException($"Hash table length must be more than zero.");

            this.Length = length;
            this.Items = new LinkedList<T>[Length];
            this.Add(objCollection);
        }
        public IEnumerable<T> this[int index]
        {
            get
            {
                foreach (var data in Items[index])
                {
                    yield return data;
                }
            }
        }
        public IEnumerable<T> GetObjectList(T obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            return this[GetHash(obj)];
        }
        public void Add(T obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            
            var objectHash = GetHash(obj);
            if (Items[objectHash] == null)
            {
                Items[objectHash] = new LinkedList<T>();
            }

            if (Items[objectHash].Contains(obj))
            {
                throw new ArgumentException($"Tried to add an object which already exist in hash table.");
            }
            else
            {
                Items[objectHash].AddFirst(obj);
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
        public bool Update(T currentObj, T newObj)
        {
            if (currentObj == null) throw new ArgumentNullException(nameof(currentObj));
            if (newObj == null) throw new ArgumentNullException(nameof(newObj));

            var currentObjHash = GetHash(currentObj);
            var newObjHash = GetHash(newObj);

            if (currentObjHash != newObjHash) throw new ArgumentException($"Arguments had different hash codes");

            if (this.Contains(currentObj))
            {
                var item = Items[currentObjHash];
                item.Find(currentObj).Value = newObj;
                return true;
            }

            return false;
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
            if (Items[objectHash] == null) return (false, 0, objectHash);

            var count = 0;
            foreach (var data in Items[objectHash])
            {
                count++;
                if (data.Equals(obj)) return (true, count, objectHash);
            }

            return (false, count, objectHash);
        }
        public void Clear()
        {
            for (var i = 0; i < Items.Length; i++)
            {
                if (Items[i] != null)
                {
                    Items[i].Clear();
                    Items[i] = null;
                }
            }
        }
        public void CopyTo(T[] array, int arrayIndex = 0)
        {
            Items.CopyTo(array, arrayIndex);
        }
        public int GetHash(T obj)
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
        public override string ToString()
        {
            var result = String.Empty;
            for (var i = 0; i < Items.Length; i++)
            {
                result += $"{i}".PadLeft(Items.Length.ToString().Length) + ": ";
                if (Items[i] == null || Items[i].Count == 0) result += "-\n";
                else result += $"{Items[i].AsString(", ")}\n";
            }

            return result;
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
