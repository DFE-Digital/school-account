namespace Dfe.SchoolAccount.Web.Models;

using Dfe.SchoolAccount.Web.Models.Content.Cards;

public sealed class HomeViewModel
{
    public string OrganisationName { get; set; } = null!;

    public IList<ExternalResourceCardContent> UsefulServicesAndGuidanceCards { get; set; } = null!;

    public IList<ExternalResourceCardContent> SupportCards { get; set; } = null!;
}
