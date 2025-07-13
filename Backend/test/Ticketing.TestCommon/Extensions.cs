using FluentAssertions;

namespace Ticketing.TestCommon.Extensions
{
  public static class TestExtensions
  {
    /// <summary>
    /// Asserts that two byte arrays are equal.
    /// </summary>
    public static void ShouldBeEquivalentTo(this byte[] actual, byte[] expected)
    {
      actual.Should().NotBeNull();
      expected.Should().NotBeNull();
      actual.Should().HaveSameCount(expected);
      actual.Should().Equal(expected);
    }

    /// <summary>
    /// Asserts that two collections contain the same elements, ignoring order.
    /// </summary>
    public static void ShouldContainSameElementsAs<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
    {
      actual.Should().BeEquivalentTo(expected);
    }

    /// <summary>
    /// Checks that a collection does not contain duplicates.
    /// </summary>
    public static void ShouldNotContainDuplicates<T>(this IEnumerable<T> collection)
    {
      var duplicates = collection.GroupBy(x => x).Where(g => g.Count() > 1).ToList();
      duplicates.Should().BeEmpty("Collection contains duplicate elements.");
    }

    /// <summary>
    /// Asserts that a DateTime is recent (within N seconds of now).
    /// </summary>
    public static void ShouldBeRecent(this DateTime actual, int withinSeconds = 5)
    {
      var now = DateTime.UtcNow;
      actual.Should().BeAfter(now.AddSeconds(-withinSeconds));
      actual.Should().BeBefore(now.AddSeconds(withinSeconds));
    }
  }
}
