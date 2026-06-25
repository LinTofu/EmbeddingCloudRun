using Microsoft.AspNetCore.Mvc;

namespace EmbeddingCloudRun;

/// <summary>
/// Upload documents to GCP Bucket
/// </summary>
[ApiController]

public class UploadDocController : ControllerBase
{
    private readonly ILogger<UploadDocController> _logger;
    private readonly IUploadDocService _uploadDocService;

    public UploadDocController(ILogger<UploadDocController> logger, IUploadDocService uploadDocService)
    {
        _logger = logger;
        _uploadDocService = uploadDocService;
    }

    /// <summary>
    /// POST: api/UploadDocToBucket
    /// Upload document to GCP Bucket
    /// </summary>
    [HttpPost]
    [Route("api/UploadDocToBucket")]
    public async Task<ActionResult<ApiResponse>> UploadDocToBucket([FromForm] ApiRequest<UploadDocRequest> request)
    {
        try
        {
            var response = await _uploadDocService.UploadDocToBucket(request);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}