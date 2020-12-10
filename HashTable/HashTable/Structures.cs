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
        public SearchingResult(bool isFound, int comparisonCount)
        {
            IsFound = isFound;
            ComparisonCount = comparisonCount;
        }
        public override bool Equals(object obj)
        {
            return obj is SearchingResult other &&
                IsFound == other.IsFound &&
                ComparisonCount == other.ComparisonCount;
        }
        public override int GetHashCode()
        {
            int hashCode = 85648994;
            hashCode = hashCode * -1521134295 + IsFound.GetHashCode();
            hashCode = hashCode * -1521134295 + ComparisonCount.GetHashCode();
            return hashCode;
        }
        public override string ToString()
        {
            return $"Item searching state: {IsFound}\nTotal comparisons: {ComparisonCount}";
        }
        public void Deconstruct(out bool isFound, out int comparisonCount)
        {
            isFound = IsFound;
            comparisonCount = ComparisonCount;
        }
        public static implicit operator (bool IsFound, int ComparisonCount)(SearchingResult value)
        {
            return (value.IsFound, value.ComparisonCount);
        }
        public static implicit operator SearchingResult((bool IsFound, int ComparisonCount) value)
        {
            return new SearchingResult(value.IsFound, value.ComparisonCount);
        }
    }
}
