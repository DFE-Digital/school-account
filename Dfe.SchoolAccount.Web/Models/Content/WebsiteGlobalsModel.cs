namespace Dfe.SchoolAccount.Web.Models.Content;

using Dfe.SchoolAccount.Web.Services.ContentHyperlinks;

public sealed class WebsiteGlobalsModel
{
    public string SiteTitle { get; set; } = null!;

    public IList<IContentHyperlink> FooterLinks { get; set; } = new List<IContentHyperlink>();
}
