using Google.Cloud.Storage.V1;

namespace EmbeddingCloudRun;

public class UploadDocService : IUploadDocService
{
    private readonly ILogger<UploadDocController> _logger;
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;

    public UploadDocService(ILogger<UploadDocController> logger, IConfiguration config, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _config = config;
        _httpClientFactory = httpClientFactory;
    }

    public async ValueTask<ApiResponse> UploadDocToBucket(ApiRequest<UploadDocRequest> request)
    {
        var bucketName = _config["GCP:Bucket:name"];

        if (request != null && request.body.file != null)
        {
            var fileName = request.body.file.FileName;
            var contentType = request.body.file.ContentType;

            var storageClient = await StorageClient.CreateAsync();

            await using var stream = request.body.file.OpenReadStream();

            var uploadObject = await storageClient.UploadObjectAsync(bucketName, fileName, contentType, stream);
            
            return new ApiResponse
            {
                resultCode = "0000",
                resultMessage = $"File '{fileName}' with content type '{contentType}' is ready to be uploaded to bucket '{bucketName}'"
            };
        } 
        else
        {
            return new ApiResponse
            {
                resultCode = "U400",
                resultMessage = "File is missing in the request."
            };
        }
    }
}
