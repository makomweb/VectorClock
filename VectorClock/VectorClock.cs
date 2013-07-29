using System.Collections.Generic;
using System.Linq;

namespace VectorClock
{
    public class VectorClock
    {
        private const int DefaultValue = 0;

        public VectorClock()
        {
            Map = new Dictionary<string, int>();
        }

        public VectorClock(VectorClock other) : this(other.Map) {}

        private VectorClock(Dictionary<string, int> dictionary)
        {
            Map = dictionary.ToDictionary(e => e.Key, e => e.Value);
        }

        public Dictionary<string, int> Map { get; private set; }

        public void Tick(string key)
        {
            if (Map.ContainsKey(key))
                Map[key] = Map[key] + 1;
            else
                Map.Add(key, 1); // starts at 0, +1
        }

        public string[] Keys
        {
            get { return Map.Keys.ToArray(); }
        }

        public int GetValue(string key)
        {
            return Map.ContainsKey(key) ? Map[key] : DefaultValue;
        }

        public bool Contains(string key)
        {
            return Map.ContainsKey(key);
        }

        private bool IsDefaultValue(string key)
        {
            return GetValue(key) == DefaultValue;
        }

        public void Merge(VectorClock other)
        {
            var temp = new VectorClock(Map);
            
            foreach (var k in other.Keys)
            {
                if (!temp.Contains(k) || temp.GetValue(k) < other.GetValue(k))
                {
                    temp.Put(k, other.GetValue(k));
                }
            }

            Map = temp.Map;
        }

        public static VectorClock Merge(VectorClock one, VectorClock other)
        {
            var result = new VectorClock(one);
            result.Merge(other);
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

    public static class VectorClockExtensions
    {
        public static void Put(this VectorClock vectorClock, string key, int value)
        {
            Put(vectorClock.Map, key, value);    
        }

        public static void Put(this Dictionary<string, int> map, string key, int value)
        {
            if (!map.ContainsKey(key))
                map.Add(key, value);
            else
                map[key] = value;
        }
    }
}
