using Microsoft.AspNetCore.Mvc;

namespace EmbeddingCloudRun.Controllers;

/// <summary>
/// Template controller for API endpoints
/// Replace "Template" with your actual controller name
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TemplateController : ControllerBase
{
    private readonly ILogger<TemplateController> _logger;

    public TemplateController(ILogger<TemplateController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// GET: api/template
    /// Get all items
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetAll()
    {
        try
        {
            _logger.LogInformation("Getting all items");
            // TODO: Implement your logic here
            return Ok(new { message = "Get all items" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting items");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// GET: api/template/{id}
    /// Get item by id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetById(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(new { error = "Id is required" });

            _logger.LogInformation("Getting item with id: {Id}", id);
            // TODO: Implement your logic here
            return Ok(new { id = id, message = "Get item by id" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting item");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// POST: api/template
    /// Create a new item
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<object>> Create([FromBody] object request)
    {
        try
        {
            if (request == null)
                return BadRequest(new { error = "Request body is required" });

            _logger.LogInformation("Creating new item");
            // TODO: Implement your logic here
            return CreatedAtAction(nameof(GetById), new { id = "new-id" }, request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating item");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// PUT: api/template/{id}
    /// Update an existing item
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] object request)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(new { error = "Id is required" });

            if (request == null)
                return BadRequest(new { error = "Request body is required" });

            _logger.LogInformation("Updating item with id: {Id}", id);
            // TODO: Implement your logic here
            return Ok(new { id = id, message = "Item updated" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating item");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// DELETE: api/template/{id}
    /// Delete an item
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(new { error = "Id is required" });

            _logger.LogInformation("Deleting item with id: {Id}", id);
            // TODO: Implement your logic here
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting item");
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
