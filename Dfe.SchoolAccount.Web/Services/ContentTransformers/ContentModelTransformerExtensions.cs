namespace Dfe.SchoolAccount.Web.Services.ContentTransformers;

using Contentful.Core.Models;

/// <summary>
/// Extensions for the <see cref="IContentModelTransformer"/> interface.
/// </summary>
public static class ContentModelTransformerExtensions
{
    /// <summary>
    /// Transforms an enumerable collection of content to a specific type of model.
    /// </summary>
    /// <param name="transformer">The content model transformer service.</param>
    /// <param name="content">The content.</param>
    /// <returns>
    /// An enumerable collection of transformed vide models. Content entries are
    /// <c>null</c> when input content entry is <c>null</c>.
    /// </returns>
    /// <typeparam name="TTargetModel">The target type of model.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="transformer"/> or <paramref name="transformer"/> is <c>null</c>.
    /// </exception>
    public static TTargetModel[] TransformContentToModel<TTargetModel>(this IContentModelTransformer transformer, IEnumerable<IContent> content)
        where TTargetModel : class
    {
        if (transformer == null) {
            throw new ArgumentNullException(nameof(transformer));
        }
        if (content == null) {
            throw new ArgumentNullException(nameof(content));
        }

        return content.Select(content => transformer.TransformContentToModel<TTargetModel>(content))
            .ToArray();
    }
}
