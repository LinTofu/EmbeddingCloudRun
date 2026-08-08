using System.Net.Http.Headers;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.WebUtilities;
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

    [Obsolete]
    public async ValueTask<ApiResponse> UploadDocToBucket(ApiRequest<UploadDocRequest> request)
    {
        // var httpClient = _httpClientFactory.CreateClient();
        var bucketName = _config["GCP:Bucket:name"];
        // var serviceAccountKeyFile = _config["GCP:Bucket:serviceAccountKeyFile"];
        // var scope = _config["GCP:Bucket:scope"];

        // // var credential = GoogleCredential.FromFile(serviceAccountKeyFile).CreateScoped(scope);
        // var credential = await GoogleCredential.GetApplicationDefaultAsync();
        // var token = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

        // var insertBaseUrl = _config["GCP:Bucket:url:insert"].Replace("{bucket-name}", bucketName);

        // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        if (request != null && request.body.file != null)
        {
            var fileName = request.body.file.FileName;
            var contentType = request.body.file.ContentType;

            var storageClient = await StorageClient.CreateAsync();

            // var queryString = new Dictionary<string, string?>
            // {
            //     ["uploadType"] = "media",
            //     ["name"] = fileName,
            // };

            // var insertUrl = QueryHelpers.AddQueryString(insertBaseUrl, queryString);

            await using var stream = request.body.file.OpenReadStream();

            var uploadObject = await storageClient.UploadObjectAsync(bucketName, fileName, contentType, stream);
            // using var content = new StreamContent(stream);
            // content.Headers.ContentType = new MediaTypeHeaderValue(request.body.file.ContentType);

            // var response = await httpClient.PostAsync(insertUrl, content);;
            // response.EnsureSuccessStatusCode();

            // 
            
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
