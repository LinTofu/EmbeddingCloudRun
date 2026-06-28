namespace EmbeddingCloudRun;

public interface IEmbeddingSearchService
{
    public ValueTask<EmbeddingSearchResponse> EmbeddingSearch(ApiRequest<EmbeddingSearchRequest> request);
}
