namespace Dfe.SchoolAccount.Web.Services.Content;

using System;
using Contentful.Core.Configuration;
using Dfe.SchoolAccount.Web.Constants;
using Dfe.SchoolAccount.Web.Models.Content;

public sealed class CustomContentTypeResolver : IContentTypeResolver
{
    private static readonly IReadOnlyDictionary<string, Type> s_ContentTypeIdToTypeMap = new Dictionary<string, Type> {
        { ContentTypeConstants.ExternalResource, typeof(ExternalResourceContent) },
        { ContentTypeConstants.SignpostingPage, typeof(SignpostingPageContent) },
        { ContentTypeConstants.Page, typeof(PageContent) },
    };

    /// <inheritdoc/>
    public Type? Resolve(string contentTypeId)
    {
        return s_ContentTypeIdToTypeMap.GetValueOrDefault(contentTypeId);
    }
}
