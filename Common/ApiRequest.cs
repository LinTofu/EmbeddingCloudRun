namespace EmbeddingCloudRun;

public class ApiRequest<T>
{
    public ApiHeader? header { get; set; }
    public T? body { get; set; }

    public class ApiHeader {
        public string? senderCode { get; set; }
    }
}
