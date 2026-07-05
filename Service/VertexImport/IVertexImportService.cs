namespace EmbeddingCloudRun;

public interface IVertexImportService
{
    public Task VertexBucketImport(ApiRequest<VertexImportRequest> request);
}
