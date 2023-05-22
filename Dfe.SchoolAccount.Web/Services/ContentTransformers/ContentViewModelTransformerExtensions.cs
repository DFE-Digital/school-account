namespace Dfe.SchoolAccount.Web.Services.ContentTransformers;

using Contentful.Core.Models;

/// <summary>
/// Extensions for the <see cref="IContentViewModelTransformer"/> interface.
/// </summary>
public static class ContentViewModelTransformerExtensions
{
    /// <summary>
    /// Transforms an enumerable collection of content to a specific type of view model.
    /// </summary>
    /// <param name="transformer">The content view model transformer service.</param>
    /// <param name="content">The content.</param>
    /// <returns>
    /// An enumerable collection of transformed vide models. Content entries are
    /// <c>null</c> when input content entry is <c>null</c>.
    /// </returns>
    /// <typeparam name="TViewModel">The type of view model.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="transformer"/> or <paramref name="transformer"/> is <c>null</c>.
    /// </exception>
    public static TViewModel[] TransformContentToViewModel<TViewModel>(this IContentViewModelTransformer transformer, IEnumerable<IContent> content)
        where TViewModel : class
    {
        if (transformer == null) {
            throw new ArgumentNullException(nameof(transformer));
        }
        if (content == null) {
            throw new ArgumentNullException(nameof(content));
        }

        return content.Select(content => transformer.TransformContentToViewModel<TViewModel>(content))
            .ToArray();
    }
}
