namespace Dfe.SchoolAccount.Web.Services.ContentTransformers;

using Contentful.Core.Models;

/// <summary>
/// A service which transforms content to a specific type of view model.
/// </summary>
public interface IContentViewModelTransformer
{
    /// <summary>
    /// Transforms content to a view model.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <returns>
    /// The transformed view model. A value of <c>null</c> if <paramref name="content"/>
    /// is <c>null</c>.
    /// </returns>
    /// <typeparam name="TViewModel">The type of view model.</typeparam>
    TViewModel TransformContentToViewModel<TViewModel>(IContent content)
        where TViewModel : class;
}
