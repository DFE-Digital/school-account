namespace Dfe.SchoolAccount.Web.Models.Content;

using Contentful.Core.Models;

public sealed class HubContent : IContent
{
    public string Handle { get; set; } = null!;

    public IList<IContent> UsefulServicesAndGuidanceCards { get; set; } = new List<IContent>();

    public IList<IContent> SupportCards { get; set; } = new List<IContent>();
}
