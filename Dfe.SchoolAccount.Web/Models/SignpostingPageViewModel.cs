namespace Dfe.SchoolAccount.Web.Models;

using Contentful.Core.Models;

public sealed class SignpostingPageViewModel
{
    public string Title { get; set; } = null!;

    public Document? Body { get; set; }
}
