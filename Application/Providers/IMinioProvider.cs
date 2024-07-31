using Domain.Common;

namespace Application.Providers;

public interface IMinioProvider
{
    public Task<Result<string>> UploadPhoto(Stream stream, string path, CancellationToken ct);
    public Task<Result> RemovePhoto(string path, CancellationToken ct);
    public Task<Result> RemovePhotos(List<string> paths, CancellationToken ct);
}