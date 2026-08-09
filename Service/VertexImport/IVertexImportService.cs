namespace EmbeddingCloudRun;

public interface IVertexImportService
{
    public Task VertexIndexCreate(ApiRequest<VertexImportRequest> request);

    public Task VertexIndexDelete(ApiRequest<VertexImportRequest> request);
}
