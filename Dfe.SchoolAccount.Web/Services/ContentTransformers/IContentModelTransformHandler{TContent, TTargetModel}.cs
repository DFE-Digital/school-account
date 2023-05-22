namespace Dfe.SchoolAccount.Web.Services.ContentTransformers;

using Contentful.Core.Models;

/// <summary>
/// A service which handles the transformation of a specific type of content to a
/// different type of model.
/// </summary>
/// <typeparam name="TContent">The type of content.</typeparam>
/// <typeparam name="TTargetModel">The target type of model.</typeparam>
public interface IContentModelTransformHandler<TContent, TTargetModel>
    where TContent : IContent
{
    /// <summary>
    /// Transforms content to a different type of model.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <returns>
    /// The transformed model.
    /// </returns>
    TTargetModel TransformContentToModel(TContent content);
}
