using Microsoft.AspNetCore.Mvc;

namespace EmbeddingCloudRun;

public class BaseController : ControllerBase
{
    protected readonly ILogger<BaseController> _logger;

    public BaseController(ILogger<BaseController> logger)
    {
        _logger = logger;
    }
}
