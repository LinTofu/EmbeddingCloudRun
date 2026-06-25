namespace EmbeddingCloudRun;

public interface IUploadDocService
{
    public ValueTask<ApiResponse> UploadDocToBucket(ApiRequest<UploadDocRequest> request);
}
