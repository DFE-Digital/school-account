namespace Dfe.SchoolAccount.Web.Tests.Helpers;

using Dfe.SchoolAccount.Web.Helpers;

[TestClass]
public sealed class ViewHelpersTests
{
    #region IReadOnlyList<IReadOnlyList<T>> ArrangeIntoGroupsOf<T>(this IEnumerable<T>, int)

    [TestMethod]
    public void ArrangeIntoGroupsOf_T__ThrowsArgumentNullException__WhenItemsArgumentIsNull()
    {
        var act = () => {
            _ = ViewHelpers.ArrangeIntoGroupsOf<int>(null!, 2);
        };

        Assert.ThrowsException<ArgumentNullException>(act);
    }

    [DataRow(-1)]
    [DataRow(0)]
    [DataTestMethod]
    public void ArrangeIntoGroupsOf_T__ThrowsArgumentOutOfRangeException__WhenItemsPerGroupArgumentIsZeroOrLess(int itemsPerGroup)
    {
        var items = new List<int> { 1, 2, 3 };

        var act = () => {
            _ = ViewHelpers.ArrangeIntoGroupsOf(items, itemsPerGroup);
        };

        Assert.ThrowsException<ArgumentOutOfRangeException>(act);
    }

    [TestMethod]
    public void ArrangeIntoGroupsOf_T__ReturnsItemsAsOneGroup__WhenItemCountIsLessThanItemsPerGroup()
    {
        var items = new List<int> { 1, 2, 3 };

        var groups = ViewHelpers.ArrangeIntoGroupsOf(items, 4);

        Assert.AreEqual(1, groups.Count);
        CollectionAssert.AreEquivalent(new int[] { 1, 2, 3 }, groups[0].ToArray());
    }

    [TestMethod]
    public void ArrangeIntoGroupsOf_T__ReturnsItemsAsOneGroup__WhenItemCountIsSameAsItemsPerGroup()
    {
        var items = new List<int> { 1, 2, 3, 4 };

        var groups = ViewHelpers.ArrangeIntoGroupsOf(items, 4);

        Assert.AreEqual(1, groups.Count);
        CollectionAssert.AreEquivalent(new int[] { 1, 2, 3, 4 }, groups[0].ToArray());
    }

    [TestMethod]
    public void ArrangeIntoGroupsOf_T__ReturnsItemsAcrossTwoGroups__WhenItemCountExceedsItemsPerGroup()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };

        var groups = ViewHelpers.ArrangeIntoGroupsOf(items, 4);

        Assert.AreEqual(2, groups.Count);
        CollectionAssert.AreEquivalent(new int[] { 1, 2, 3, 4 }, groups[0].ToArray());
        CollectionAssert.AreEquivalent(new int[] { 5 }, groups[1].ToArray());
    }

    #endregion

    #region string GetUniqueIdentifier()

    [TestMethod]
    public void GetUniqueIdentifier__ReturnsDifferentValueEachTimeCalled()
    {
        string firstUniqueIdentifier = ViewHelpers.GetUniqueIdentifier();
        string secondUniqueIdentifier = ViewHelpers.GetUniqueIdentifier();

        Assert.AreNotEqual(secondUniqueIdentifier, firstUniqueIdentifier);
    }

    #endregion

    #region string ConvertTitleToSlug(string)

    [TestMethod]
    public void ConvertTitleToSlug__ThrowsArgumentNullException__WhenTitleArgumentIsNull()
    {
        var act = () => {
            _ = ViewHelpers.ConvertTitleToSlug(null!);
        };

        Assert.ThrowsException<ArgumentNullException>(act);
    }

    [DataRow("an example title", "an-example-title")]
    [DataRow("An example title", "an-example-title")]
    [DataRow("    An example title   with    excess    spacing   ", "an-example-title-with-excess-spacing")]
    [DataRow("An example title.", "an-example-title")]
    [DataRow("An example title with the number 42", "an-example-title-with-the-number-42")]
    [DataRow("an-example-title", "an-example-title")]
    [DataRow("@+_ABC", "abc")]
    [DataRow("A", "a")]
    [DataRow("An example WITH_UNDERSCORES", "an-example-with-underscores")]
    [DataTestMethod]
    public void ConvertTitleToSlug__ReturnsTitleFormattedAsSlug(string title, string expectedSlugResult)
    {
        var slugResult = ViewHelpers.ConvertTitleToSlug(title);

        Assert.AreEqual(expectedSlugResult, slugResult);
    }

    #endregion
}
