using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTableTest
{
    public struct SearchingResult
    {
        public bool IsFound { get; }
        public int ComparisonCount { get; }
        public int Hash { get; }
        public SearchingResult(bool isFound, int comparisonCount, int hash)
        {
            IsFound = isFound;
            ComparisonCount = comparisonCount;
            Hash = hash;
        }
        public override bool Equals(object obj)
        {
            return obj is SearchingResult other &&
                IsFound == other.IsFound &&
                ComparisonCount == other.ComparisonCount &&
                Hash == other.Hash;
        }
        public override int GetHashCode()
        {
            int hashCode = 85648994;
            hashCode = hashCode * -1521134295 + IsFound.GetHashCode();
            hashCode = hashCode * -1521134295 + ComparisonCount.GetHashCode();
            hashCode = hashCode * -1521134295 + Hash.GetHashCode();
            return hashCode;
        }
        public override string ToString()
        {
            return $"Item hash: {Hash}\nItem searching state: {IsFound}\nTotal comparisons: {ComparisonCount}";
        }
        public void Deconstruct(out bool isFound, out int comparisonCount, out int hash)
        {
            isFound = IsFound;
            comparisonCount = ComparisonCount;
            hash = Hash;
        }
        public static implicit operator (bool IsFound, int ComparisonCount, int Hash)(SearchingResult value)
        {
            return (value.IsFound, value.ComparisonCount, value.Hash);
        }
        public static implicit operator SearchingResult((bool IsFound, int ComparisonCount, int Hash) value)
        {
            return new SearchingResult(value.IsFound, value.ComparisonCount, value.Hash);
        }
    }
}
