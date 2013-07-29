using System.Collections.Generic;
using System.Linq;

namespace VectorClock
{
    public class VectorClock
    {
        private readonly Dictionary<string, int> _items;

        private const int DefaultValue = 0;

        public VectorClock()
        {
            _items = new Dictionary<string, int>();
        }

        private VectorClock(VectorClock other) : this(other._items) {}

        private VectorClock(Dictionary<string, int> dictionary)
        {
            _items = dictionary.ToDictionary(e => e.Key, e => e.Value);;
        }

        public void Tick(string key)
        {
            if (_items.ContainsKey(key))
                _items[key] = _items[key] + 1;
            else
                _items.Add(key, 1); // starts at 0, +1
        }

        public string[] Keys
        {
            get { return _items.Keys.ToArray(); }
        }

        private int[] Values
        {
            get { return _items.Values.ToArray(); }
        }

        public int GetValue(string key)
        {
            return _items.ContainsKey(key) ? _items[key] : DefaultValue;
        }

        public bool Contains(string key)
        {
            return _items.ContainsKey(key);
        }

        private void Put(string key, int value)
        {
            if (!_items.ContainsKey(key))
                _items.Add(key, value);
            else
                _items[key] = value;
        }

        private bool IsDefaultValue(string key)
        {
            return GetValue(key) == DefaultValue;
        }

        public static VectorClock Merge(VectorClock one, VectorClock other)
        {
            var result = new VectorClock(one);

            foreach (var k in other.Keys)
            {
                if (!result.Contains(k) || result.GetValue(k) < other.GetValue(k))
                {
                    result.Put(k, other.GetValue(k));
                }
            }

            return result;
        }

        public enum VectorComparision
        {
            Greater,    // if one > other
            Equal,      // if one == other
            Smaller,    // if one < other
            Simultanous // if one <> other
        }

        public VectorComparision Compare(VectorClock one, VectorClock other)
        {
            var equal = true;
            var greater = true;
            var smaller = true;

            foreach (var k in one.Keys)
            {
                var oneValue = one.GetValue(k);

                if (other.Contains(k))
                {
                    var otherValue = other.GetValue(k);

                    if (oneValue < otherValue)
                    {
                        equal = false;
                        greater = false;
                    }

                    if (oneValue > otherValue)
                    {
                        equal = false;
                        smaller = false;
                    }
                }
                else if (oneValue != DefaultValue)
                {
                    equal = false;
                    smaller = false;
                }
            }

            foreach (var k in other.Keys)
            {
                if (!one.Contains(k) && !other.IsDefaultValue(k))
                {
                    equal = false;
                    greater = false;
                }
            }

            if (equal)
                return VectorComparision.Equal;
            if (greater)
                return VectorComparision.Greater;
            if (smaller)
                return VectorComparision.Smaller;
            else
                return VectorComparision.Simultanous;
        }
    }
}
