using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;

namespace EmbeddingCloudRun;

/// <summary>
/// Delete documents from GCP Bucket
/// </summary>
[ApiController]

public class DeleteDocController : BaseController
{
    private readonly IDeleteDocService _deleteDocService;

    public DeleteDocController(ILogger<DeleteDocController> logger, IDeleteDocService deleteDocService)
        : base(logger)
    {
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
        var response = await _deleteDocService.DeleteDocFromBucket(request);

        return response;
    }
}