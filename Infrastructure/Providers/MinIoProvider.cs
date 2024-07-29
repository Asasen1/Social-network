using Application.Providers;
using Domain.Common;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;

namespace Infrastructure.Providers;

public class MinIoProvider : IMinIoProvider
{
    private const string PhotoBucket = "images"; 
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinIoProvider> _logger;

    public MinIoProvider(IMinioClient minioClient, ILogger<MinIoProvider> logger)
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
            return Errors.General.SaveFailure("photo");
        }
    }
}