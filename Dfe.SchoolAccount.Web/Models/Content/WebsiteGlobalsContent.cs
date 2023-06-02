namespace Dfe.SchoolAccount.Web.Models.Content;

using Contentful.Core.Models;

public sealed class WebsiteGlobalsContent : IContent
{
    public string SiteTitle { get; set; } = null!;

    public IList<IContent> FooterLinks { get; set; } = new List<IContent>();
}
