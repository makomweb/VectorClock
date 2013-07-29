using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace VectorClock
{
    public class EventSource
    {
        public EventSource(IEnumerable<Item> items) : this(items, Scheduler.Default)
        {
        }

        public EventSource(IEnumerable<Item> items, IScheduler scheduler)
        {
            SourceItems = items.ToObservable(scheduler);
        }

        public IObservable<Item> SourceItems { get; private set; }
    }
}