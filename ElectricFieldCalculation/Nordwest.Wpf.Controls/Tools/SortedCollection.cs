using System.Collections.Generic;

namespace Nordwest.Wpf.Controls
{
    
    /// <summary>
    /// Collections that holds elements in the specified order. The complexity and efficiency
    /// of the algorithm is comparable to the SortedList from .NET collections. In contrast 
    /// to the SortedList SortedCollection accepts redundant elements. If no comparer is 
    /// is specified the list will use the default comparer for given type.
    /// </summary>
    /// <author>consept</author>
    public class SortedCollection<TValue> : List<TValue>
    {
        // Fields
        private readonly IComparer<TValue> _comparer;

        // Constructors
        public SortedCollection()
        {
            this._comparer = Comparer<TValue>.Default;
        }

        public SortedCollection(IComparer<TValue> comparer)
        {
            this._comparer = comparer;
        }

        public new void Add(TValue value)
        {
            // check where the element should be placed
            var index = this.BinarySearch(value, this._comparer);
            if (index < 0)// xor
                index = ~index;
            Insert(index, value);
        }
    }
}