using System;

namespace VectorClock
{
    public class Item
    {
        public Item(string name)
            : this(name, DateTime.Now)
        {
        }

        public Item(string name, DateTime dateTime)
        {
            Name = name;
            Created = dateTime;
        }

        public string Name { get; private set; }

        public DateTime Created { get; private set; }
    }
}