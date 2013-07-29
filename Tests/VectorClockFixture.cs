using Xunit;

namespace VectorClock.Tests
{
    public class VectorClockFixture
    {
        [Fact]
        public void There_should_be_3_resulting_items()
        {
            var one = new VectorClock();
            one.Tick("foo");
            one.Tick("bar");

            var other = new VectorClock();
            other.Tick("baz");
            other.Tick("baz");
            other.Tick("bar");
            other.Tick("foo");

            var result = VectorClock.Merge(one, other);

            Assert.Equal(3, result.Keys.Length);
        }
    }
}
