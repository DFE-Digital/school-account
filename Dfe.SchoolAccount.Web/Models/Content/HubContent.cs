namespace Dfe.SchoolAccount.Web.Models.Content;

public sealed class HubContent
{
    public string Handle { get; set; } = null!;

    public IList<CardContent> UsefulServicesAndGuidanceCards { get; set; } = new List<CardContent>();

    public IList<CardContent> SupportCards { get; set; } = new List<CardContent>();
}
