using Microsoft.AspNetCore.Mvc;

namespace EmbeddingCloudRun;

/// <summary>
/// Get embedding search results from GCP Vertex AI Search
/// </summary>
[ApiController]
public class EmbeddingSearchController : ControllerBase
{
    private readonly ILogger<EmbeddingSearchController> _logger;
    private readonly IEmbeddingSearchService _embeddingSearchService;

    public EmbeddingSearchController(ILogger<EmbeddingSearchController> logger, IEmbeddingSearchService embeddingSearchService)
    {
        _logger = logger;
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
        try
        {
            var response = await _embeddingSearchService.EmbeddingSearch(request);
            
            return Ok(response);
        } 
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while performing embedding search.");
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
