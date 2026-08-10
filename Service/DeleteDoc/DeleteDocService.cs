using Google.Cloud.Storage.V1;

namespace EmbeddingCloudRun;

public class DeleteDocService : IDeleteDocService
{
    private readonly ILogger<DeleteDocController> _logger;
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;

    public DeleteDocService(ILogger<DeleteDocController> logger, IConfiguration config, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _config = config;
        _httpClientFactory = httpClientFactory;
    }

    public async ValueTask<ApiResponse> DeleteDocFromBucket(ApiRequest<DeleteDocRequest> request)
    {
        var bucketName = _config["GCP:Bucket:name"];

        if (request != null || request.body != null)
        {
            var storageClient = await StorageClient.CreateAsync();
            await storageClient.DeleteObjectAsync(bucketName, request.body.fileName);
            
            return new ApiResponse
            {
                resultCode = "0000",
                resultMessage = $"File '{request.body.fileName}' has been successfully deleted from bucket '{bucketName}'."
            };
        } 
        else
        {
            return new ApiResponse
            {
                resultCode = "U401",
                resultMessage = "File is missing in the request."
            };
        }
    }
}   