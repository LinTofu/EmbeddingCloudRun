using Google.Cloud.DiscoveryEngine.V1;
using Google.Protobuf.WellKnownTypes;

namespace EmbeddingCloudRun;

public class EmbeddingSearchService : IEmbeddingSearchService
{
    private readonly ILogger<EmbeddingSearchController> _logger;
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;

    public EmbeddingSearchService(ILogger<EmbeddingSearchController> logger, IConfiguration config, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _config = config;
        _httpClientFactory = httpClientFactory;
    }

    public async ValueTask<ApiResponse> EmbeddingSearch(ApiRequest<EmbeddingSearchRequest> request)
    {
        var gcpClient = await new SearchServiceClientBuilder {
            Endpoint = "us-discoveryengine.googleapis.com"
        }.BuildAsync();

        var project = _config["VertexAISearch:Project"];
        var location = _config["VertexAISearch:Location"];
        var engines = _config["VertexAISearch:Engines"];

        var servingConfig = ServingConfigName.FromProjectLocationCollectionEngineServingConfig(project, location, "default_collection", engines, "default_search");

        var searchRequest = new SearchRequest
        {
            ServingConfigAsServingConfigName = servingConfig,
            Query = request.body.Query,
            PageSize = request.body.ResultSize ?? 15
        };

        var searchResponse = gcpClient.Search(searchRequest);

        var res = new EmbeddingSearchResponse();

        foreach (var item in searchResponse)
        {
            res.ResultAnsList.Add(item.Document?.DerivedStructData?.ToString() ?? string.Empty);
        }

        res.resultCode = "0000";
        res.resultMessage = "Import Index Success";

        return res;
    }
}
