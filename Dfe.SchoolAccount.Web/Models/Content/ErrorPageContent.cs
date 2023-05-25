namespace Dfe.SchoolAccount.Web.Models.Content;

using Contentful.Core.Models;

public sealed class ErrorPageContent : IContent
{
    public string Handle { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Summary { get; set; } = null!;

    public Document? Body { get; set; }
}
