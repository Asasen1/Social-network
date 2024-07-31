using Application.Providers;
using Domain.Common;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;

namespace Infrastructure.Providers;

public class MinioProvider : IMinioProvider
{
    private const string PhotoBucket = "images"; 
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }
    
    public async Task<Result<string>> UploadPhoto(Stream stream, string path, CancellationToken ct)
    {
        try
        {
            var bucketExistsArgs = new BucketExistsArgs().WithBucket(PhotoBucket);
            var isExist = await _minioClient.BucketExistsAsync(bucketExistsArgs, ct);
            if (isExist)
            {
                var makeBucketArgs = new MakeBucketArgs().WithBucket(PhotoBucket);
                await _minioClient.MakeBucketAsync(makeBucketArgs, ct);
            }

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(PhotoBucket)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithObject(path);
            var response = await _minioClient.PutObjectAsync(putObjectArgs, ct);
            return response.ObjectName;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error with saving photo in MinIO {message}", ex.Message);
            return Errors.General.SaveFailure(path);
        }
    }

    public async Task<Result> RemovePhoto(string path, CancellationToken ct)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(PhotoBucket)
                .WithObject(path);
            await _minioClient.RemoveObjectAsync(removeObjectArgs, ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error with removing photo in MinIO {message}", ex.Message);
            return Errors.General.RemoveFailure(path);
        }
    }
    public async Task<Result> RemovePhotos(List<string> paths, CancellationToken ct)
    {
        try
        {
            var removeObjectsArgs = new RemoveObjectsArgs()
                .WithBucket(PhotoBucket)
                .WithObjects(paths);
            await _minioClient.RemoveObjectsAsync(removeObjectsArgs, ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error with removing photo in MinIO {message}", ex.Message);
            return Errors.General.RemoveFailure("photos");
        }
    }
}