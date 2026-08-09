using System.Security.Cryptography;
using System.Text;
using Google.Cloud.DiscoveryEngine.V1;
using Google.Protobuf.WellKnownTypes;

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

    public async Task VertexIndexCreate(ApiRequest<VertexImportRequest> request)
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

        var parent = BranchName.FromProjectLocationDataStoreBranch(project, location, datastore, "default_branch");

        // var vertexRequest = new ImportDocumentsRequest
        // {
        //     Parent = parent,
        //     GcsSource = new GcsSource
        //     {
        //         InputUris = { $"gs://{bucket}/{request.body.FileName}" },
        //         DataSchema = "content",
        //     },
        //     ReconciliationMode = ImportDocumentsRequest.Types.ReconciliationMode.Full
        // };

        // await vertexClient.ImportDocumentsAsync(vertexRequest);

        var documentId = GenerateDocumentId(request.body.FileName);

        var document = new Document
        {
            Id = documentId,
            StructData = new Struct
            {
                Fields =
                {
                    ["title"] = Value.ForString(request.body.FileName),
                    ["fileName"] = Value.ForString(request.body.FileName),
                    ["gcsUri"] = Value.ForString($"gs://{bucket}/{request.body.FileName}")
                }
            },
            Content = new Document.Types.Content
            {
                Uri = $"gs://{bucket}/{request.body.FileName}"
            }
        };

        var createRequest = new CreateDocumentRequest
        {
            ParentAsBranchName = parent,
            Document = document,
            DocumentId = documentId
        };

        await vertexClient.CreateDocumentAsync(createRequest);
    }

    public async Task VertexIndexDelete(ApiRequest<VertexImportRequest> request)
    {
        var project = _configuration["VertexAISearch:Project"];
        var location = _configuration["VertexAISearch:Location"];
        var datastore = _configuration["VertexAISearch:DataStoreId"];
        var endpoint = $"{location}-discoveryengine.googleapis.com";

        var vertexClient = await new DocumentServiceClientBuilder
        {
            Endpoint = endpoint,
        }.BuildAsync();

        var documentId = GenerateDocumentId(request.body.FileName);

        var documentName = DocumentName.FromProjectLocationDataStoreBranchDocument(project, location, datastore, "default_branch", documentId);

        await vertexClient.DeleteDocumentAsync(documentName);
    }

    private string GenerateDocumentId(string fileName)
    {
        var hash256 = SHA256.HashData(Encoding.UTF8.GetBytes(fileName));

        return Convert.ToHexString(hash256).ToLowerInvariant();
    }
}
