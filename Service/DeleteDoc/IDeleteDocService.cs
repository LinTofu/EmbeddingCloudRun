namespace EmbeddingCloudRun;

public interface IDeleteDocService
{
    public ValueTask<ApiResponse> DeleteDocFromBucket(ApiRequest<DeleteDocRequest> request);
}
