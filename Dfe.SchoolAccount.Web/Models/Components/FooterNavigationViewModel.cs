namespace Dfe.SchoolAccount.Web.Models.Components;

using Dfe.SchoolAccount.Web.Services.ContentHyperlinks;

public sealed class FooterNavigationViewModel
{
    public IList<ContentHyperlink> Links { get; set; } = new List<ContentHyperlink>();
}
