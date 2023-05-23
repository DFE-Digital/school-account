namespace Dfe.SchoolAccount.Web.Services.ContentTransformers;

using Contentful.Core.Models;

/// <summary>
/// A service which transforms content to a specific type of model.
/// </summary>
public interface IContentModelTransformer
{
    /// <summary>
    /// Transforms content to a different type of model.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <returns>
    /// The transformed model. A value of <c>null</c> if <paramref name="content"/>
    /// is <c>null</c>.
    /// </returns>
    /// <typeparam name="TTargetModel">The type of model.</typeparam>
    TTargetModel TransformContentToModel<TTargetModel>(IContent content)
        where TTargetModel : class;
}
