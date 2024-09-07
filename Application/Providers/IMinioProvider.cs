using Domain.Common;
using Minio.DataModel;

namespace Application.Providers;

public interface IFileProvider
{
    public Task<Result<string>> UploadPhoto(Stream stream, string path, CancellationToken ct);
    public Task<Result<List<string>>> GetPhotos(IEnumerable<string> paths, CancellationToken ct);
    public Task<Result<string>> GetPhoto(string path, CancellationToken ct);
    public Task<Result> RemovePhoto(string path, CancellationToken ct);
    public Task<Result> RemovePhotos(List<string> paths, CancellationToken ct);
    public IObservable<Item> GetObjectList(CancellationToken ct);

}