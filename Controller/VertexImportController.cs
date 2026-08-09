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
    /// POST: api/VertexIndexCreate
    /// Create doc index into GCP Vertex AI
    /// </summary>
    [HttpPost]
    [Route("api/VertexIndexCreate")]
    public async Task<ActionResult<VertexImportResponse>> VertexIndexCreate([FromBody] ApiRequest<VertexImportRequest> request)
    {
        await _vertexImportService.VertexIndexCreate(request);
        
        return Ok(new ApiResponse
        {
            resultCode = "0000",
            resultMessage = "Vertex import operation completed successfully."
        });
    }

    /// <summary>
    /// POST: api/VertexIndexDelete
    /// Delete doc index from GCP Vertex AI
    /// </summary>
    [HttpPost]
    [Route("api/VertexIndexDelete")]
    public async Task<ActionResult<VertexImportResponse>> VertexIndexDelete([FromBody] ApiRequest<VertexImportRequest> request)
    {
        await _vertexImportService.VertexIndexDelete(request);
        
        return Ok(new ApiResponse
        {
            resultCode = "0000",
            resultMessage = "Vertex import operation completed successfully."
        });
    }
}
