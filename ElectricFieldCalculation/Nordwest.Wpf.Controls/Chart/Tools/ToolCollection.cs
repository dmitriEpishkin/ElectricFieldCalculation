using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Nordwest.Wpf.Controls.Properties;

namespace Nordwest.Wpf.Controls.Chart
{
    public class ToolCollection : IDictionary<Type, Tool>, INotifyCollectionChanged
    {
        private readonly Dictionary<Type, Tool> _dictionary = new Dictionary<Type, Tool>();

        #region Implementation of IEnumerable

        public IEnumerator<KeyValuePair<Type, Tool>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<KeyValuePair<Type,Tool>>

        public void Add(KeyValuePair<Type, Tool> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _dictionary.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(KeyValuePair<Type, Tool> item)
        {
            return ContainsKey(item.Key);
        }

        public void CopyTo(KeyValuePair<Type, Tool>[] array, int arrayIndex)
        {
            var source = _dictionary.ToArray();
            source.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<Type, Tool> item)
        {
            return Remove(item.Key);
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region Implementation of IDictionary<Type,Tool>

        public bool ContainsKey(Type key)
        {
            return _dictionary.ContainsKey(key);
        }

        public void Add(Type key, Tool value)
        {
            if (key != value.GetType())
                throw new ArgumentException(Resources.ToolCollection_Add_WrongTypeArgumentException);
            _dictionary.Add(key, value);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,new List<Tool>{value}));
        }

        public bool Remove(Type key)
        {
            var oldItem = _dictionary[key];
            var ret = _dictionary.Remove(key);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove,  new List<Tool>{oldItem}));
            return ret;
        }

        public bool TryGetValue(Type key, out Tool value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public Tool this[Type key]
        {
            get { return _dictionary[key]; }
            set
            {
                if (key != value.GetType())
                    throw new ArgumentException(Resources.ToolCollection_Add_WrongTypeArgumentException);
                var oldValue = _dictionary[key];
                _dictionary[key] = value;
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, new List<Tool> { oldValue }, new List<Tool>{value}));
            }
        }

        public ICollection<Type> Keys
        {
            get { return _dictionary.Keys; }
        }

        public ICollection<Tool> Values
        {
            get { return _dictionary.Values; }
        }

        #endregion

        public void Add(Tool value)
        {
            var key = value.GetType();
            Add(key,value);
        }

        #region Implementation of INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler handler = CollectionChanged;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}