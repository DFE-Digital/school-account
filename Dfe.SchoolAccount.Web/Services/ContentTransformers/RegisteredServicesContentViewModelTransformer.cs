namespace Dfe.SchoolAccount.Web.Services.ContentTransformers;

using System.Reflection;
using Contentful.Core.Models;

/// <summary>
/// A service which resolves a <see cref="IContentViewModelTransformHandler{TContent, TViewModel}"/>
/// in order to transform content to a specific type of view model.
/// </summary>
public sealed class RegisteredServicesContentViewModelTransformer : IContentViewModelTransformer
{
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisteredServicesContentViewModelTransformer"/> class.
    /// </summary>
    /// <param name="serviceProvider">Service provider in order to resolve transform handlers.</param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="serviceProvider"/> is <c>null</c>.
    /// </exception>
    public RegisteredServicesContentViewModelTransformer(IServiceProvider serviceProvider)
    {
        if (serviceProvider == null) {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        this.serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public TViewModel TransformContentToViewModel<TViewModel>(IContent content)
        where TViewModel : class
    {
        var transformHandlerType = GetTransformHandlerType(content.GetType(), typeof(TViewModel));
        var transformHandler = this.serviceProvider.GetService(transformHandlerType);
        if (transformHandler == null) {
            throw new InvalidOperationException($"Cannot resolve transform handler for `{content.GetType()}` to `{typeof(TViewModel)}`.");
        }
        
        var transformMethodInfo = GetTransformMethod(transformHandlerType);
        return (TViewModel)transformMethodInfo.Invoke(transformHandler, new object[] { content })!;
    }

    private static Type GetTransformHandlerType(Type contentType, Type viewModelType)
    {
        var transformHandlerGenericType = typeof(IContentViewModelTransformHandler<,>);
        return transformHandlerGenericType.MakeGenericType(contentType, viewModelType);
    }

    private static MethodInfo GetTransformMethod(Type transformHandlerType)
    {
        string transformMethodName = nameof(IContentViewModelTransformHandler<IContent, object>.TransformContentToViewModel);
        return transformHandlerType?.GetMethod(transformMethodName, BindingFlags.Public | BindingFlags.Instance)!;
    }
}
