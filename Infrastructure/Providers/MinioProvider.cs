using Application.Providers;
using Domain.Common;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel;
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

    public async Task<Result<string>> GetPhoto(string path, CancellationToken ct)
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

            var presignedGetObjectArgs = new PresignedGetObjectArgs()
                .WithBucket(PhotoBucket)
                .WithObject(path);
            return await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error with get photo in MinIO {message}", ex.Message);
            return Errors.General.GetFailure(path);
        }
    }

    public async Task<Result<List<string>>> GetPhotos(IEnumerable<string> paths, CancellationToken ct)
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

            List<string> urls = [];
            foreach (var path in paths)
            {
                var presignedGetObjectArgs = new PresignedGetObjectArgs()
                    .WithBucket(PhotoBucket)
                    .WithObject(path)
                    .WithExpiry(60 * 60 * 24);
                urls.Add(await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs));
            }

            return urls;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error with get photo in MinIO {message}", ex.Message);
            return Errors.General.GetFailure("photos");
        }
    }

    public IObservable<Item> GetObjectList(CancellationToken ct)
    {
        var bucketExistsArgs = new BucketExistsArgs().WithBucket(PhotoBucket);
        var isExist = _minioClient.BucketExistsAsync(bucketExistsArgs, ct).Result;
        if (isExist)
        {
            var makeBucketArgs = new MakeBucketArgs().WithBucket(PhotoBucket);
            _minioClient.MakeBucketAsync(makeBucketArgs, ct);
        }

        var listObjectArgs = new ListObjectsArgs().WithBucket(PhotoBucket);

        return _minioClient.ListObjectsAsync(listObjectArgs, ct);
    }

    public async Task<Result> RemovePhoto(string path, CancellationToken ct)
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
            var bucketExistsArgs = new BucketExistsArgs().WithBucket(PhotoBucket);
            var isExist = await _minioClient.BucketExistsAsync(bucketExistsArgs, ct);
            if (isExist)
            {
                var makeBucketArgs = new MakeBucketArgs().WithBucket(PhotoBucket);
                await _minioClient.MakeBucketAsync(makeBucketArgs, ct);
            }

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