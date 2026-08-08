namespace EmbeddingCloudRun;

public interface IEmbeddingSearchService
{
    public ValueTask<ApiResponse> EmbeddingSearch(ApiRequest<EmbeddingSearchRequest> request);
}
