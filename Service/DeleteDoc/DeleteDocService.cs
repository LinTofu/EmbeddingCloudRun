using System.Net.Http.Headers;
using Google.Apis.Auth.OAuth2;

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

    [Obsolete]
    public async ValueTask<ApiResponse> DeleteDocFromBucket(ApiRequest<DeleteDocRequest> request)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            var bucketName = _config["GCP:Bucket:name"];
            var serviceAccountKeyFile = _config["GCP:Bucket:serviceAccountKeyFile"];
            var scope = _config["GCP:Bucket:scope"];

            // var credential = GoogleCredential.FromFile(serviceAccountKeyFile).CreateScoped(scope);
            var credential = await GoogleCredential.GetApplicationDefaultAsync();
            var token = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

            var deleteBaseUrl = _config["GCP:Bucket:url:delete"].Replace("{bucket-name}", bucketName).Replace("{file-name}", request.body.fileName);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (request != null)
            {
                var response = await httpClient.DeleteAsync(deleteBaseUrl);
                response.EnsureSuccessStatusCode();
                
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
                    resultCode = "4010",
                    resultMessage = "File is missing in the request."
                };
            }
        } 
        catch (Exception ex)
        {
            // Log the exception
            _logger.LogError(ex, "Error occurred while deleting document from bucket.");
            
            throw;
        }
    }
}