namespace Dfe.SchoolAccount.Web.Models.Content;

using Dfe.SchoolAccount.Web.Models.Content.Cards;

public sealed class HubContent
{
    public string Handle { get; set; } = null!;

    public IList<ExternalResourceCardContent> UsefulServicesAndGuidanceCards { get; set; } = new List<ExternalResourceCardContent>();

    public IList<ExternalResourceCardContent> SupportCards { get; set; } = new List<ExternalResourceCardContent>();
}
