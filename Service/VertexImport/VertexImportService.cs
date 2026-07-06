using Google.Cloud.DiscoveryEngine.V1;

namespace EmbeddingCloudRun;

public class VertexImportService : IVertexImportService
{
    private readonly ILogger<VertexImportService> _logger;
    private readonly IConfiguration _configuration;

    public VertexImportService(ILogger<VertexImportService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task VertexBucketImport(ApiRequest<VertexImportRequest> request)
    {
        try
        {
            var project = _configuration["VertexAISearch:Project"];
            var location = _configuration["VertexAISearch:Location"];
            var datastore = _configuration["VertexAISearch:DataStoreId"];
            var endpoint = $"{location}-discoveryengine.googleapis.com";

            var bucket = _configuration["GCP:Bucket:name"];

            var vertexClient = await new DocumentServiceClientBuilder
            {
                Endpoint = endpoint,
            }.BuildAsync();

            var parent = BranchName.FromProjectLocationDataStoreBranch(project, location, datastore, "default_branch").ToString();

            var vertexRequest = new ImportDocumentsRequest
            {
                Parent = parent,
                GcsSource = new GcsSource
                {
                    // InputUris = { $"gs://{bucket}/{request.body.FileName}" },
                    // DataSchema = "content",
                    InputUris = { $"gs://{bucket}/*" },
                    DataSchema = "content"
                },
                ReconciliationMode = ImportDocumentsRequest.Types.ReconciliationMode.Full
            };

            var operation = await vertexClient.ImportDocumentsAsync(vertexRequest);

            _logger.LogInformation($"Import operation started: {operation.Name}");

            // await operation.PollUntilCompletedAsync();

            _logger.LogInformation($"Import operation completed: {operation.Name}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error occurred while performing vertex import: {ex.Message}", ex);
        }
    }
}
