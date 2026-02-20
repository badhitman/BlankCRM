////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.Threading.Tasks;
using System.Threading;

namespace SharedLib;

/// <inheritdoc/>
public interface IEventsNotify
{
    /// <summary>
    /// Toast - notify for client`s
    /// </summary>
    public Task<ResponseBaseModel> ToastClientShow(ToastShowClientModel req, CancellationToken cancellationToken = default);
}
