using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Helper.Collections
{
    /// <summary>
    /// A concrete implementation of the abstract KeyedCollection class using lambdas for the
    /// implementation.
    /// </summary>
    public class KeyedCollection2<TKey, TItem> : KeyedCollection<TKey, TItem>
    {
        private const string DELEGATE_NULL_EXCEPTION_MESSAGE = "Delegate passed cannot be null";
        private readonly Func<TItem, TKey> _getKeyForItemFunction;

        public KeyedCollection2(Func<TItem, TKey> getKeyForItemFunction)
        {
            if (getKeyForItemFunction == null) throw new ArgumentNullException(DELEGATE_NULL_EXCEPTION_MESSAGE);
            _getKeyForItemFunction = getKeyForItemFunction;
        }

        public KeyedCollection2(Func<TItem, TKey> getKeyForItemDelegate, IEqualityComparer<TKey> comparer) : base(comparer)
        {
            if (getKeyForItemDelegate == null) throw new ArgumentNullException(DELEGATE_NULL_EXCEPTION_MESSAGE);
            _getKeyForItemFunction = getKeyForItemDelegate;
        }

        protected override TKey GetKeyForItem(TItem item)
        {
            return _getKeyForItemFunction(item);
        }

        public void SortByKeys()
        {
            var comparer = Comparer<TKey>.Default;
            SortByKeys(comparer);
        }

        public void SortByKeys(IComparer<TKey> keyComparer)
        {
            var comparer = new Comparer2<TItem>((x, y) => keyComparer.Compare(GetKeyForItem(x), GetKeyForItem(y)));
            Sort(comparer);
        }

        public void SortByKeys(Comparison<TKey> keyComparison)
        {
            var comparer = new Comparer2<TItem>((x, y) => keyComparison(GetKeyForItem(x), GetKeyForItem(y)));
            Sort(comparer);
        }

        public void Sort()
        {
            var comparer = Comparer<TItem>.Default;
            Sort(comparer);
        }

        public void Sort(Comparison<TItem> comparison)
        {
            Sort(new Comparer2<TItem>(comparison));
        }

        public void Sort(IComparer<TItem> comparer)
        {
            var list = Items as List<TItem>;
            list?.Sort(comparer);
        }
    }
}
