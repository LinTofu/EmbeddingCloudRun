using Microsoft.AspNetCore.Mvc;

namespace EmbeddingCloudRun;

/// <summary>
/// Controller for handling Vertex AI import operations
/// </summary>
[ApiController]
public class VertexImportController : BaseController
{
    private readonly IVertexImportService _vertexImportService;

    public VertexImportController(ILogger<VertexImportController> logger, IVertexImportService vertexImportService)
        : base(logger)
    {
        _vertexImportService = vertexImportService;
    }

    /// <summary>
    /// POST: api/VertexImport
    /// Import data into GCP Vertex AI
    /// </summary>
    [HttpPost]
    [Route("api/VertexImport")]
    public async Task<ActionResult<VertexImportResponse>> VertexImport([FromBody] ApiRequest<VertexImportRequest> request)
    {
        await _vertexImportService.VertexBucketImport(request);
        
        return Ok(new ApiResponse
        {
            resultCode = "0000",
            resultMessage = "Vertex import operation completed successfully."
        });
    }
}
