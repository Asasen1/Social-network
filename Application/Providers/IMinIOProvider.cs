using Domain.Common;

namespace Application.Providers;

public interface IMinIoProvider
{
    public Task<Result<string>> UploadPhoto(Stream stream, string path, CancellationToken ct);
}