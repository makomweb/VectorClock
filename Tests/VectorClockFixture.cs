using System.Security;
using Xunit;

namespace VectorClock.Tests
{
    public class VectorClockFixture
    {
        [Fact]
        public void There_should_be_3_resulting_items()
        {
            var one = new VectorClock();
            var other = new VectorClock();

            AssertMergedResultCount(0, one, other);

            one.Tick("foo");
            one.Tick("bar");

            AssertMergedResultCount(2, one, other);

            other.Tick("foo");

            AssertMergedResultCount(2, one, other);

            other.Tick("baz");

            AssertMergedResultCount(3, one, other);

            one.Tick("baz");

            AssertMergedResultCount(3, one, other);
        }

        private static void AssertMergedResultCount(int expectedCount, VectorClock one, VectorClock other)
        {
            var res = VectorClock.Merge(one, other);
            Assert.Equal(expectedCount, res.Keys.Length);
        }
    }
}
