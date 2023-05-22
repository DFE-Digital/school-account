namespace Dfe.SchoolAccount.Web.Models;

using Dfe.SchoolAccount.Web.Models.Content;

public sealed class HomeViewModel
{
    public string OrganisationName { get; set; } = null!;

    public IList<CardModel> UsefulServicesAndGuidanceCards { get; set; } = null!;

    public IList<CardModel> SupportCards { get; set; } = null!;
}
