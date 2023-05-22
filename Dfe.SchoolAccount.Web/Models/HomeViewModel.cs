namespace Dfe.SchoolAccount.Web.Models;

public sealed class HomeViewModel
{
    public string OrganisationName { get; set; } = null!;

    public IList<CardViewModel> UsefulServicesAndGuidanceCards { get; set; } = null!;

    public IList<CardViewModel> SupportCards { get; set; } = null!;
}
