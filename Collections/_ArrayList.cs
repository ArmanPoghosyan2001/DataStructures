using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    public class _ArrayList
    {
        private Object[] _items;

        [ContractPublicPropertyName("Count")]
        private int _size;
        private int _version;
        [NonSerialized]

        private const int _defaultCapacity = 4;
        //private static readonly Object[] emptyArray = EmptyArray<Object>.Empty;

        public _ArrayList(int capacity)
        {
            _items = new Object[capacity];
        }
        public virtual int Capacity
        {
            get
            {
                return _items.Length;
            }
            set
            {
                if (value != _items.Length)
                {
                    if (value > 0)
                    {
                        Object[] newItems = new Object[value];
                        if (_size > 0)
                        {
                            Array.Copy(_items, 0, newItems, 0, _size);
                        }
                        _items = newItems;
                    }
                    else
                    {
                        _items = new Object[_defaultCapacity];
                    }
                }
            }

        }
        public virtual int Count
        {
            get
            {
                return _size;
            }
        }

        public virtual Object this[int index]
        {
            get
            {
                return _items[index];
            }
            set
            {
                _items[index] = value;
                _version++;
            }
        }
        public virtual int Add(Object value)
        {
            if (_size == _items.Length) EnsureCapacity(_size + 1);
            _items[_size] = value;
            _version++;
            return _size++;
        }

        public virtual void AddRange(ICollection c)
        {
            InsertRange(_size, c);
        }
        private void EnsureCapacity(int min)
        {
            if (_items.Length < min)
            {
                int newCapacity = _items.Length == 0 ? _defaultCapacity : _items.Length * 2;
                if ((uint)newCapacity > 0X7FEFFFFF) newCapacity = 0X7FEFFFFF;
                if (newCapacity < min) newCapacity = min;
                Capacity = newCapacity;
            }
        }
        public virtual void Clear()
        {
            if (_size > 0)
            {
                Array.Clear(_items, 0, _size);
                _size = 0;
            }
            _version++;
        }
        public virtual bool Contains(Object item)
        {
            if (item == null)
            {
                for (int i = 0; i < _size; i++)
                    if (_items[i] == null)
                        return true;
                return false;
            }
            else
            {
                for (int i = 0; i < _size; i++)
                    if ((_items[i] != null) && (_items[i].Equals(item)))
                        return true;
                return false;
            }
        }
        public virtual int IndexOf(Object value)
        {
            return Array.IndexOf((Array)_items, value, 0, _size);
        }

        public virtual int IndexOf(Object value, int startIndex)
        {
            return Array.IndexOf((Array)_items, value, startIndex, _size - startIndex);
        }
        public virtual int IndexOf(Object value, int startIndex, int count)
        {
            return Array.IndexOf((Array)_items, value, startIndex, count);
        }
        public virtual void Insert(int index, Object value)
        {

            if (_size == _items.Length) EnsureCapacity(_size + 1);
            if (index < _size)
            {
                Array.Copy(_items, index, _items, index + 1, _size - index);
            }
            _items[index] = value;
            _size++;
            _version++;
        }
        public virtual void InsertRange(int index, ICollection c)
        {
            int count = c.Count;
            if (count > 0)
            {
                EnsureCapacity(_size + count);
                if (index < _size)
                {
                    Array.Copy(_items, index, _items, index + count, _size - index);
                }

                Object[] itemsToInsert = new Object[count];
                c.CopyTo(itemsToInsert, 0);
                itemsToInsert.CopyTo(_items, index);
                _size += count;
                _version++;
            }
        }
        public virtual int LastIndexOf(Object value, int startIndex)
        {
            Contract.Ensures(Contract.Result<int>() < Count);
            Contract.EndContractBlock();
            return LastIndexOf(value, startIndex, startIndex + 1);
        }
        public virtual int LastIndexOf(Object value, int startIndex, int count)
        {
            if (_size == 0)
                return -1;

            return Array.LastIndexOf((Array)_items, value, startIndex, count);
        }
        public virtual void Remove(Object obj)
        {

            int index = IndexOf(obj);
            if (index >= 0)
                RemoveAt(index);
        }

        public virtual void RemoveAt(int index)
        {
            _size--;
            if (index < _size)
            {
                Array.Copy(_items, index + 1, _items, index, _size - index);
            }
            _items[_size] = null;
            _version++;
        }
        public virtual void RemoveRange(int index, int count)
        {
            if (count > 0)
            {
                int i = _size;
                _size -= count;
                if (index < _size)
                {
                    Array.Copy(_items, index + count, _items, index, _size - index);
                }
                while (i > _size) _items[--i] = null;
                _version++;
            }
        }
    }
}
