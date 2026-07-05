using Microsoft.AspNetCore.Mvc;

namespace EmbeddingCloudRun;

/// <summary>
/// Controller for handling Vertex AI import operations
/// </summary>
[ApiController]
public class VertexImportController : ControllerBase
{
    private readonly ILogger<VertexImportController> _logger;
    private readonly IVertexImportService _vertexImportService;

    public VertexImportController(ILogger<VertexImportController> logger, IVertexImportService vertexImportService)
    {
        _logger = logger;
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
        try
        {
            await _vertexImportService.VertexBucketImport(request);
            
            return Ok(new ApiResponse
            {
                resultCode = "0000",
                resultMessage = "Vertex import operation completed successfully."
            });
        } 
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while performing vertex import.");
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
