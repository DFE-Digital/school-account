namespace Dfe.SchoolAccount.Web.Services.ContentTransformers;

using System.Reflection;
using Contentful.Core.Models;

/// <summary>
/// A service which resolves a <see cref="IContentModelTransformHandler{TContent, TTargetModel}"/>
/// in order to transform content to a different type of model.
/// </summary>
public sealed class RegisteredServicesContentModelTransformer : IContentModelTransformer
{
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisteredServicesContentModelTransformer"/> class.
    /// </summary>
    /// <param name="serviceProvider">Service provider in order to resolve transform handlers.</param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="serviceProvider"/> is <c>null</c>.
    /// </exception>
    public RegisteredServicesContentModelTransformer(IServiceProvider serviceProvider)
    {
        if (serviceProvider == null) {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        this.serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public TTargetModel TransformContentToModel<TTargetModel>(IContent content)
        where TTargetModel : class
    {
        var transformHandlerType = GetTransformHandlerType(content.GetType(), typeof(TTargetModel));
        var transformHandler = this.serviceProvider.GetService(transformHandlerType);
        if (transformHandler == null) {
            throw new InvalidOperationException($"Cannot resolve transform handler for `{content.GetType()}` to `{typeof(TTargetModel)}`.");
        }
        
        var transformMethodInfo = GetTransformMethod(transformHandlerType);
        return (TTargetModel)transformMethodInfo.Invoke(transformHandler, new object[] { content })!;
    }

    private static Type GetTransformHandlerType(Type contentType, Type targetModelType)
    {
        var transformHandlerGenericType = typeof(IContentModelTransformHandler<,>);
        return transformHandlerGenericType.MakeGenericType(contentType, targetModelType);
    }

    private static MethodInfo GetTransformMethod(Type transformHandlerType)
    {
        string transformMethodName = nameof(IContentModelTransformHandler<IContent, object>.TransformContentToModel);
        return transformHandlerType?.GetMethod(transformMethodName, BindingFlags.Public | BindingFlags.Instance)!;
    }
}
