namespace Dfe.SchoolAccount.Web.Models.Content;

using System.Collections.Generic;
using Dfe.SchoolAccount.Web.Services.ContentHyperlinks;

public interface IWebsiteGlobalsModel
{
    IReadOnlyList<IContentHyperlink> FooterLinks { get; }

    string SiteTitle { get; }
}
