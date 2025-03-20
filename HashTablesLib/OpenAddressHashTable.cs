using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HashTablesLib
{
    public class OpenAddressHashTable<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue> where TKey: IEquatable<TKey>
    {
        Pair<TKey, TValue>[] _table;
        private int _capacity;
        HashMaker<TKey> _hashMaker1, _hashMaker2;
        public int Count { get; private set; }
        public bool IsReadOnly {  get; private set; }

        private const double FillFactor = 0.7;
        private static readonly GetPrimeNumber _primeNumber = new GetPrimeNumber();

        public OpenAddressHashTable() : this(_primeNumber.GetMin()) { }
        public OpenAddressHashTable(int m)
        {
            _capacity = m;
            _table = new Pair<TKey, TValue>[_capacity];
            _hashMaker1 = new HashMaker<TKey>(_capacity);
            _hashMaker2 = new HashMaker<TKey>(_capacity - 1);
            Count = 0;
        }
        public void Add(TKey key, TValue value)
        {
            var hash1 = _hashMaker1.ReturnHash(key);

            if (!TryToPut(hash1, key, value)) // ячейка занята
            {
                var hash2 = _hashMaker2.ReturnHash(key);
                int iterationNumber = 1;
                while (true) 
                {
                    var place = (hash1 + iterationNumber * (1 + hash2)) % _capacity;
                    if (TryToPut(place, key, value))
                        break;
                    iterationNumber++;
                    if (iterationNumber >= _capacity)
                        throw new ApplicationException("HashTable full!!!");
                }
            }
            if ((double) Count / _capacity >= FillFactor)
            {
                IncreaseTable();    
            }
        }

        private bool TryToPut(int place, TKey key, TValue value)
        {
            if (_table[place] == null || _table[place].IsDeleted())
            {
                _table[place] = new Pair<TKey, TValue>(key, value);
                Count++;
                return true;
            }
            if (_table[place].Key.Equals(key))
            {
                throw new ArgumentException();
            }
            return false;
        }

        private Pair<TKey,TValue> Find(TKey x)
        {
            var hash = _hashMaker1.ReturnHash(x);
            if (_table[hash] == null)
                return null;
            if (!_table[hash].IsDeleted() && _table[hash].Key.Equals(x))
            {
                return _table[hash];
            }
            int iterationNumber = 1;
            while (true)
            {
                var place = (hash + iterationNumber * (1 + _hashMaker2.ReturnHash(x))) % _capacity;
                if (_table[place] == null)
                    return null;
                if (!_table[place].IsDeleted() && _table[place].Key.Equals(x))
                {
                    return _table[place];
                }
                iterationNumber++;
                if (iterationNumber >= _capacity)
                    return null;
            }
        }
        public TValue this[TKey key]
        {
            get
            {
                var pair = Find(key);
                if (pair == null)
                    throw new KeyNotFoundException();
                return pair.Value;
            }

            set
            {
                var pair = Find(key);
                if (pair == null)
                {
                    Add(key, value);
                    return;
                }
                pair.Value = value;
            }
        }

        private void IncreaseTable()
        {
            _capacity = _primeNumber.Next();
            var oldTable = _table;
            _table = new Pair<TKey, TValue>[_capacity];
            _hashMaker1 = new HashMaker<TKey>(_capacity);
            _hashMaker2 = new HashMaker<TKey>(_capacity - 1);
            Count = 0;
            foreach (var pair in oldTable)
            {
                if (pair != null && !pair.IsDeleted())
                {
                    Add(pair);
                }
            }
        }

        public bool ContainsKey(TKey key)
        {
            return Find(key) != null;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            //return _table.Where(x => x != null && !x.IsDeleted()).Select(x => new KeyValuePair<TKey, TValue>(x.Key, x.Value)).ToList().GetEnumerator();
            for (int i = 0; i < _capacity; i++)
            {
                if (_table[i] != null && !_table[i].IsDeleted())
                {
                    yield return new KeyValuePair<TKey, TValue>(_table[i].Key, _table[i].Value);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key is null ! ! !");
            }
            var item = Find(key);
            if (item == null)
            {
                return false;
            }
            item.DeletePair();
            Count--;
            return true;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var pair = Find(key);
            value = pair == null ? default(TValue) : pair.Value;
            return pair != null;
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Add(Pair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _capacity = _primeNumber.GetMin();
            _table = new Pair<TKey, TValue>[_capacity];
            _hashMaker1 = new HashMaker<TKey>(_capacity);
            _hashMaker2 = new HashMaker<TKey>(_capacity - 1);
            Count = 0;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ContainsKey(item.Key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            var tableArray = (from pair in _table where pair != null && !pair.IsDeleted() select new KeyValuePair<TKey, TValue>(pair.Key, pair.Value)).ToArray();
            tableArray.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }
        public ICollection<TKey> Keys => (from pair in _table where pair != null && !pair.IsDeleted() select pair.Key).ToList();

        public ICollection<TValue> Values => (from pair in _table where pair != null && !pair.IsDeleted() select pair.Value).ToList();
    }
}
