namespace Dfe.SchoolAccount.Web.Models.Content;

using Contentful.Core.Models;

public sealed class CardGroupModel : IContent
{
    public IList<CardModel> Cards { get; set; } = new List<CardModel>();
}
