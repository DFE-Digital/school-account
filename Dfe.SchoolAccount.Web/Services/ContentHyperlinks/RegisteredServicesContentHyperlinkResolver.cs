namespace Dfe.SchoolAccount.Web.Services.ContentHyperlinks;

using System.Reflection;
using Contentful.Core.Models;

/// <summary>
/// A service which resolves a <see cref="IContentHyperlinkResolutionHandler{TContent}"/>
/// in order to transform content to a different type of model.
/// </summary>
public sealed class RegisteredServicesContentHyperlinkResolver : IContentHyperlinkResolver
{
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisteredServicesContentHyperlinkResolver"/> class.
    /// </summary>
    /// <param name="serviceProvider">Service provider in order to resolve hyperlink resolution handlers.</param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="serviceProvider"/> is <c>null</c>.
    /// </exception>
    public RegisteredServicesContentHyperlinkResolver(IServiceProvider serviceProvider)
    {
        if (serviceProvider == null) {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        this.serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public ContentHyperlink? ResolveContentHyperlink(object content)
    {
        if (content == null) {
            throw new ArgumentNullException(nameof(content));
        }

        var hyperlinkResolutionHandlerType = GetHyperlinkResolutionHandlerType(content.GetType());
        var hyperlinkResolutionHandler = this.serviceProvider.GetService(hyperlinkResolutionHandlerType);
        if (hyperlinkResolutionHandler == null) {
            return null;
        }

        var methodInfo = GetResolveContentHyperlinkMethod(hyperlinkResolutionHandlerType);
        return (ContentHyperlink?)methodInfo.Invoke(hyperlinkResolutionHandler, new object[] { content })!;
    }

    private static Type GetHyperlinkResolutionHandlerType(Type contentType)
    {
        var handlerGenericType = typeof(IContentHyperlinkResolutionHandler<>);
        return handlerGenericType.MakeGenericType(contentType);
    }

    private static MethodInfo GetResolveContentHyperlinkMethod(Type hyperlinkResolutionHandlerType)
    {
        string methodName = nameof(IContentHyperlinkResolutionHandler<IContent>.ResolveContentHyperlink);
        return hyperlinkResolutionHandlerType?.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance)!;
    }
}
