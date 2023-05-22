namespace Dfe.SchoolAccount.Web.Services.ContentTransformers;

using Contentful.Core.Models;

/// <summary>
/// A service which handles the transformation of a specific type of content to a
/// specific type of view model.
/// </summary>
/// <typeparam name="TContent">The type of content.</typeparam>
/// <typeparam name="TViewModel">The type of view model.</typeparam>
public interface IContentViewModelTransformHandler<TContent, TViewModel>
    where TContent : IContent
{
    /// <summary>
    /// Transforms content to a view model.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <returns>
    /// The transformed view model.
    /// </returns>
    TViewModel TransformContentToViewModel(TContent content);
}
