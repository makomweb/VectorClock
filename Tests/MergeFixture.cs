using System;
using System.Linq;
using System.Reactive.Linq;
using Xunit;

namespace VectorClock.Tests
{
    public class MergeFixture
    {
        private readonly EventSource _sourceA = new EventSource(new[]
        {
            new Item("A", DateTime.Now.AddSeconds(1)),
            new Item("B", DateTime.Now.AddSeconds(3))
        });

        private readonly EventSource _sourceB = new EventSource(new[] 
        {
            new Item("C", DateTime.Now.AddSeconds(2)),
            new Item("D", DateTime.Now.AddSeconds(4))
        });

        [Fact]
        public void Test_concat()
        {
            var concat = _sourceA.SourceItems.Concat(_sourceB.SourceItems).ToEnumerable().ToArray();
            Assert.NotNull(concat);
            Assert.True(concat.Count() == 4);
        }

        [Fact]
        public void Test_merge()
        {
            var merged = _sourceA.SourceItems.Merge(_sourceB.SourceItems).ToEnumerable().ToArray();
            Assert.NotNull(merged);
            Assert.True(merged.ToArray().Count() == 4);
        }
    }
}