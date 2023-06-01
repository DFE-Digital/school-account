namespace Dfe.SchoolAccount.Web.Models.Components;

using Dfe.SchoolAccount.Web.Services.ContentHyperlinks;

public sealed class FooterNavigationViewModel
{
    public IList<IContentHyperlink> Links { get; set; } = new List<IContentHyperlink>();
}
