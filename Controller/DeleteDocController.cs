using Microsoft.AspNetCore.Mvc;

namespace EmbeddingCloudRun;

/// <summary>
/// Upload documents to GCP Bucket
/// </summary>
[ApiController]

public class DeleteDocController : ControllerBase
{
    private readonly ILogger<DeleteDocController> _logger;
    private readonly IDeleteDocService _deleteDocService;

    public DeleteDocController(ILogger<DeleteDocController> logger, IDeleteDocService deleteDocService)
    {
        _logger = logger;
        _deleteDocService = deleteDocService;
    }

    /// <summary>
    /// POST: api/DeleteDocFromBucket
    /// Delete document from GCP Bucket
    /// </summary>
    [HttpPost]
    [Route("api/DeleteDocFromBucket")]
    public async Task<ActionResult<ApiResponse>> DeleteDocFromBucket([FromBody] ApiRequest<DeleteDocRequest> request)
    {
        try
        {
            var response = await _deleteDocService.DeleteDocFromBucket(request);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}