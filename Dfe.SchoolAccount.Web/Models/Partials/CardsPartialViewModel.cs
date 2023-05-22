namespace Dfe.SchoolAccount.Web.Models.Partials;

using Dfe.SchoolAccount.Web.Models.Content;

/// <summary>
/// View model for the "Partials/_Cards" partial view.
/// </summary>
public sealed class CardsPartialViewModel
{
    /// <summary>
    /// Gets or sets the heading level used for each card.
    /// </summary>
    /// <remarks>
    /// <para>Defaults to a value of 2 to produce <c>&lt;h2&gt;</c> elements.</para>
    /// </remarks>
    public int HeadingLevel { get; set; } = 2;

    /// <summary>
    /// Gets or sets the number of columns which cards should be arranged within.
    /// </summary>
    /// <remarks>
    /// <para>Defaults to a value of 3.</para>
    /// </remarks>
    public int ColumnCount { get; set; } = 3;

    /// <summary>
    /// Gets or sets the collection of cards that will be presented in the partial view.
    /// </summary>
    public IEnumerable<CardModel> Cards { get; set; } = Array.Empty<CardModel>();
}
