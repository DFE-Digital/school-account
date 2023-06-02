namespace Dfe.SchoolAccount.Web.Models.Components;

using Dfe.SchoolAccount.Web.Services.ContentHyperlinks;

public sealed class FooterNavigationViewModel
{
    public IReadOnlyList<IContentHyperlink> Links { get; set; } = new List<IContentHyperlink>();
}
