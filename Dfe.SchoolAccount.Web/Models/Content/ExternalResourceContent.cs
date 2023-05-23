namespace Dfe.SchoolAccount.Web.Models.Content;

using Contentful.Core.Models;

public sealed class ExternalResourceContent : IContent
{
    public string Title { get; set; } = null!;

    public string Summary { get; set; } = null!;

    public string LinkUrl { get; set; } = null!;
}
