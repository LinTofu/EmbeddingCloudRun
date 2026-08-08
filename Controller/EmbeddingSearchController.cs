using Microsoft.AspNetCore.Mvc;

namespace EmbeddingCloudRun;

/// <summary>
/// Get embedding search results from GCP Vertex AI Search
/// </summary>
[ApiController]
public class EmbeddingSearchController : BaseController
{
    private readonly IEmbeddingSearchService _embeddingSearchService;

    public EmbeddingSearchController(ILogger<EmbeddingSearchController> logger, IEmbeddingSearchService embeddingSearchService)
        : base(logger)
    {
        _embeddingSearchService = embeddingSearchService;
    }

    /// <summary>
    /// POST: api/EmbeddingSearch
    /// Get embedding search results from GCP Vertex AI Search
    /// </summary>
    [HttpPost]
    [Route("api/EmbeddingSearch")]
    public async Task<ActionResult<EmbeddingSearchResponse>> EmbeddingSearch([FromBody] ApiRequest<EmbeddingSearchRequest> request)
    {
        var response = await _embeddingSearchService.EmbeddingSearch(request);
        
        return Ok(response);
    }
}
