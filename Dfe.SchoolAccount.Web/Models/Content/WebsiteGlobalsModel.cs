namespace Dfe.SchoolAccount.Web.Models.Content;

using Dfe.SchoolAccount.Web.Services.ContentHyperlinks;

public sealed class WebsiteGlobalsModel : IWebsiteGlobalsModel
{
    public string SiteTitle { get; set; } = null!;

    public IReadOnlyList<IContentHyperlink> FooterLinks { get; set; } = new List<IContentHyperlink>();
}
