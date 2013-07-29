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
           
            AssertMergedKeyCount(0, one, other);

            one.Tick("foo");
            one.Tick("bar");

            AssertMergedKeyCount(2, one, other);

            other.Tick("foo");

            AssertMergedKeyCount(2, one, other);

            other.Tick("baz");

            AssertMergedKeyCount(3, one, other);

            one.Tick("baz");

            AssertMergedKeyCount(3, one, other);
        }

        [Fact]
        public void After_merging_there_should_be_3_results()
        {
            var one = new VectorClock();

            one.Tick("foo");
            one.Tick("bar");

            var other = VectorClock.Merge(new VectorClock(), one);

            other.Tick("foo");
            other.Tick("bar");
            other.Tick("baz");

            Assert.Equal(3, VectorClock.Merge(one, other).Keys.Length);

            one.Merge(other);

            one.Tick("foo");
            one.Tick("foobar");

            AssertMergedKeyCount(4, one, other);
        }

        private static void AssertMergedKeyCount(int expectedCount, VectorClock one, VectorClock other)
        {
            var merged = VectorClock.Merge(one, other);
            Assert.Equal(expectedCount, merged.Keys.Length);
        }

        private static void AssertTickCount(int expectedTickCount, VectorClock clock)
        {
        }
    }
}
