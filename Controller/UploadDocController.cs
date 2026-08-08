using Microsoft.AspNetCore.Mvc;

namespace EmbeddingCloudRun;

/// <summary>
/// Upload documents to GCP Bucket
/// </summary>
[ApiController]

public class UploadDocController : BaseController
{
    private readonly IUploadDocService _uploadDocService;

    public UploadDocController(ILogger<UploadDocController> logger, IUploadDocService uploadDocService) 
        : base(logger)
    {
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
        var response = await _uploadDocService.UploadDocToBucket(request);

        return Ok(response);
    }
}