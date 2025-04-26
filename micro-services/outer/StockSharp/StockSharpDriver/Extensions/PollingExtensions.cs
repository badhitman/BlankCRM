using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// PollingExtensions
/// </summary>
public static class PollingExtensions
{
    /// <inheritdoc/>
    public static T GetConfiguration<T>(this IServiceProvider serviceProvider)
        where T : class
    {
        var o = serviceProvider.GetService<IOptions<T>>();
        return o is null ? throw new ArgumentNullException(nameof(T)) : o.Value;
    }
}